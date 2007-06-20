// project created on 26/05/2007 at 16:02
using System;
using Gtk;
using Gdk;
using Glade;
using Utility;
using System.IO;
using System.Text;
using Mono.Posix;
using System.Reflection;

public class GladeApp
{
	
	// Form Objects
	[Glade.Widget] Gtk.Window 			topWindow;
	[Glade.Widget] Gtk.ImageMenuItem 	item_NewFile;
	[Glade.Widget] Gtk.ImageMenuItem 	item_OpenFile;
	[Glade.Widget] Gtk.ImageMenuItem 	item_SaveFile;
	[Glade.Widget] Gtk.ImageMenuItem 	item_SaveFileSim;
	[Glade.Widget] Gtk.ImageMenuItem 	item_Exit;
	[Glade.Widget] Gtk.ImageMenuItem 	item_ConnectSim;
	[Glade.Widget] Gtk.ImageMenuItem 	item_SaveSim;
	[Glade.Widget] Gtk.ImageMenuItem 	item_SaveSimFile;
	[Glade.Widget] Gtk.ImageMenuItem 	item_InfoHelp;
	[Glade.Widget] Gtk.Toolbar			toolbar1;
	[Glade.Widget] Gtk.ToolButton		tbOpenFile;
	[Glade.Widget] Gtk.ToolButton		tbSaveFile;	
	[Glade.Widget] Gtk.ToolButton		tbConnectSim;	
	[Glade.Widget] Gtk.ToolButton		tbSaveSimFile;
	[Glade.Widget] Gtk.ToolButton		tbExit;
	[Glade.Widget] Gtk.TreeView			listFile;
	[Glade.Widget] Gtk.TreeView			listSim;
	[Glade.Widget] Gtk.Statusbar		statusbar1;
	[Glade.Widget] Gtk.ProgressBar		pBar;
	[Glade.Widget] Gtk.Menu				popmenuFile;
	[Glade.Widget] Gtk.ImageMenuItem	popmenuFileAdd;
	[Glade.Widget] Gtk.ImageMenuItem	popmenuFileDel;
	[Glade.Widget] Gtk.ImageMenuItem	popmenuFileVSim;
	[Glade.Widget] Gtk.Menu				popmenuSim;
	[Glade.Widget] Gtk.ImageMenuItem	popmenuSimAdd;
	[Glade.Widget] Gtk.ImageMenuItem	popmenuSimDel;
	[Glade.Widget] Gtk.ImageMenuItem	popmenuSimVFile;
	

	// Local Objects
	string solutionTitle 	= "";
	string pcscResult		= "";
	string retErrorString 	= "";
	string readerToUse 	= "";
	string[] listReaders = new string[0];
	string[] theItems = new string[2];
	int totRecords = 0;
	int maxAlfaChar = 0;
	IPCSCinterface myCard;
	cEncoding myUtility = new cEncoding();	
	clsTreeViewHelper tvHelper = new clsTreeViewHelper();
	ListStore lsListFile;
	ListStore lsListSim;	
	Cursor watchCursor = new Cursor(CursorType.Watch);
	Cursor normalCursor = new Cursor(CursorType.LeftPtr);
	Gtk.ToolButton tbSaveFileSim = null;
	Gtk.ToolButton tbSaveSim = null;
	Gtk.Tooltips tips1 = null;
	Gtk.Tooltips tips2 = null;
	
	
	
	public static void Main (string[] args)
	{
		
		new GladeApp (args);

	}
	

