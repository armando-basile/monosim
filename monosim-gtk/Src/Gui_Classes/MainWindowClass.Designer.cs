using System;
using System.Collections.Generic;
using System.IO;
using Glade;
using Gtk;
using Gdk;
using Pango;

using log4net;

using comexbase;
using monosimbase;

namespace monosimgtk
{

	public partial class MainWindowClass
	{
		
		#region Gtk Widgets
		
		
		[Glade.Widget]  Gtk.Window             MainWindow = null;
		
		
		[Glade.Widget]  Gtk.ToolButton         TbOpen = null;
		[Glade.Widget]  Gtk.ToolButton         TbSaveFile = null;
		[Glade.Widget]  Gtk.ToolButton         TbSaveSim = null;
		[Glade.Widget]  Gtk.ToolButton         TbSettings = null;
		[Glade.Widget]  Gtk.ToolButton         TbOpenSim = null;
		[Glade.Widget]  Gtk.ToolButton         TbSaveSimFile = null;
		[Glade.Widget]  Gtk.ToolButton         TbSaveSimSim = null;
		[Glade.Widget]  Gtk.ToolButton         TbAbout = null;
		[Glade.Widget]  Gtk.ToolButton         TbExit = null;
		
		[Glade.Widget]  Gtk.Statusbar          StatusBar = null;
		[Glade.Widget]  Gtk.ProgressBar        PBar = null;
		
		[Glade.Widget]  Gtk.MenuItem       	   MenuFileItem = null;		
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuFileNew = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuFileOpen = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuFileSaveFile = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuFileSaveSim = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuFileClose = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuFileSettings = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuFileExit = null;		
		
		[Glade.Widget]  Gtk.MenuItem       	   MenuReaderItem = null;
		[Glade.Widget]  Gtk.Menu       	       MenuReader = null;
		
		[Glade.Widget]  Gtk.MenuItem       	   MenuSimItem = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuSimConnect = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuSimPin = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuSimSaveFile = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuSimSaveSim = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuSimDeleteAll = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuSimDisconnect = null;
		
		[Glade.Widget]  Gtk.MenuItem       	   MenuAboutItem = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   MenuAboutInfo = null;
		
		[Glade.Widget]  Gtk.Label              LblSim = null;
		[Glade.Widget]  Gtk.Label              LblFile = null;
		[Glade.Widget]  Gtk.TreeView           LstFileContacts = null;
		[Glade.Widget]  Gtk.TreeView           LstSimContacts = null;
		
		[Glade.Widget]  Gtk.Menu	       	   PopMenuFile = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   PopMenuFileAdd = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   PopMenuFileDel = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   PopMenuFileVSim = null;
		
		[Glade.Widget]  Gtk.Menu         	   PopMenuSim = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   PopMenuSimAdd = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   PopMenuSimDel = null;
		[Glade.Widget]  Gtk.ImageMenuItem  	   PopMenuSimVFile = null;
		
		
		#endregion Gtk Widgets
		
		
		
		
		// Log4Net object
        private static readonly ILog log = LogManager.GetLogger(typeof(MainWindowClass));
		
		

		// Attributes
		private ListStore lstFileContacts;
		private ListStore lstSimContacts;
		
		
		
		
		
		#region Public Methods
		
		
		
		
		/// <summary>
        /// Constructor
        /// </summary>
        public MainWindowClass()
        {                
            // Instance glade xml object using glade file
            Glade.XML gxml =  new Glade.XML("MainWindow.glade", "MainWindow");
            
            // Aonnect glade xml object to this Gtk.Windows
            gxml.Autoconnect(this);
            
			// Use PopMenuFile top object from glade file
			gxml =  new Glade.XML("MainWindow.glade", "PopMenuFile");
			gxml.Autoconnect(this);
			
			// Use PopMenuSim top object from glade file
			gxml =  new Glade.XML("MainWindow.glade", "PopMenuSim");
			gxml.Autoconnect(this);
			
            // Update Gtk graphic objects
            UpdateGraphicObjects();
            
            // Update Event Handlers
            UpdateReactors();                       
                       
        }
		
		
		
		
		
		/// <summary>
		/// Show MainWindow
		/// </summary>
		public void Show()
		{
			MainWindow.Show();
			MainWindow.Title = MainClass.AppNameVer + " [" + GlobalObj.AppNameVer + "]";
		}
		
		
		
		
		
		
		
		
		
