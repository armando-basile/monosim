
// project created on 26/05/2007 at 16:02
using System;
using Gtk;
using Gdk;
using Glade;
using Utility;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

public class GladeApp
{
	
	// Form Objects
	[Glade.Widget] Gtk.Window 			topWindow;
	[Glade.Widget] Gtk.MenuItem 		menu_File;
	[Glade.Widget] Gtk.MenuItem 		menu_Sim;
	[Glade.Widget] Gtk.MenuItem 		menu_Languages;
	[Glade.Widget] Gtk.Menu	 			menu_Languages_Items;
	[Glade.Widget] Gtk.MenuItem 		menu_Help;
	[Glade.Widget] Gtk.ImageMenuItem 	item_NewFile;
	[Glade.Widget] Gtk.ImageMenuItem 	item_OpenFile;
	[Glade.Widget] Gtk.ImageMenuItem 	item_SaveFile;
	[Glade.Widget] Gtk.ImageMenuItem 	item_SaveFileSim;
	[Glade.Widget] Gtk.ImageMenuItem 	item_Exit;
	[Glade.Widget] Gtk.ImageMenuItem 	item_ConnectSim;
	[Glade.Widget] Gtk.ImageMenuItem 	item_Pin;
	[Glade.Widget] Gtk.ImageMenuItem 	item_SaveSim;
	[Glade.Widget] Gtk.ImageMenuItem 	item_DeleteSim;
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
	[Glade.Widget] Gtk.Label			lblFile;
	[Glade.Widget] Gtk.Label			lblSim;
	
	

	// Local Objects
	string solutionTitle 	= "";
	string pcscResult		= "";
	string retErrorString 	= "";
	string applicationPath = "";	
	string readerToUse 	= "";
	string defaultLanguage = "";
	string[] listReaders = new string[0];
	string[] theItems = new string[2];
	int totRecords = 0;
	int maxAlfaChar = 0;
	clsPCSC myCard;
	clsLanguageHelper translator = null;
	cEncoding myUtility = new cEncoding();	
	clsTreeViewHelper tvHelper = new clsTreeViewHelper();
	ListStore lsListFile;
	ListStore lsListSim;	
	// Cursor watchCursor = new Cursor(CursorType.Watch);
	// Cursor normalCursor = new Cursor(CursorType.LeftPtr);
	Gtk.ToolButton tbSaveFileSim = null;
	Gtk.ToolButton tbSaveSim = null;
	Gtk.Tooltips tips1 = null;
	Gtk.Tooltips tips2 = null;
	Gtk.RadioMenuItem languageItemMenu = null;
	

	
	public static void Main (string[] args)
	{
		
		new GladeApp (args);

	}
	