	public GladeApp (string[] args) 
	{
		Application.Init ();

		// Main Window
		Glade.XML gxml = new Glade.XML (null, "mainwin.glade", "topWindow", null);
		gxml.Autoconnect (this);
		
		// Init Window Objects layout and properties
		topWindow.WindowPosition =	WindowPosition.Center;

		solutionTitle = Assembly.GetExecutingAssembly().GetName().Name.ToString() + 
	                  " v" + Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + 
	                  "." + Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + 
	                  "." + Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();

		topWindow.Title = solutionTitle;		
		this.topWindow.GdkWindow.Cursor = normalCursor;
		this.topWindow.Icon =  Gdk.Pixbuf.LoadFromResource("monosim_16.png");
		
		pBar.Visible=false;
		statusbar1.Push(1, "status bar");
		
		// Design TreeView
		listFile.Selection.Mode = SelectionMode.Multiple;
		listSim.Selection.Mode = SelectionMode.Multiple;
		theItems[0] = "description";
		theItems[1] = "phone number";
		tvHelper.TreeViewInit(ref listFile, ref lsListFile, ref theItems);
		tvHelper.TreeViewInit(ref listSim, ref lsListSim, ref theItems);
		
		
		// Icons for menu items		
		item_SaveFileSim.Image = new Gtk.Image(Pixbuf.LoadFromResource("chip_16.png"));
		item_InfoHelp.Image = new Gtk.Image(Stock.About, IconSize.Menu);
		item_ConnectSim.Image = new Gtk.Image(Stock.Connect , IconSize.Menu);
		item_NewFile.Image = new Gtk.Image(Stock.New , IconSize.Menu);
		item_OpenFile.Image = new Gtk.Image(Stock.Open, IconSize.Menu);
		item_SaveFile.Image = new Gtk.Image(Stock.SaveAs, IconSize.Menu);
		item_Exit.Image = new Gtk.Image(Stock.Quit, IconSize.Menu);
		item_SaveSimFile.Image = new Gtk.Image(Stock.SaveAs, IconSize.Menu);
		item_SaveSim.Image = new Gtk.Image(Pixbuf.LoadFromResource("chip_16.png"));

		
		tbSaveFileSim = new Gtk.ToolButton(new Gtk.Image(Pixbuf.LoadFromResource("chip_26.png")) ,"Save on sim");		
		this.tbSaveFileSim.Sensitive=false;		
		this.tbSaveFileSim.Clicked += On_tbSaveFileSim_Clicked;		
		
		this.tbSaveSim = new Gtk.ToolButton(new Gtk.Image(Pixbuf.LoadFromResource("chip_26.png")) ,"Save on sim");
		this.tbSaveSim.Sensitive=false;
		this.tbSaveSim.Clicked += On_tbSaveSim_Clicked;
		
		this.toolbar1.Insert(tbSaveFileSim, 2);
		this.toolbar1.Insert(tbSaveSim, 6);
		
		this.toolbar1.ShowAll();
		
		tips1 = new Gtk.Tooltips();
		tips2 = new Gtk.Tooltips();
		this.tbSaveFileSim.SetTooltip(tips1, "Save file phonebook on sim", null);
		this.tbSaveSim.SetTooltip(tips2, "Save sim phonebook on sim", null);

		// Create Instance of pcsc library (for windows or other os)
		if (this.IsWindows() == true)
			myCard = new clsWindowsPCSC();
		else
			myCard = new clsLinuxPCSC();
		
		// Create Context
		pcscResult = myCard.CreateContext();
		
		if (pcscResult != "")
		{
			Console.WriteLine("CreateContext = " + pcscResult);
			Application.Quit ();			
		}
		
		// Read Readers name
		pcscResult = myCard.ListReaders(out listReaders);
		
		if (pcscResult != "")
		{
			Console.WriteLine("ListReaders = " + pcscResult);
			Application.Quit ();			
		}
		
		// Delete Context
		pcscResult = myCard.DeleteContext();
		
		// No Readers founded
		if (listReaders.Length < 1)
		{
			MessageDialog Dlg = new MessageDialog(topWindow, 
		    	                                  DialogFlags.Modal,
		    	                                  MessageType.Error,
		    	                  				  ButtonsType.Ok ,
												  "No PCSC Smartcard Readers Founded ...");
			Dlg.Title = "Error";
		
			Dlg.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");			
			Dlg.Run();
			Dlg.Destroy();
			Application.Quit ();
		}
		
		
		Application.Run ();
		
	}

	// Connect the Signals defined in Glade
	private void OnWindowDeleteEvent (object sender, DeleteEventArgs a) 
	{

		// Are you sure ?		
/*		MessageDialog eDlg = new MessageDialog(topWindow, 
		                                       DialogFlags.DestroyWithParent,
		                                       MessageType.Question,
		                      			 	   ButtonsType.OkCancel ,
										 	   "Are you sure to exit ?");
		
		eDlg.Title = "Exit";		
		eDlg.TransientFor = topWindow;
		//eDlg.Icon = Gdk.Pixbuf.LoadFromResource("monoSIM.png");
		ResponseType ResDlg = (ResponseType)eDlg.Run();
		
		eDlg.Destroy();
*/
		int theResp = 0;
		clsMsgDialog eDlg = new clsMsgDialog(topWindow, DialogFlags.DestroyWithParent,ref theResp);
		
		if (theResp == 1)
		{
			eDlg = null;
			Application.Quit ();
			a.RetVal = true;
			return;
		}	
		
		eDlg = null;
		a.RetVal = false;
 		return;

	}
	
	private void OnKeyReleaseEvent (object sender, KeyReleaseEventArgs a)
	{
		Gdk.EventKey theKey = a.Event;
		
		if ( theKey.KeyValue == 65307)
			OnWindowDeleteEvent(this, new DeleteEventArgs() );
		
	}
	
	
	
	// MenuBar Events START ******************************************************

	private void On_NewFile_Press(object sender, ButtonReleaseEventArgs a)
	{
		lsListFile.Clear();
		checkListStatus();
		return;
		
	}
	
	private void On_OpenFile_Press(object sender, ButtonReleaseEventArgs a)
	{
			
			string tmpFName = "";
			string tmpItem1 = "";
			string tmpItem2 = "";
			
			// New dialog for select contacts file 
			Gtk.FileChooserDialog FileBox = new Gtk.FileChooserDialog("Select .monosim file to open...", 
			                                topWindow,
			                                FileChooserAction.Open, 
			                                "Cancel", Gtk.ResponseType.Cancel,
                                            "Open", Gtk.ResponseType.Accept);
			
			// Filter for useing only avi files
			Gtk.FileFilter myFilter = new Gtk.FileFilter(); 
			myFilter.AddPattern("*.monosim");
			myFilter.Name = "monosim files";
			FileBox.AddFilter(myFilter);
			
			// Manage result of dialog box
			FileBox.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");
			int retFileBox = FileBox.Run();
			if (retFileBox == Gtk.ResponseType.Accept.value__)
			{	
				// path of a right file returned
				tmpFName = FileBox.Filename.ToString();
				
				FileBox.Destroy();
				FileBox.Dispose();				
			}
			else
			{
				// nothing returned
				FileBox.Destroy();
				FileBox.Dispose();
				return;
			}
		
		
			// Disable controls
			this.topWindow.GdkWindow.Cursor = watchCursor;
			this.topWindow.Sensitive=false;
			
			// Update GUI...
			while (Gtk.Application.EventsPending ())
            	Gtk.Application.RunIteration ();
            
			// Read contacts in a monosim file			
			StreamReader sReader = new StreamReader(tmpFName, System.Text.Encoding.ASCII);
			
			// write listSim Rows
			this.lsListFile.Clear();			
			tmpItem1 = sReader.ReadLine();			
			while(tmpItem1 != null) 
			{
  				tmpItem2 = sReader.ReadLine();
  				this.lsListFile.AppendValues(tmpItem1, tmpItem2);
  				tmpItem1 = sReader.ReadLine();
			}
			
			sReader.Close();
			sReader = null;
			
			// Enable controls
			this.topWindow.GdkWindow.Cursor = normalCursor;
			this.topWindow.Sensitive=true;

			this.statusbar1.Push(1, "Total contacts founded on file: " + lsListFile.IterNChildren().ToString() );

			// Update GUI...
			while (Gtk.Application.EventsPending ())
            	Gtk.Application.RunIteration ();
	
			checkListStatus();
			return;
	}