		#endregion Public Methods
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		#region Private Methods
		
		
		
		
		

		/// <summary>
		/// Update Gtk objects properties
		/// </summary>
		private void UpdateGraphicObjects()
		{
			// Set dialog icon
            MainWindow.Icon = Gdk.Pixbuf.LoadFromResource("monosim.png");
			
			// Set tool tip text for toolbutton
			TbOpen.TooltipText = GlobalObjUI.LMan.GetString("openfileact");
			TbSaveFile.TooltipText = GlobalObjUI.LMan.GetString("savefileact");
			
			
			TbSaveSim.TooltipText = GlobalObjUI.LMan.GetString("savefilesimact");
			TbSaveSim.IconWidget = new Gtk.Image(Pixbuf.LoadFromResource("chip_24.png"));
			TbSaveSim.ShowAll();
			
			TbSettings.TooltipText = GlobalObjUI.LMan.GetString("settingsact");
			TbOpenSim.TooltipText = GlobalObjUI.LMan.GetString("opensimact");
			TbSaveSimFile.TooltipText = GlobalObjUI.LMan.GetString("savesimfileact");
			
			TbSaveSimSim.TooltipText = GlobalObjUI.LMan.GetString("savesimact");
			TbSaveSimSim.IconWidget = new Gtk.Image(Pixbuf.LoadFromResource("chip_24.png"));
			TbSaveSimSim.ShowAll();
			
			TbAbout.TooltipText = GlobalObjUI.LMan.GetString("infoact");
			TbExit.TooltipText = GlobalObjUI.LMan.GetString("exitact");
			
			// change icons for chip
			MenuFileSaveSim.Image = new Gtk.Image(Pixbuf.LoadFromResource("chip_16.png"));
			MenuSimSaveSim.Image = new Gtk.Image(Pixbuf.LoadFromResource("chip_16.png"));
				
			// Set labels
			((Label)MenuFileItem.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("filemenulbl");
			((Label)MenuFileNew.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("newfileact");
			((Label)MenuFileOpen.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("openfileact");
			((Label)MenuFileSaveFile.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("savefileact");
			((Label)MenuFileSaveSim.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("savefilesimact");
			((Label)MenuFileClose.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("closefileact");
			((Label)MenuFileSettings.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("settingsact");
			((Label)MenuFileExit.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("exitact");
			
			((Label)MenuReaderItem.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("readermenulbl");
			
			((Label)MenuSimItem.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("simmenulbl");
			((Label)MenuSimConnect.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("opensimact");
			((Label)MenuSimPin.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("pinsimact");
			((Label)MenuSimSaveFile.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("savesimfileact");
			((Label)MenuSimSaveSim.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("savesimact");
			((Label)MenuSimDeleteAll.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("deletesimact");
			((Label)MenuSimDisconnect.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("closesimact");
			
			((Label)MenuAboutItem.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("helpmenulbl");
			((Label)MenuAboutInfo.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("infoact");
			
			LblFile.Markup = "<b>" + GlobalObjUI.LMan.GetString("framefile") + "</b>"; 
			LblSim.Markup =  "<b>" + GlobalObjUI.LMan.GetString("framesim") + "</b>";
			
			
			// Set Popup menu
			((Label)PopMenuFileAdd.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("addcontacts");
			((Label)PopMenuFileDel.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("delcontacts");
			((Label)PopMenuFileVSim.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("copycontactstosim");

			((Label)PopMenuSimAdd.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("addcontacts");
			((Label)PopMenuSimDel.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("delcontacts");
			((Label)PopMenuSimVFile.Child).TextWithMnemonic = GlobalObjUI.LMan.GetString("copycontactstofile");
			
			
			// Set ListView
			string[] columnHeaders = new string[]{ GlobalObjUI.LMan.GetString("descnumber"),
				                                   GlobalObjUI.LMan.GetString("phonenumber") };
			
			InitListView(ref lstFileContacts, ref LstFileContacts, columnHeaders);
			InitListView(ref lstSimContacts, ref LstSimContacts, columnHeaders);
			
			
			// update gui menu
			Gtk.RadioMenuItem rmi;
			
			// loop for all readers
			List<string> allReaders = new List<string>();
			
			// loop for each managed readers type
			foreach(IReader rdr in GlobalObj.ReaderManager.Values)
			{
				allReaders.AddRange(rdr.Readers);
			}
			
			
			for (int n=0; n<allReaders.Count; n++)
			{	
				// set first as selected
				if (n==0)
				{
					rmi = new Gtk.RadioMenuItem(allReaders[n]);
					GlobalObj.SelectedReader = allReaders[n];
					StatusBar.Push(1, GlobalObj.LMan.GetString("selreader") + ": " + allReaders[n]);
				}
				else
				{
					// added others
					rmi = new Gtk.RadioMenuItem((RadioMenuItem)MenuReader.Children[0], allReaders[n]);
				}
				
				rmi.ButtonReleaseEvent += ActionChangeReader;
				MenuReader.Add(rmi);
			}
			
			MenuReader.ShowAll();
			
			
		}
		
		
		
		
		
		
		/// <summary>
		/// Update Gtk objects reactors
		/// </summary>
		private void UpdateReactors()
		{
			MainWindow.DeleteEvent += ActionCancel;
			MenuFileNew.Activated += ActionFileNew;
			MenuFileOpen.Activated += ActionFileOpen;
			MenuFileSaveFile.Activated += ActionFileSave;
			MenuFileSaveSim.Activated += ActionFileSaveSim;
			MenuFileClose.Activated += ActionFileClose;
			MenuFileSettings.Activated += ActionSettings;
			MenuFileExit.Activated += ActionCancel;
			
			MenuSimConnect.Activated += ActionSimConnect;
			MenuSimPin.Activated += ActionSimChangePin;
			MenuSimSaveFile.Activated += ActionSimSaveFile;
			MenuSimSaveSim.Activated += ActionSimSave;
			MenuSimDeleteAll.Activated += ActionSimDeleteAll;
			MenuSimDisconnect.Activated += ActionSimDisconnect;
			
			MenuAboutInfo.Activated += ActionAbout;
			
			TbOpen.Clicked += ActionFileOpen;
			TbSaveFile.Clicked += ActionFileSave;
			TbSaveSim.Clicked += ActionFileSaveSim;
			TbSettings.Clicked += ActionSettings;
			TbOpenSim.Clicked += ActionSimConnect;
			TbSaveSimFile.Clicked += ActionSimSaveFile;
			TbSaveSimSim.Clicked += ActionSimSave;
			TbAbout.Clicked += ActionAbout;
			TbExit.Clicked += ActionCancel;
			
			PopMenuFileAdd.Activated += ActionFileAdd;
			PopMenuFileDel.Activated += ActionFileDel;
			PopMenuFileVSim.Activated += ActionFileMoveToSim;
			PopMenuSimAdd.Activated += ActionSimAdd;
			PopMenuSimDel.Activated += ActionSimDel;
			PopMenuSimVFile.Activated += ActionSimMoveToFile;
			
			LstFileContacts.ButtonPressEvent += ActionListFileContactsButtonPress;
			LstSimContacts.ButtonPressEvent += ActionListSimContactsButtonPress;
		}
		
		
		
		
		
		
		/// <summary>
		/// Init ListView widget.
		/// </summary>
		private void InitListView(ref ListStore lStore, 
			                      ref TreeView tvObject, 
			                          string[] tvColumns)
		{
			TreeViewColumn tvColumn = null;
			CellRendererText tvCell = null;
			
			tvObject.Selection.Mode = SelectionMode.Multiple;
			System.Type[] lsParam = new System.Type[tvColumns.Length];
			
			// Add Columns to TreeView
			for (int j=0; j<tvColumns.Length; j++)
			{
				tvColumn = new Gtk.TreeViewColumn();		
				tvColumn.MinWidth = 150 ;
				tvColumn.Title = tvColumns[j];
				tvColumn.Resizable = true;
				
				tvCell = new Gtk.CellRendererText();
				tvColumn.PackStart(tvCell, true);
				tvColumn.AddAttribute(tvCell, "text", j);
				
				tvObject.RulesHint = true;
				tvObject.AppendColumn(tvColumn);				
				lsParam[j] = typeof(string);
			}
		
		
			lStore = new Gtk.ListStore(lsParam);
			tvObject.Model = lStore;
			tvObject.ShowAll();
		}
		
		
		
		
		
		
		
		
		#endregion Private Methods
		
		
		
		
		
		
		
		
		
	}
	
}