	public GladeApp (string[] args) 
	{
		Application.Init ();
		Console.WriteLine("application Init");
		
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
		applicationPath = Assembly.GetExecutingAssembly().Location.ToString();
		applicationPath = applicationPath.Replace(System.IO.Path.GetFileName(applicationPath), "");
		applicationPath += "languages";
		Console.WriteLine("applicationPath = " + applicationPath);
		
		translator = new clsLanguageHelper(applicationPath);
		string[] totLanguages = translator.getLanguagesName();
		
		// defaultLanguage = "english";
		clsSerializationHelper.load(".monosim");
		defaultLanguage = clsSettings.defaultLanguage;
		
		// Debug Name Languages
		if (totLanguages != null)
		{
			for (int h=0; h<totLanguages.Length; h++)
			{
				languageItemMenu = new RadioMenuItem(Path.GetFileName(totLanguages[h]));
				
				if (h>0)
					languageItemMenu.Group = ((RadioMenuItem)(menu_Languages_Items.Children[0])).Group;
				
				languageItemMenu.Active=false;
				if (defaultLanguage == Path.GetFileName(totLanguages[h]))
					languageItemMenu.Active=true;
				
				languageItemMenu.ButtonReleaseEvent += On_Change_Language;
				menu_Languages_Items.Append(languageItemMenu);
				
			}
			
			menu_Languages_Items.ShowAll();
			menu_Languages.ShowAll();
		}
		
		
		
		//this.topWindow.GdkWindow.Cursor = normalCursor;
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
		item_DeleteSim.Image = new Gtk.Image(Stock.Delete, IconSize.Menu);
		item_InfoHelp.Image = new Gtk.Image(Stock.About, IconSize.Menu);
		item_ConnectSim.Image = new Gtk.Image(Stock.Connect , IconSize.Menu);
		item_NewFile.Image = new Gtk.Image(Stock.New , IconSize.Menu);
		item_OpenFile.Image = new Gtk.Image(Stock.Open, IconSize.Menu);
		item_SaveFile.Image = new Gtk.Image(Stock.SaveAs, IconSize.Menu);
		item_Exit.Image = new Gtk.Image(Stock.Quit, IconSize.Menu);
		item_SaveSimFile.Image = new Gtk.Image(Stock.SaveAs, IconSize.Menu);
		item_SaveSim.Image = new Gtk.Image(Pixbuf.LoadFromResource("chip_16.png"));
		
		item_Pin.Image = new Gtk.Image(Pixbuf.LoadFromResource("lock_16.png"));
		
		
		tbSaveFileSim = new Gtk.ToolButton(new Gtk.Image(Pixbuf.LoadFromResource("chip_26.png")) ,"Save on sim");		
		this.tbSaveFileSim.Sensitive=false;	
		this.item_DeleteSim.Sensitive=false;
		this.tbSaveFileSim.Clicked += On_tbSaveFileSim_Clicked;		
		
		this.tbSaveSim = new Gtk.ToolButton(new Gtk.Image(Pixbuf.LoadFromResource("chip_26.png")) ,"Save on sim");
		this.tbSaveSim.Sensitive=false;
		this.tbSaveSim.Clicked += On_tbSaveSim_Clicked;
		
		this.toolbar1.Insert(tbSaveFileSim, 2);
		this.toolbar1.Insert(tbSaveSim, 6);
		
		this.toolbar1.ToolbarStyle = ToolbarStyle.Icons;
		this.toolbar1.ShowAll();
		
		tips1 = new Gtk.Tooltips();
		tips2 = new Gtk.Tooltips();
		this.tbSaveFileSim.SetTooltip(tips1, "Save file phonebook on sim", null);
		this.tbSaveSim.SetTooltip(tips2, "Save sim phonebook on sim", null);

		// Set Language Items
		changeLanguage(defaultLanguage);


		// Create Instance of pcsc library (for windows or other os)
		myCard = new clsPCSC();
		
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
												  translator.readTranslatedString(103));
			Dlg.Title = "Error";
		
			Dlg.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");			
			Dlg.Run();
			Dlg.Destroy();			
			Application.Quit ();
			return;
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
		a.RetVal = true;
		
		int theResp = 0;
		clsMsgDialog eDlg = new clsMsgDialog(topWindow, DialogFlags.DestroyWithParent,ref theResp, ref translator);
		