	private void On_SaveFile_Press(object sender, ButtonReleaseEventArgs a)
	{
			string tmpFName = "";
			
			// New dialog for select contacts file 
			Gtk.FileChooserDialog FileBox = new Gtk.FileChooserDialog("Select .monosim file to write your file contacts...", 
			                                topWindow,
			                                FileChooserAction.Save, 
			                                "Cancel", Gtk.ResponseType.Cancel,
                                            "Save", Gtk.ResponseType.Accept);
			
			// Filter for useing only avi files
			Gtk.FileFilter myFilter = new Gtk.FileFilter(); 
			myFilter.AddPattern("*.monosim");
			myFilter.Name = "monosim files";
			FileBox.AddFilter(myFilter);
			
			// Manage result of dialog box
			FileBox.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");
			int retFileBox = FileBox.Run();
			if (retFileBox == Gtk.ResponseType.Accept.value__)
			{	
				// path of a right file returned
				tmpFName = FileBox.Filename.ToString();
				if (tmpFName.Substring(tmpFName.Length-8).ToUpper() != ".MONOSIM")
					tmpFName += ".monosim";
					
				FileBox.Destroy();
				FileBox.Dispose();
				
			}
			else
			{
				// nothing returned
				FileBox.Destroy();
				FileBox.Dispose();
				return;
			}
		
			this.topWindow.GdkWindow.Cursor = watchCursor;
			this.topWindow.Sensitive=false;
			
			// Update GUI...
			while (Gtk.Application.EventsPending ())
          		Gtk.Application.RunIteration ();
						
			// Save contacts in a monosim file			
			StreamWriter sWriter = new StreamWriter(tmpFName, false, System.Text.Encoding.ASCII);
			
			// read listFile Rows
			TreeIter tmpIter = new TreeIter();
			lsListFile.GetIterFirst(out tmpIter);
			string tmpItem1 = (string) lsListFile.GetValue(tmpIter,0);
			string tmpItem2 = (string) lsListFile.GetValue(tmpIter,1);
			sWriter.WriteLine(tmpItem1);
			sWriter.WriteLine(tmpItem2);
				
			while(lsListFile.IterNext(ref tmpIter)) 
			{
  				tmpItem1 = (string) lsListFile.GetValue(tmpIter,0);
  				tmpItem2 = (string) lsListFile.GetValue(tmpIter,1);
				sWriter.WriteLine(tmpItem1);
				sWriter.WriteLine(tmpItem2);			
			}
			
			sWriter.Close();
			sWriter = null;
			
			// Update GUI...
			while (Gtk.Application.EventsPending ())
          		Gtk.Application.RunIteration ();
			
			this.topWindow.GdkWindow.Cursor = normalCursor;
			this.topWindow.Sensitive=true;
						
			checkListStatus();
			return;
			
	}

	
	private void On_SaveFileSim_Press(object sender, ButtonReleaseEventArgs a)
	{
		writeSimContacts(lsListFile);
		return;
	}
	
	private void On_Exit_Press(object sender, ButtonReleaseEventArgs a)
	{
		OnWindowDeleteEvent(this, new DeleteEventArgs() );
	}

	private void On_ConnectSim_Press(object sender, ButtonReleaseEventArgs a)
	{
		
		clsReadersDlg newDlg = new clsReadersDlg(topWindow, 
		                                         DialogFlags.DestroyWithParent,
		                                         ref listReaders,
		                                         ref readerToUse);
		
		newDlg = null;
		
		Console.WriteLine("readerToUse = " + readerToUse);
		
		if (readerToUse.CompareTo("0") == 0 )
		{
/*			MessageDialog WDlg = new MessageDialog(topWindow, 
		    	                                   DialogFlags.Modal,
		    	                                   MessageType.Warning,
		    	                  				   ButtonsType.Ok,
												   "You must select a smartcard reader from list");
			WDlg.Title = "Error";		
			//WDlg.Icon = Gdk.Pixbuf.LoadFromResource("monoSIM.png");
			WDlg.Run();
			WDlg.Destroy();	
*/
			
			
			return;
		}
		
		
		// Disable controls
		this.topWindow.GdkWindow.Cursor = watchCursor;
		this.topWindow.Sensitive=false;
		
		// Update GUI...
			while (Gtk.Application.EventsPending ())
            	Gtk.Application.RunIteration ();
		
		// Call read function
		readSimContacts();
		
		// Disable controls
		this.topWindow.GdkWindow.Cursor = normalCursor;
		this.topWindow.Sensitive=true;
		
		
		// Update GUI...
			while (Gtk.Application.EventsPending ())
            	Gtk.Application.RunIteration ();
		
		checkListStatus();
		return;
	}

	private void On_SaveSim_Press(object sender, ButtonReleaseEventArgs a)
	{
		writeSimContacts(lsListSim);
		return;
	}
	
	
	private void On_SaveSimFile_Press(object sender, ButtonReleaseEventArgs a)
	{
			string tmpFName = "";
			
			// New dialog for select contacts file
			Gtk.FileChooserDialog FileBox = new Gtk.FileChooserDialog("Select .monosim file to write your sim contacts...", 
			                                topWindow,
			                                FileChooserAction.Save, 
			                                "Cancel", Gtk.ResponseType.Cancel,
                                            "Save", Gtk.ResponseType.Accept);
			
			// Filter for useing only avi files
			Gtk.FileFilter myFilter = new Gtk.FileFilter(); 
			myFilter.AddPattern("*.monosim");
			myFilter.Name = "monosim files";
			FileBox.AddFilter(myFilter);
			
			// Manage result of dialog box
			FileBox.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");
			int retFileBox = FileBox.Run();
			if (retFileBox == Gtk.ResponseType.Accept.value__)
			{	
				// path of a right file returned
				tmpFName = FileBox.Filename.ToString();
				if (tmpFName.Substring(tmpFName.Length-8).ToUpper() != ".MONOSIM")
					tmpFName += ".monosim";
								
				FileBox.Destroy();
				FileBox.Dispose();				
			}
			else
			{
				// nothing returned
				FileBox.Destroy();
				FileBox.Dispose();
				return;
			}
		
			// Disable controls
			this.topWindow.GdkWindow.Cursor = watchCursor;
			this.topWindow.Sensitive=false;
			
			// Update GUI...
			while (Gtk.Application.EventsPending ())
            	Gtk.Application.RunIteration ();
            
			// Save contacts in a monosim file			
			StreamWriter sWriter = new StreamWriter(tmpFName, false, System.Text.Encoding.ASCII);
			
			// read listSim Rows
			TreeIter tmpIter = new TreeIter();
			lsListSim.GetIterFirst(out tmpIter);
			string tmpItem1 = (string) lsListSim.GetValue(tmpIter,0);
			string tmpItem2 = (string) lsListSim.GetValue(tmpIter,1);
			sWriter.WriteLine(tmpItem1);
			sWriter.WriteLine(tmpItem2);
				
			while(lsListSim.IterNext(ref tmpIter)) 
			{
  				tmpItem1 = (string) lsListSim.GetValue(tmpIter,0);
  				tmpItem2 = (string) lsListSim.GetValue(tmpIter,1);
				sWriter.WriteLine(tmpItem1);
				sWriter.WriteLine(tmpItem2);			
			}
			
			sWriter.Close();
			sWriter = null;
			
			// Enable controls
			this.topWindow.GdkWindow.Cursor = normalCursor;
			this.topWindow.Sensitive=true;
			
			// Update GUI...
			while (Gtk.Application.EventsPending ())
            	Gtk.Application.RunIteration ();
			
			checkListStatus();
			return;
	}
	
	
	
	private void On_InfoHelp_Press(object sender, ButtonReleaseEventArgs a)
	{
	
	}

	// MenuBar Events END ******************************************************
	
	[GLib.ConnectBefore]
	private void On_listFile_button_press_event(object sender, ButtonPressEventArgs a)
	{
		Gdk.EventButton ev = a.Event;
		MenuItem tmpItem;
		
		if (a.Event.Button == 3)
		{
			if(popmenuFile == null)
			{
				Glade.XML gui = new Glade.XML(null,"mainwin.glade","popmenuFile",null);	
				popmenuFile = (Gtk.Menu) gui["popmenuFile"];
				popmenuFileAdd = (Gtk.ImageMenuItem ) gui["popmenuFileAdd"];
				popmenuFileDel = (Gtk.ImageMenuItem ) gui["popmenuFileDel"];
				popmenuFileVSim = (Gtk.ImageMenuItem ) gui["popmenuFileVSim"];
				
				popmenuFileAdd.ButtonReleaseEvent += On_Add_button_release_event;
				popmenuFileDel.ButtonReleaseEvent += On_Del_button_release_event;
				popmenuFileVSim.ButtonReleaseEvent += On_Move_button_release_event;
				popmenuFile.Insert(new Gtk.SeparatorMenuItem(), 2);
			}
			
			if (lsListFile.IterNChildren() == 0)
			{
				tmpItem = popmenuFileDel;
				tmpItem.Sensitive=false;
				tmpItem = popmenuFileVSim;
				tmpItem.Sensitive=false;				
			}
			else
			{
				tmpItem = popmenuFileDel;
				tmpItem.Sensitive=true;
				tmpItem = popmenuFileVSim;
				tmpItem.Sensitive=true;
			}
			
			this.popmenuFile.Popup(null, null, null,ev.Button, ev.Time);
			this.popmenuFile.ShowAll();
		}
		
		return;
	}
	