		if (theResp == 1)
		{
			eDlg = null;
			Application.Quit ();			
			return;
		}	
		
//		eDlg = null;
//		a.RetVal = false;
//		return;

	}
	
	private void OnKeyReleaseEvent (object sender, KeyReleaseEventArgs a)
	{
		/*
		
		Gdk.EventKey theKey = a.Event;
		
		if ( theKey.KeyValue == 65307)
			OnWindowDeleteEvent(this, new DeleteEventArgs() );
		
		*/
		
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
			Gtk.FileChooserDialog FileBox = new Gtk.FileChooserDialog(translator.readTranslatedString(106) , 
			                                topWindow,
			                                FileChooserAction.Open, 
			                                translator.readTranslatedString(36), Gtk.ResponseType.Cancel,
                                            translator.readTranslatedString(37), Gtk.ResponseType.Accept);
			
			// Filter for useing only avi files
			Gtk.FileFilter myFilter = new Gtk.FileFilter(); 
			myFilter.AddPattern("*.monosim");
			myFilter.Name = "monosim files";
			FileBox.AddFilter(myFilter);
			
			// Manage result of dialog box
			FileBox.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");
			int retFileBox = FileBox.Run();
			if ((ResponseType)retFileBox == Gtk.ResponseType.Accept)
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
			//this.topWindow.GdkWindow.Cursor = watchCursor;
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
			//this.topWindow.GdkWindow.Cursor = normalCursor;
			this.topWindow.Sensitive=true;

			this.statusbar1.Push(1, translator.readTranslatedString(101)  + " " + lsListFile.IterNChildren().ToString() );

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
			Gtk.FileChooserDialog FileBox = new Gtk.FileChooserDialog(translator.readTranslatedString(107), 
			                                topWindow,
			                                FileChooserAction.Save, 
			                                translator.readTranslatedString(36), Gtk.ResponseType.Cancel,
                                            translator.readTranslatedString(38), Gtk.ResponseType.Accept);
			
			// Filter for useing only avi files
			Gtk.FileFilter myFilter = new Gtk.FileFilter(); 
			myFilter.AddPattern("*.monosim");
			myFilter.Name = "monosim files";
			FileBox.AddFilter(myFilter);
			
			// Manage result of dialog box
			FileBox.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");
			int retFileBox = FileBox.Run();
			if ((ResponseType)retFileBox == Gtk.ResponseType.Accept)
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
		
			//this.topWindow.GdkWindow.Cursor = watchCursor;
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
			
			//this.topWindow.GdkWindow.Cursor = normalCursor;
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

	private void On_Pin_Change_Press(object sender, ButtonReleaseEventArgs a)
	{
		// Select reader to use
		clsReadersDlg newDlg = new clsReadersDlg(topWindow, 
		                                         DialogFlags.DestroyWithParent,
		                                         ref listReaders,
		                                         ref readerToUse, 
		                                         ref translator);
		
		newDlg = null;
		
		Console.WriteLine("readerToUse = " + readerToUse);
		
		if (readerToUse.CompareTo("0") == 0 )
			return;
		
		// If a reader was selected...
		bool Pin1;
		string retErrorPin="";
		
		this.topWindow.Sensitive=false;
	
		// Update GUI...
		while (Gtk.Application.EventsPending ())
    		Gtk.Application.RunIteration ();
	
		retErrorPin = PIN1_Status(out Pin1);
		
		if (retErrorPin != "")
		{
			this.topWindow.Sensitive=true;
		
			// Update GUI...
			while (Gtk.Application.EventsPending ())
        		Gtk.Application.RunIteration ();
			return;
		}
		
		Console.WriteLine("PIN1 Status = " + Pin1.ToString());
		int RetWin = 0;
		string RetChange = "";
		
		if (Pin1 == true)
		{
			clsMsgDialog PinMessage = new clsMsgDialog(topWindow, 
			                                           DialogFlags.DestroyWithParent,
			                                           ref RetWin,
			                                           "PIN1", 
			                                           translator.readTranslatedString(130));
			if (RetWin == 1)
				RetChange = ChangePin(false);
		}
		else
		{
			clsMsgDialog PinMessage = new clsMsgDialog(topWindow, 
			                                           DialogFlags.DestroyWithParent,
			                                           ref RetWin,
			                                           "PIN1", 
			                                           translator.readTranslatedString(131));
			if (RetWin == 1)
				RetChange = ChangePin(true);			
			
		}
		
		
		this.topWindow.Sensitive=true;
		
		// Update GUI...
		while (Gtk.Application.EventsPending ())
    		Gtk.Application.RunIteration ();
		
		
		
	}
	
	private void On_ConnectSim_Press(object sender, ButtonReleaseEventArgs a)
	{
		
		clsReadersDlg newDlg = new clsReadersDlg(topWindow, 
		                                         DialogFlags.DestroyWithParent,
		                                         ref listReaders,
		                                         ref readerToUse, 
		                                         ref translator);
		
		newDlg = null;
		
		Console.WriteLine("readerToUse = " + readerToUse);
		
		if (readerToUse.CompareTo("0") == 0 )
			return;
		
		
		// Disable controls
		//this.topWindow.GdkWindow.Cursor = watchCursor;
		this.topWindow.Sensitive=false;
		
		// Update GUI...
			while (Gtk.Application.EventsPending ())
            	Gtk.Application.RunIteration ();
		
		// Call read function
		readSimContacts();
		
		// Disable controls
		//this.topWindow.GdkWindow.Cursor = normalCursor;
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
			Gtk.FileChooserDialog FileBox = new Gtk.FileChooserDialog(translator.readTranslatedString(108), 
			                                topWindow,
			                                FileChooserAction.Save, 
			                                translator.readTranslatedString(36), Gtk.ResponseType.Cancel,
                                            translator.readTranslatedString(38), Gtk.ResponseType.Accept);
			
			// Filter for useing only avi files
			Gtk.FileFilter myFilter = new Gtk.FileFilter(); 
			myFilter.AddPattern("*.monosim");
			myFilter.Name = "monosim files";
			FileBox.AddFilter(myFilter);
			
			// Manage result of dialog box
			FileBox.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");
			int retFileBox = FileBox.Run();
			if ((ResponseType)retFileBox == Gtk.ResponseType.Accept)
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
			//this.topWindow.GdkWindow.Cursor = watchCursor;
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
			//this.topWindow.GdkWindow.Cursor = normalCursor;
			this.topWindow.Sensitive=true;
			
			// Update GUI...
			while (Gtk.Application.EventsPending ())
            	Gtk.Application.RunIteration ();
			
			checkListStatus();
			return;
	}
	
	
	
	private void On_InfoHelp_Press(object sender, ButtonReleaseEventArgs a)
	{
        clsInfoDialog newInfo = new clsInfoDialog(topWindow, DialogFlags.DestroyWithParent, ref translator);
        
	}

	// MenuBar Events END ******************************************************
	
	[GLib.ConnectBefore]
	private void On_listFile_button_press_event(object sender, ButtonPressEventArgs a)
	{
		Gdk.EventButton ev = a.Event;
		MenuItem tmpItem;
		
		if (a.Event.Button == 3)
		{
			
			Glade.XML gui = new Glade.XML(null,"mainwin.glade","popmenuFile",null);	
			popmenuFile = (Gtk.Menu) gui["popmenuFile"];
			popmenuFileAdd = (Gtk.ImageMenuItem ) gui["popmenuFileAdd"];
			popmenuFileDel = (Gtk.ImageMenuItem ) gui["popmenuFileDel"];
			popmenuFileVSim = (Gtk.ImageMenuItem ) gui["popmenuFileVSim"];
			
			((AccelLabel)(((ImageMenuItem)popmenuFileAdd).Children[0])).Text = translator.readTranslatedString(32);
			((AccelLabel)(((ImageMenuItem)popmenuFileDel).Children[0])).Text = translator.readTranslatedString(33);
			((AccelLabel)(((ImageMenuItem)popmenuFileVSim).Children[0])).Text = translator.readTranslatedString(34);
			
			Console.WriteLine("debug translator..." + translator.readTranslatedString(32));
			
			popmenuFileAdd.ButtonReleaseEvent += On_Add_button_release_event;
			popmenuFileDel.ButtonReleaseEvent += On_Del_button_release_event;
			popmenuFileVSim.ButtonReleaseEvent += On_Move_button_release_event;
			popmenuFile.Insert(new Gtk.SeparatorMenuItem(), 2);
			
			
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
			
			Glade.XML gui = new Glade.XML(null,"mainwin.glade","popmenuSim",null);			
			popmenuSim = (Gtk.Menu) gui["popmenuSim"];
			popmenuSimAdd = (Gtk.ImageMenuItem ) gui["popmenuSimAdd"];
			popmenuSimDel = (Gtk.ImageMenuItem ) gui["popmenuSimDel"];
			popmenuSimVFile = (Gtk.ImageMenuItem ) gui["popmenuSimVFile"];
			
			((AccelLabel)(((ImageMenuItem)popmenuSimAdd).Children[0])).Text = translator.readTranslatedString(32) ;
			((AccelLabel)(((ImageMenuItem)popmenuSimDel).Children[0])).Text = translator.readTranslatedString(33);
			((AccelLabel)(((ImageMenuItem)popmenuSimVFile).Children[0])).Text = translator.readTranslatedString(35);
			
			Console.WriteLine("debug translator..." + translator.readTranslatedString(32));
			
			popmenuSimAdd.ButtonReleaseEvent += On_Add_button_release_event;
			popmenuSimDel.ButtonReleaseEvent += On_Del_button_release_event;
			popmenuSimVFile.ButtonReleaseEvent += On_Move_button_release_event;
			
			popmenuSim.Insert(new Gtk.SeparatorMenuItem(), 2);
			
			
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
		public bool IsWindows()
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
			clsNewItemDlg nI = new clsNewItemDlg(topWindow, DialogFlags.DestroyWithParent,ref strNI, ref translator);
			
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
		
		
		private string ChangePin(bool newstatus)
		{
			int RetPin = 0;
			string RetValue = "";
			clsInputDialog PINinput = new clsInputDialog(topWindow,
		                                             DialogFlags.DestroyWithParent,
		                                             ref RetPin,
		                                             "PIN1",
		                                             translator.readTranslatedString(132),
		                                             ref RetValue);
			if (RetPin != 1)
				return "cancel";
		
			// Console.WriteLine("PIN1 = " + RetValue);
			
			this.statusbar1.Push(1, translator.readTranslatedString(133));
			
			string RetSet = PIN1_Set(newstatus, RetValue);
			
			if (RetSet != "")
				this.statusbar1.Push(1, "ERROR: " + RetSet);
			else
				this.statusbar1.Push(1, translator.readTranslatedString(134));
			
			return RetSet;
		}
	
	
		private string PIN1_Status(out bool P1)
		{
			P1 = false;
			this.topWindow.Sensitive=false;
		
			// Update GUI...
			while (Gtk.Application.EventsPending ())
            	Gtk.Application.RunIteration ();
		
			this.statusbar1.Push(1, translator.readTranslatedString(129));
	
		
			// Manage Creation of context
			pcscResult = this.myCard.CreateContext();
			if (pcscResult != "")
			{
				endReading("ERROR: " + pcscResult);				
				return pcscResult;
			}
			
			// Manage SmartCard Connection
			pcscResult = this.myCard.ConnectSmartCard(readerToUse, ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR: " + retErrorString);
				return retErrorString;
			}
			
			
			// Read PIN Status
			pcscResult = this.parseCommand("A0A40000023F00", "9F??", ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR: " + retErrorString);
				return retErrorString;
			}
			
			pcscResult = this.parseCommand("A0C00000" + pcscResult.Substring(2,2) , "", ref retErrorString);
			if (retErrorString != "")
			{
				endReading("ERROR: " + retErrorString);
				return retErrorString;
			}
			
			// Check PIN
			Console.WriteLine("3F00 Response = " + pcscResult);
			if ( Convert.ToInt32(pcscResult.Substring(26, 1), 16) < 8)
				P1 = true;
			else
				P1 = false;
			
			return "";
		}
	
	
		private string PIN1_Set(bool Pin1_status, string Pin1Val)
		{
			cEncoding MyEnc = new cEncoding();
		
			string PIN1HEX = MyEnc.getHexFromAscii(Pin1Val);
			PIN1HEX = PIN1HEX.PadRight(16, Convert.ToChar("F"));
			
		
			// Manage Creation of context
			pcscResult = this.myCard.CreateContext();
			if (pcscResult != "")
				return pcscResult;
			
			// Manage SmartCard Connection
			pcscResult = this.myCard.ConnectSmartCard(readerToUse, ref retErrorString);
			if (retErrorString != "")
				return retErrorString;
			
			if (Pin1_status == true)
			{
				pcscResult = this.myCard.SendCommand("A028000108" + PIN1HEX, ref retErrorString);
				if (retErrorString != "")
					return retErrorString;

				if (pcscResult != "9000")
					return pcscResult;			
			}			
			else
			{
				pcscResult = this.myCard.SendCommand("A026000108" + PIN1HEX, ref retErrorString);
				if (retErrorString != "")
					return retErrorString;			

				if (pcscResult != "9000")
					return pcscResult;
			}
			
			return "";
		
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
			this.statusbar1.Push(1, translator.readTranslatedString(109));
			
			// Update GUI...
			while (Gtk.Application.EventsPending ())
              Gtk.Application.RunIteration ();
			
			bool PinStatus = false;
			pcscResult = PIN1_Status(out PinStatus);
			if (pcscResult != "")
			{
				endReading("ERROR: " + pcscResult);
				return;
			}				
			
			if (PinStatus == true)
			{
				int RetWin = 0;
				clsMsgDialog PinMessage = new clsMsgDialog(topWindow, 
				                                           DialogFlags.DestroyWithParent,
				                                           ref RetWin,
				                                           "PIN1", 
				                                           translator.readTranslatedString(130));
				if (RetWin != 1)
					return;		
			
				pcscResult = ChangePin(false);

				if (pcscResult != "")
				{
					endReading("ERROR: " + pcscResult);
					return;
				}
			
			}
		
			
			

		
		
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
				endReading("ERROR: " + translator.readTranslatedString(110));
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
				
				// Console.WriteLine(pcscResult);
        		
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
        		this.statusbar1.Push(1, translator.readTranslatedString(111) + " " + k.ToString("D3"));
        		
        		// Update GUI...
				while (Gtk.Application.EventsPending ())
              		Gtk.Application.RunIteration ();
				
        	}
        	
        	this.statusbar1.Push(1, translator.readTranslatedString(102) + " " + filledRecords.ToString());
        	
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
			                                         ref readerToUse, 
			                                         ref translator);
		
			newDlg = null;
			if (readerToUse.CompareTo("0") == 0 )
			{
				endReading("...");
				return;
			}
			
			this.topWindow.Sensitive = false;
			this.pBar.Fraction = 0;
			
			this.pBar.Visible=true;
			this.statusbar1.Push(1, translator.readTranslatedString(112));
			
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
				endReading("ERROR: " + translator.readTranslatedString(110));
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
			this.statusbar1.Push(1, translator.readTranslatedString(113));
			
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
        		this.statusbar1.Push(1, translator.readTranslatedString(113) + " " + k.ToString("D3"));
        		
        		// Update GUI...
				while (Gtk.Application.EventsPending ())
              		Gtk.Application.RunIteration ();
        	}
        	
        	if(EmptyPosition < sender.IterNChildren())
			{
				pcscResult = this.myCard.DeleteContext();
        		endReading("ERROR: " + translator.readTranslatedString(114));
				return;
			}
        	
        	this.pBar.Adjustment.Lower=0;
        	this.statusbar1.Push(1, translator.readTranslatedString(112));
        	
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
	        			// Console.WriteLine("APDU " + posToWrite.ToString("D2") + " = " + tmpCommand);
	        			pcscResult = this.parseCommand(tmpCommand, "9000", ref retErrorString);
	        			posToWrite ++;
	        			
	        			if (retErrorString != "")
        		        {
        		            endReading("ERROR: " + retErrorString);
        		            return;
        		        }
	        			
	        			
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
        		this.statusbar1.Push(1, translator.readTranslatedString(115) + " " + k.ToString("D3"));
        		
        		// Update GUI...
				while (Gtk.Application.EventsPending ())
              		Gtk.Application.RunIteration ();
        	}
        	
        	
        	this.statusbar1.Push(1, translator.readTranslatedString(116));
        	
			// Manage Delete of context
			pcscResult = this.myCard.DeleteContext();
			if (pcscResult != "")
			{
				endReading("ERROR: " + pcscResult);
				return;
			}
			
			lsListSim.Clear();
			checkListStatus();
		
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
				errorTmp = translator.readTranslatedString(117) + ": " + pcscResult;
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
							Console.WriteLine("Char n. " + n.ToString() + " pcscResult=" + pcscResult + " theResponse=" + theResponse);
							errorTmp = translator.readTranslatedString(118) + ": " + pcscResult;
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
				this.item_DeleteSim.Sensitive=false;
				this.tbSaveFile.Sensitive = false;
				this.tbSaveFileSim.Sensitive = false;
			}else{
				this.item_SaveFile.Sensitive = true;
				this.item_SaveFileSim.Sensitive = true;
				this.item_DeleteSim.Sensitive=true;
				this.tbSaveFile.Sensitive = true;
				this.tbSaveFileSim.Sensitive = true;
			}

			if (this.lsListSim.IterNChildren() == 0)
			{
				this.item_SaveSim.Sensitive=false;
				this.item_SaveSimFile.Sensitive=false;
				this.item_DeleteSim.Sensitive=false;
				this.tbSaveSim.Sensitive = false;
				this.tbSaveSimFile.Sensitive = false;				
			}else{
				this.item_SaveSim.Sensitive=true;
				this.item_SaveSimFile.Sensitive=true;
				this.item_DeleteSim.Sensitive=true;
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
			AlphaNew = AlphaNew.PadRight(maxAlfaChar * 2, Convert.ToChar("F"));
			
			TelNew = phonenumber.Replace("*", "A");
			TelNew = TelNew.Replace("#", "B");
        	
			theL = TelNew.Length;
			
			if ( IsOdd(theL) == true )
				theL = ( ( (theL + (int)1) / 2 ) + 1 );			    
			else
				theL = ( (theL / 2) + (int)1);
			
			phoneLen = theL.ToString("X2");
			TelNew = TelNew.PadRight(20, Convert.ToChar("F"));
			
			for (int n=0; n<TelNew.Length; n +=2)
				TelNewOut = TelNewOut + TelNew.Substring(n+1,1) + TelNew.Substring(n,1);
			
			outStr = AlphaNew + phoneLen + "00" + TelNewOut + "FFFF";
				
			return outStr;		
		}
		
		
		private void changeLanguage(string languageName)
		{
			translator.readLanguageFile(languageName);
		
			// Debug Language Items
		
			// Update GUI...
				while (Gtk.Application.EventsPending ())
	              Gtk.Application.RunIteration ();
		
			// MenuBar Items...
			((AccelLabel)(menu_File.Children[0])).Text = translator.readTranslatedString(1);
			((AccelLabel)(item_NewFile.Children[0])).Text = translator.readTranslatedString(2);
			((AccelLabel)(item_OpenFile.Children[0])).Text = translator.readTranslatedString(3);
			((AccelLabel)(item_SaveFile.Children[0])).Text = translator.readTranslatedString(4);
			((AccelLabel)(item_SaveFileSim.Children[0])).Text = translator.readTranslatedString(5);
			((AccelLabel)(item_Exit.Children[0])).Text = translator.readTranslatedString(6);
			
			((AccelLabel)(menu_Sim.Children[0])).Text = translator.readTranslatedString(7);
			((AccelLabel)(item_ConnectSim.Children[0])).Text = translator.readTranslatedString(8);
			((AccelLabel)(item_SaveSim.Children[0])).Text = translator.readTranslatedString(9);
			((AccelLabel)(item_SaveSimFile.Children[0])).Text = translator.readTranslatedString(10);
			((AccelLabel)(item_DeleteSim.Children[0])).Text = translator.readTranslatedString(71);
			
			((AccelLabel)(menu_Languages.Children[0])).Text = translator.readTranslatedString(11);
			
			((AccelLabel)(menu_Help.Children[0])).Text = translator.readTranslatedString(12);
			((AccelLabel)(item_InfoHelp.Children[0])).Text = translator.readTranslatedString(13);
			
			
			// ToolBar Items ToolTipText...
			tips1 = new Gtk.Tooltips();
					
			tbOpenFile.SetTooltip(tips1, translator.readTranslatedString(14), null);
			tbSaveFile.SetTooltip(tips1, translator.readTranslatedString(15), null);
			tbSaveFileSim.SetTooltip(tips1, translator.readTranslatedString(16), null);
			
			tbConnectSim.SetTooltip(tips1, translator.readTranslatedString(17), null);
			tbSaveSimFile.SetTooltip(tips1, translator.readTranslatedString(18), null);
			tbSaveSim.SetTooltip(tips1, translator.readTranslatedString(19), null);
			
			tbExit.SetTooltip(tips1, translator.readTranslatedString(20), null);
			
			// ToolBar Items Label...
			tbOpenFile.Label = translator.readTranslatedString(25);
			tbSaveFile.Label = translator.readTranslatedString(26);
			tbSaveFileSim.Label = translator.readTranslatedString(27);
			
			tbConnectSim.Label = translator.readTranslatedString(28);
			tbSaveSimFile.Label = translator.readTranslatedString(29);
			tbSaveSim.Label = translator.readTranslatedString(30);
			
			tbExit.Label = translator.readTranslatedString(31);
			
			// Frames and Lists
			lblFile.Markup = "<b>" + translator.readTranslatedString(21) + "</b>";
			lblSim.Markup = "<b>" + translator.readTranslatedString(22) + "</b>";
			
			listFile.Columns[0].Title = translator.readTranslatedString(23);
			listFile.Columns[1].Title = translator.readTranslatedString(24);
			listSim.Columns[0].Title = translator.readTranslatedString(23);
			listSim.Columns[1].Title = translator.readTranslatedString(24);
			
			clsSettings.defaultLanguage = languageName;
			clsSerializationHelper.save(".monosim");
			
			// Update GUI...
				while (Gtk.Application.EventsPending ())
	              Gtk.Application.RunIteration ();
			
			return;
		}
		
		
	private void On_Change_Language(object sender, ButtonReleaseEventArgs args)
	{
		string choseLanguage = ((AccelLabel)(((RadioMenuItem)(sender)).Children[0])).Text;
		changeLanguage(choseLanguage);
	}
	
	
	
	private void On_DeleteSim_Press(object sender, ButtonReleaseEventArgs args)
	{
		
		// Write on sim from TreeView List
		string RLen = "";
		string FLen = "";
		string tmpCommand = "";
		string tmpResp = "";
		string tmpFill = "";
		string theName = "";
		string thePhone = "";
		string emptyStr = "";
		TreeIter theRow;			
		int EmptyPosition = 0;
		int posToWrite = 0;
		
		MessageDialog delDlg = new MessageDialog(topWindow,
	    	                                     DialogFlags.Modal,
	    	                                     MessageType.Warning,
	    	                  				     ButtonsType.YesNo,
											     translator.readTranslatedString(125));
		
		delDlg.Title = translator.readTranslatedString(126);
	
		delDlg.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");			
		ResponseType retDel = (ResponseType)(delDlg.Run());
		delDlg.Destroy();
		Console.WriteLine("retDel = " + retDel.ToString());
		
		if (retDel != ResponseType.Yes)
		    return;

		clsReadersDlg newDlg = new clsReadersDlg(topWindow, 
		                                         DialogFlags.DestroyWithParent,
		                                         ref listReaders,
		                                         ref readerToUse, 
		                                         ref translator);
	
		newDlg = null;
		if (readerToUse.CompareTo("0") == 0 )
		{
			endReading("...");
			return;
		}
		
		this.topWindow.Sensitive = false;
		this.pBar.Fraction = 0;
		
		this.pBar.Visible=true;
		this.statusbar1.Push(1, translator.readTranslatedString(112));
		
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
			endReading("ERROR: " + translator.readTranslatedString(110));
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
		
   	    // Update GUI...
		while (Gtk.Application.EventsPending ())
         Gtk.Application.RunIteration ();
		
   	    tmpFill = new string( Convert.ToChar("F") ,  (int)(Convert.ToInt32(RLen,16) * 2));
   	    tmpResp = "9000";
   	    
   	    for (int k=1; k<=totRecords; k++)
   	    {
       		tmpCommand = "A0DC" + k.ToString("X2") + "04" + RLen + tmpFill;
       		
       		pcscResult = this.parseCommand(tmpCommand, tmpResp, ref retErrorString);
       		if (retErrorString != "")
    		{
    			endReading("ERROR [P6]: " + retErrorString);
    			return;
    		}			
       		
       		this.pBar.Adjustment.Value = (double)k;
       		this.statusbar1.Push(1, translator.readTranslatedString(127) + " " + k.ToString("D3"));
       		
       		// Update GUI...
    			while (Gtk.Application.EventsPending ())
             		Gtk.Application.RunIteration ();
   	    }
   	
   	    this.statusbar1.Push(1, translator.readTranslatedString(128));
   	
    	// Manage Delete of context
    	pcscResult = this.myCard.DeleteContext();
    	if (pcscResult != "")
    	{
    		endReading("ERROR: " + pcscResult);
    		return;
    	}
    	
    	lsListSim.Clear();
		checkListStatus();
		

    	// Update GUI...
    	while (Gtk.Application.EventsPending ())
        Gtk.Application.RunIteration ();
    		
    	// Reactive the Main Window			
    	this.pBar.Visible=false;
    	this.topWindow.Sensitive = true;
	
	
	    return;
		
		
	}
	
	
	
	
	
}