	[GLib.ConnectBefore]
	private void On_listSim_button_press_event(object sender, ButtonPressEventArgs a)
	{
		Gdk.EventButton ev = a.Event;
		MenuItem tmpItem;
		
		if (a.Event.Button == 3)
		{
			if(popmenuSim == null)
			{
				Glade.XML gui = new Glade.XML(null,"mainwin.glade","popmenuSim",null);			
				popmenuSim = (Gtk.Menu) gui["popmenuSim"];
				popmenuSimAdd = (Gtk.ImageMenuItem ) gui["popmenuSimAdd"];
				popmenuSimDel = (Gtk.ImageMenuItem ) gui["popmenuSimDel"];
				popmenuSimVFile = (Gtk.ImageMenuItem ) gui["popmenuSimVFile"];
				
				popmenuSimAdd.ButtonReleaseEvent += On_Add_button_release_event;
				popmenuSimDel.ButtonReleaseEvent += On_Del_button_release_event;
				popmenuSimVFile.ButtonReleaseEvent += On_Move_button_release_event;
				
				popmenuSim.Insert(new Gtk.SeparatorMenuItem(), 2);
			}
			
			if (lsListSim.IterNChildren() == 0)
			{
				tmpItem = popmenuSimDel;
				tmpItem.Sensitive=false;
				tmpItem = popmenuSimVFile;
				tmpItem.Sensitive=false;
				
			}
			else
			{
				tmpItem = popmenuSimDel;
				tmpItem.Sensitive=true;
				tmpItem = popmenuSimVFile;
				tmpItem.Sensitive=true;
			}
			
			this.popmenuSim.Popup(null, null, null,ev.Button, ev.Time);
			this.popmenuSim.ShowAll();
		}
		
		return;
	}
	
	// ToolBar Events START ******************************************************
	private void On_tbOpenFile_Clicked(object sender, EventArgs a)
	{
		On_OpenFile_Press(this, new ButtonReleaseEventArgs());
	}

	private void On_tbSaveFile_Clicked(object sender, EventArgs a)
	{
		On_SaveFile_Press(this, new ButtonReleaseEventArgs());	
	}

	private void On_tbSaveFileSim_Clicked(object sender, EventArgs a)
	{
		On_SaveFileSim_Press(this, new ButtonReleaseEventArgs());	
	}
	
	
	private void On_tbConnectSim_Clicked(object sender, EventArgs a)
	{
		On_ConnectSim_Press(this, new ButtonReleaseEventArgs());
	}

	private void On_tbSaveSimFile_Clicked(object sender, EventArgs a)
	{
		On_SaveSimFile_Press(this, new ButtonReleaseEventArgs());
	}

	private void On_tbSaveSim_Clicked(object sender, EventArgs a)
	{
		On_SaveSim_Press(this, new ButtonReleaseEventArgs());
	}


	private void On_tbExit_Clicked(object sender, EventArgs a)
	{
		OnWindowDeleteEvent(this, new DeleteEventArgs() );
	}
	
	// ToolBar Events END ******************************************************

	
		/// <summary>
	    /// Provide to see if we're on windows
	    /// </summary>
		private bool IsWindows()
		{
		    PlatformID platform = Environment.OSVersion.Platform;	    
		    return (platform == PlatformID.Win32NT | platform == PlatformID.Win32Windows |
		            platform == PlatformID.Win32S | platform == PlatformID.WinCE);    
		}
	
		

		private void On_Del_button_release_event(object sender, ButtonReleaseEventArgs args)
		{
			TreeSelection theSelection;
			TreeModel tModel;
			TreeIter theIter;
			Console.WriteLine("sender = " + ((Gtk.ImageMenuItem)sender).Name );
			
			if(((Gtk.ImageMenuItem)sender).Name == "popmenuFileDel")
				theSelection = listFile.Selection;
			else
				theSelection = listSim.Selection;			
			
			Array treePaths = theSelection.GetSelectedRows(out tModel);
			if(tModel != null)
    			tModel = null;
			int maxRows = treePaths.Length - 1;
			
			for(int j=maxRows; j>=0; j--)
			{
				TreePath tPath = (TreePath)(treePaths.GetValue(j));
				
				if(((Gtk.ImageMenuItem)sender).Name == "popmenuFileDel")
				{
					Console.WriteLine("tPath = " + tPath.ToString() );
					if(this.lsListFile.GetIter(out theIter, tPath))
						this.lsListFile.Remove(ref theIter);
				}
				else
				{
					Console.WriteLine("tPath = " + tPath.ToString() );
					if(this.lsListSim.GetIter(out theIter, tPath))
						this.lsListSim.Remove(ref theIter);				
				}
			}
			
			checkListStatus();
		}
		
		
		private void On_Move_button_release_event(object sender, ButtonReleaseEventArgs args)
		{
			TreeSelection theSelection;
			TreeModel tModel;
			TreeIter theIter;
			
			if(((Gtk.ImageMenuItem)sender).Name == "popmenuFileVSim")
				theSelection = listFile.Selection;
			else
				theSelection = listSim.Selection;			
			
			Array treePaths = theSelection.GetSelectedRows(out tModel);
			if(tModel != null)
    			tModel = null;
			int maxRows = treePaths.Length - 1;
			
			for(int j=0; j<=maxRows; j++)
			{
				TreePath tPath = (TreePath)(treePaths.GetValue(j));
				
				if(((Gtk.ImageMenuItem)sender).Name == "popmenuFileVSim")
				{
					Console.WriteLine("tPath = " + tPath.ToString() );
					if(this.lsListFile.GetIter(out theIter, tPath))
					{						
						TreeIter tmpIter = this.lsListSim.Append();						
						this.lsListSim.SetValue(tmpIter,0, this.lsListFile.GetValue(theIter,0));
						this.lsListSim.SetValue(tmpIter,1, this.lsListFile.GetValue(theIter,1));
					}
				}
				else
				{
					Console.WriteLine("tPath = " + tPath.ToString() );
					if(this.lsListSim.GetIter(out theIter, tPath))
					{
						TreeIter tmpIter = this.lsListFile.Append();						
						this.lsListFile.SetValue(tmpIter,0, this.lsListSim.GetValue(theIter,0));
						this.lsListFile.SetValue(tmpIter,1, this.lsListSim.GetValue(theIter,1));
					}
				}
			}

		}

		
		
		private void On_Add_button_release_event(object sender, ButtonReleaseEventArgs args)		
		{
			string[] strNI = new string[0];
			clsNewItemDlg nI = new clsNewItemDlg(topWindow, DialogFlags.DestroyWithParent,ref strNI);
			
			if (strNI.Length == 0)
				return;
			
			if(((Gtk.ImageMenuItem)sender).Name == "popmenuFileAdd")
			{
				lsListFile.AppendValues(strNI[0], strNI[1]);
			}
			else
			{
				lsListSim.AppendValues(strNI[0], strNI[1]);
			}
			
			checkListStatus();
			return;
		}
		
		
		
		
		private void readSimContacts()
		{
			string RLen = "";
			string FLen = "";
			string tmpCommand = "";
			string tmpResp = "";
			string theName = "";
			string thePhone = "";			
			int filledRecords = 0;
			
			
			
			this.topWindow.Sensitive = false;
			this.lsListSim.Clear();
			this.pBar.Fraction = 0;
			
			this.pBar.Visible=true;
			this.statusbar1.Push(1, "open contacts file on sim...");
			
			// Update GUI...
			while (Gtk.Application.EventsPending ())
              Gtk.Application.RunIteration ();
			
			// Manage Creation of context
			pcscResult = this.myCard.CreateContext();
			if (pcscResult != "")
			{
				endReading("ERROR: " + pcscResult);
				return;
			}
			
			// Manage SmartCard Connection
			pcscResult = this.myCard.ConnectSmartCard(readerToUse, ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR: " + retErrorString);
				return;
			}
			
			
			// Read PIN Status
			pcscResult = this.parseCommand("A0A40000023F00", "9F??", ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR: " + retErrorString);
				return;
			}
			
			pcscResult = this.parseCommand("A0C00000" + pcscResult.Substring(2,2) , "", ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR: " + retErrorString);
				return;
			}
			
			// Check PIN
			if ( Convert.ToInt32(pcscResult.Substring(26, 1), 16) < 8)
			{
				endReading("ERROR: PIN is enabled. To use monoSIM you must disable it");
				return;
			}
				
			
			// Read Size
			pcscResult = this.parseCommand("A0A40000027F10", "9F??", ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR: " + retErrorString);
				return;
			}
			
			pcscResult = this.parseCommand("A0A40000026F3A", "9F??", ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR: " + retErrorString);
				return;
			}
			
			pcscResult = this.parseCommand("A0C00000" + pcscResult.Substring(2,2) , "", ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR: " + retErrorString);
				return;
			}
			
			RLen = pcscResult.Substring(28,2);
        	FLen = pcscResult.Substring(4, 4);
        	maxAlfaChar = Convert.ToInt32(RLen,16) - 14;
        	totRecords = (int)(Convert.ToInt32(FLen,16) / Convert.ToInt32(RLen,16));
			        	
        	this.pBar.Adjustment.Lower=0;
        	this.pBar.Adjustment.Upper= (double)totRecords;
			
        	
        	// Read the Sim contacts
        	for (int k=1; k<=totRecords; k++)
        	{
        		tmpCommand = "A0B2" + k.ToString("X2") + "04" + RLen;
        		tmpResp = new string( Convert.ToChar("?") ,  (int)(Convert.ToInt32(RLen,16) * 2));
        		tmpResp = tmpResp + "9000";
        		
        		pcscResult = this.parseCommand(tmpCommand, tmpResp, ref retErrorString);
        		if (retErrorString != "")
				{
					endReading("ERROR: " + retErrorString);
					return;
				}
				
        		
        		theName="";
        		thePhone="";
        		
        		if (pcscResult.Substring(0, pcscResult.Length-4) != new string( Convert.ToChar("F"), (pcscResult.Length-4))) 
        		{
	        		// Upgrade list
	        		filledRecords ++;
	        		decodeRecord(pcscResult.Substring(0, pcscResult.Length-4), ref theName, ref thePhone);
	        		lsListSim.AppendValues(theName, thePhone);        			
        		}
        		
        		
        		this.pBar.Adjustment.Value = (double)k;
        		this.statusbar1.Push(1, "Read phonebook position " + k.ToString("D3"));
        		
        		// Update GUI...
				while (Gtk.Application.EventsPending ())
              		Gtk.Application.RunIteration ();
				
        	}
        	
        	this.statusbar1.Push(1, "Total contacts founded on sim: " + filledRecords.ToString());
        	
			// Manage Delete of context
			pcscResult = this.myCard.DeleteContext();
			if (pcscResult != "")
			{
				endReading("ERROR: " + pcscResult);
				return;
			}
		
			// Update GUI...
			while (Gtk.Application.EventsPending ())
              Gtk.Application.RunIteration ();
				
			// Reactive the Main Window			
			this.pBar.Visible=false;
			this.topWindow.Sensitive = true;

		}

		
		
		private void writeSimContacts(ListStore sender)
		{
			// Write on sim from TreeView List
			string RLen = "";
			string FLen = "";
			string tmpCommand = "";
			string tmpResp = "";
			string theName = "";
			string thePhone = "";
			string emptyStr = "";
			TreeIter theRow;			
			int EmptyPosition = 0;
			int posToWrite = 0;

			clsReadersDlg newDlg = new clsReadersDlg(topWindow, 
			                                         DialogFlags.DestroyWithParent,
			                                         ref listReaders,
			                                         ref readerToUse);
		
			newDlg = null;
			if (readerToUse.CompareTo("0") == 0 )
			{
				endReading("...");
				return;
			}
			
			this.topWindow.Sensitive = false;
			this.pBar.Fraction = 0;
			
			this.pBar.Visible=true;
			this.statusbar1.Push(1, "write contacts file on sim...");
			
			// Update GUI...
			while (Gtk.Application.EventsPending ())
              Gtk.Application.RunIteration ();
			
			// Manage Creation of context
			pcscResult = this.myCard.CreateContext();
			if (pcscResult != "")
			{
				endReading("ERROR: " + pcscResult);
				return;
			}
			
			// Manage SmartCard Connection
			pcscResult = this.myCard.ConnectSmartCard(readerToUse, ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR: " + retErrorString);
				return;
			}
			
			
			// Read PIN Status
			pcscResult = this.parseCommand("A0A40000023F00", "9F??", ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR [P1]: " + retErrorString);
				return;
			}
			
			pcscResult = this.parseCommand("A0C00000" + pcscResult.Substring(2,2) , "", ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR [P2]: " + retErrorString);
				return;
			}
			
			// Check PIN
			if ( Convert.ToInt32(pcscResult.Substring(26, 1), 16) < 8)
			{
				endReading("ERROR: PIN is enabled. To use monoSIM you must disable it");
				return;
			}
				
			
			// Read Size
			pcscResult = this.parseCommand("A0A40000027F10", "9F??", ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR [P3]: " + retErrorString);
				return;
			}
			
			pcscResult = this.parseCommand("A0A40000026F3A", "9F??", ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR [P4]: " + retErrorString);
				return;
			}
			
			pcscResult = this.parseCommand("A0C00000" + pcscResult.Substring(2,2) , "", ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR [P5]: " + retErrorString);
				return;
			}
			
			RLen = pcscResult.Substring(28,2);
        	FLen = pcscResult.Substring(4, 4);
        	maxAlfaChar = Convert.ToInt32(RLen,16) - 14;
        	totRecords = (int)(Convert.ToInt32(FLen,16) / Convert.ToInt32(RLen,16));
			emptyStr = new string( Convert.ToChar("F") ,  (int)(Convert.ToInt32(RLen,16) * 2));
			emptyStr += "9000";
        	
			this.pBar.Adjustment.Lower=0;
        	this.pBar.Adjustment.Upper= (double)totRecords;
			
        	
        	// Read the Sim contacts
			this.statusbar1.Push(1, "verify empty positions on sim...");
			
			// Update GUI...
			while (Gtk.Application.EventsPending ())
              Gtk.Application.RunIteration ();
			
        	EmptyPosition = 0;
        	for (int k=1; k<=totRecords; k++)
        	{
        		tmpCommand = "A0B2" + k.ToString("X2") + "04" + RLen;
        		tmpResp = new string( Convert.ToChar("F") ,  (int)(Convert.ToInt32(RLen,16) * 2));
        		tmpResp = tmpResp + "9000";
        		
        		pcscResult = this.parseCommand(tmpCommand, tmpResp, ref retErrorString);
        		if (retErrorString == "")
					EmptyPosition ++;				
        		
        		this.pBar.Adjustment.Value = (double)k;
        		this.statusbar1.Push(1, "read phonebook position " + k.ToString("D3"));
        		
        		// Update GUI...
				while (Gtk.Application.EventsPending ())
              		Gtk.Application.RunIteration ();
        	}
        	
        	if(EmptyPosition < sender.IterNChildren())
			{
				pcscResult = this.myCard.DeleteContext();
        		endReading("ERROR: insufficient space on sim...");
				return;
			}
        	
        	this.pBar.Adjustment.Lower=0;
        	this.statusbar1.Push(1, "write sim...");
        	
        	for (int k=1; k<=totRecords; k++)
        	{
        		tmpCommand = "A0B2" + k.ToString("X2") + "04" + RLen;
        		tmpResp = new string( Convert.ToChar("?") ,  (int)(Convert.ToInt32(RLen,16) * 2));
        		tmpResp = tmpResp + "9000";
        		retErrorString = "";
        		
        		pcscResult = this.parseCommand(tmpCommand, tmpResp, ref retErrorString);
       			
        		// Empty Position...
        		if (retErrorString == "")
        		{
        			if (pcscResult == emptyStr)
        			{
	        			sender.GetIterFromString(out theRow, posToWrite.ToString());
	        			tmpCommand = "A0DC" + k.ToString("X2") + "04" + RLen;
	        			
	        			// Write Function
	        			theName = (string)sender.GetValue(theRow,0);
	        			thePhone = (string)sender.GetValue(theRow,1);
	        			
	        			tmpCommand += getRecordToWrite(theName, thePhone);	        			
	        			Console.WriteLine("APDU " + posToWrite.ToString("D2") + " = " + tmpCommand);
	        			posToWrite ++;
	        			
	        			if (sender.IterNChildren() <= posToWrite)
	        				break;
	        			
        			}
        		}
        		else
        		{

        			endReading("ERROR [P6]: " + retErrorString);
					return;
        		}
        		
        		this.pBar.Adjustment.Value = (double)k;
        		this.statusbar1.Push(1, "update phonebook position " + k.ToString("D3"));
        		
        		// Update GUI...
				while (Gtk.Application.EventsPending ())
              		Gtk.Application.RunIteration ();
        	}
        	
        	
        	this.statusbar1.Push(1, "sim card updated...");
        	
			// Manage Delete of context
			pcscResult = this.myCard.DeleteContext();
			if (pcscResult != "")
			{
				endReading("ERROR: " + pcscResult);
				return;
			}
		
			// Update GUI...
			while (Gtk.Application.EventsPending ())
              Gtk.Application.RunIteration ();
				
			// Reactive the Main Window			
			this.pBar.Visible=false;
			this.topWindow.Sensitive = true;
			
			
			return;
		}
		
		
		
		private string parseCommand(string theCommand, string theResponse, ref string errorTmp)
		{
			string retV = "";
			errorTmp = retV;
			pcscResult = myCard.SendCommand(theCommand, ref retV);
			if (retV != "")
			{
				errorTmp = retV;
				return "";
			}
			

			if ((pcscResult.Length != theResponse.Length) && (theResponse.Length > 0))
			{
				errorTmp = "length not attempted: " + pcscResult;
				return "";
			}
			
			if (theResponse.Length > 0)
			{
				for (int n=0; n<pcscResult.Length; n++)
				{
					if (theResponse[n] != Convert.ToChar("?"))
					{
						if (pcscResult[n] != theResponse[n])
						{	
							Console.WriteLine("Char n. " + n.ToString() + " pcscResult[n]=" + pcscResult[n] + " theResponse[n]=" +theResponse[n]);
							errorTmp = "response not attempted: " + pcscResult;
							return "";
						}
					}
				}
			}
			
			return pcscResult;			
		}
		
		
		
		private void endReading(string theMessage)
		{
				this.statusbar1.Push(1,theMessage);
				this.pBar.Visible=false;
				this.topWindow.Sensitive = true;

				// Update GUI...
				while (Gtk.Application.EventsPending ())
	              Gtk.Application.RunIteration ();
				
				return;
		}
		
		private string SwapTel(string numTel, string lenTel)
		{
			int theLen = Convert.ToInt32(lenTel,16);
			string N1 = "";
			
			for (int k=0; k<(theLen*2) - 2; k +=2)
				N1 +=  (numTel.Substring((k+1),1) + numTel.Substring(k, 1));
			
			N1 = N1.Replace("A", "*");
			N1 = N1.Replace("B", "#");
			N1 = N1.Replace("F", "");
			
			return N1;
		}
		
		private void decodeRecord(string dataIN, ref string out1, ref string out2)
		{
			int lenAlpha = (dataIN.Length-28)/2;
			string alphaID = dataIN.Substring(0, (lenAlpha * 2));
        	string numLength = dataIN.Substring((lenAlpha * 2), 2);
        	string TonNpi = dataIN.Substring((lenAlpha * 2) + 2, 2);
        	string DialNum = dataIN.Substring((lenAlpha * 2) + 4, 20);
        	string ConfigId = dataIN.Substring((lenAlpha * 2) + 24, 2);
        	string EXT1Rec = dataIN.Substring((lenAlpha * 2) + 26, 2);
        	
        	Console.WriteLine ("alphaID 1 = " + alphaID);
        	alphaID = myUtility.getAsciiFromHex(alphaID);
        	alphaID = alphaID.Replace(((char)255).ToString() , "");
        	DialNum = SwapTel(DialNum, numLength); 
        	Console.WriteLine (alphaID + " - " + DialNum);
        	out1 = alphaID;
        	out2 = DialNum;
			return;
		}

		
		private void checkListStatus()
		{
			if (this.lsListFile.IterNChildren() == 0)
			{
				this.item_SaveFile.Sensitive=false;
				this.item_SaveFileSim.Sensitive=false;
				this.tbSaveFile.Sensitive = false;
				this.tbSaveFileSim.Sensitive = false;
			}else{
				this.item_SaveFile.Sensitive = true;
				this.item_SaveFileSim.Sensitive = true;
				this.tbSaveFile.Sensitive = true;
				this.tbSaveFileSim.Sensitive = true;
			}

			if (this.lsListSim.IterNChildren() == 0)
			{
				this.item_SaveSim.Sensitive=false;
				this.item_SaveSimFile.Sensitive=false;
				this.tbSaveSim.Sensitive = false;
				this.tbSaveSimFile.Sensitive = false;				
			}else{
				this.item_SaveSim.Sensitive=true;
				this.item_SaveSimFile.Sensitive=true;
				this.tbSaveSim.Sensitive = true;
				this.tbSaveSimFile.Sensitive = true;				
			}

		}
		
		public static bool IsOdd(int i)
			{	return (i & 1) == 1;	}	
		
		
		private string getRecordToWrite(string description, string phonenumber)
		{
			string AlphaNew = "";
			string TelNew = "";
			string TelNewOut = "";
			string phoneLen = "";
			string outStr = "";
			int theL = 0;
			
			AlphaNew = myUtility.getHexFromAscii(description);			
			AlphaNew = AlphaNew.PadLeft(maxAlfaChar * 2, Convert.ToChar("F"));
			
			TelNew = phonenumber.Replace("*", "A");
			TelNew = TelNew.Replace("*", "A");
        	
			theL = TelNew.Length;
			
			if ( IsOdd(theL) == true )
				theL = ( ( (theL + (int)1) / 2 ) + 1 );			    
			else
				theL = ( (theL / 2) + (int)1);
			
			phoneLen = theL.ToString("X2");
			TelNew = TelNew.PadLeft(20, Convert.ToChar("F"));
			
			for (int n=0; n<TelNew.Length; n +=2)
				TelNewOut = TelNewOut + TelNew.Substring(n+1,1) + TelNew.Substring(n,1);
			
			outStr = AlphaNew + phoneLen + "00" + TelNewOut + "FFFF";
				
			return outStr;		
		}
		
}

