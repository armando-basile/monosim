
using System;
using Qyoto;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using comexbase;
using monosimbase;

using log4net;

namespace monosimqt
{
	
	
	public partial class MainWindowClass: QMainWindow
	{
		
		// Attributes		
		private Ui.MainWindow mainwindow_Ui;
		private QProgressBar PBar = null;		
		
		private QMenu menuFileItem = null;
		private QMenu menuSimItem = null;
		private List<QAction> menuFileActions = null;
		private List<QAction> menuSimActions = null;
		
		// Log4Net object
        private static readonly ILog log = LogManager.GetLogger(typeof(monosimqt.MainWindowClass));

		
		
		/// <summary>
		/// Constructor
		/// </summary>
		public MainWindowClass()
		{
			// Create new mainwindow_Ui object
			mainwindow_Ui = new Ui.MainWindow();
			
			// Configure layout of this new QMainWindow with 
			// mainwindow_Ui objects and data
			mainwindow_Ui.SetupUi(this);
			
			// Update Graphic Objects
			UpdateGraphicObjects();
			
			// Add eventhandlers
			UpdateReactors();
			
		}
		
		
		
		
		
		
		
		
	
		
		
		
		
		
		
	

		#region Private Methods
		
		
		
		/// <summary>
		/// Update graphic objects with language file values
		/// </summary>
		private void UpdateGraphicObjects()
		{
			// Main Window Title
			this.WindowTitle = MainClass.AppNameVer + " [" + GlobalObj.AppNameVer + "]";
			
			// Set tool tip text for toolbutton
			mainwindow_Ui.MenuFileItem.Title = GlobalObjUI.LMan.GetString("filemenulbl").Replace("_", "&");
			mainwindow_Ui.MenuFileNew.ToolTip = GlobalObjUI.LMan.GetString("newfileact").Replace("_", "");
			mainwindow_Ui.MenuFileNew.Text = GlobalObjUI.LMan.GetString("newfileact").Replace("_", "&");
			mainwindow_Ui.MenuFileOpen.ToolTip = GlobalObjUI.LMan.GetString("openfileact").Replace("_", "");
			mainwindow_Ui.MenuFileOpen.Text = GlobalObjUI.LMan.GetString("openfileact").Replace("_", "&");
			mainwindow_Ui.MenuFileSaveFile.ToolTip = GlobalObjUI.LMan.GetString("savefileact").Replace("_", "");
			mainwindow_Ui.MenuFileSaveFile.Text = GlobalObjUI.LMan.GetString("savefileact").Replace("_", "&");
			mainwindow_Ui.MenuFileSaveSim.ToolTip = GlobalObjUI.LMan.GetString("savefilesimact").Replace("_", "");
			mainwindow_Ui.MenuFileSaveSim.Text = GlobalObjUI.LMan.GetString("savefilesimact").Replace("_", "&");
			mainwindow_Ui.MenuFileClose.ToolTip = GlobalObjUI.LMan.GetString("closefileact").Replace("_", "");
			mainwindow_Ui.MenuFileClose.Text = GlobalObjUI.LMan.GetString("closefileact").Replace("_", "&");
			mainwindow_Ui.MenuFileSettings.ToolTip = GlobalObjUI.LMan.GetString("settingsact").Replace("_", "");
			mainwindow_Ui.MenuFileSettings.Text = GlobalObjUI.LMan.GetString("settingsact").Replace("_", "&");
			mainwindow_Ui.MenuFileExit.ToolTip = GlobalObjUI.LMan.GetString("exitact").Replace("_", "");
			mainwindow_Ui.MenuFileExit.Text = GlobalObjUI.LMan.GetString("exitact").Replace("_", "&");
			
			mainwindow_Ui.MenuSimItem.Title = GlobalObjUI.LMan.GetString("simmenulbl").Replace("_", "&");
			mainwindow_Ui.MenuSimConnect.ToolTip = GlobalObjUI.LMan.GetString("opensimact").Replace("_", "");
			mainwindow_Ui.MenuSimConnect.Text = GlobalObjUI.LMan.GetString("opensimact").Replace("_", "&");
			mainwindow_Ui.MenuSimPin.ToolTip = GlobalObjUI.LMan.GetString("pinsimact").Replace("_", "");
			mainwindow_Ui.MenuSimPin.Text = GlobalObjUI.LMan.GetString("pinsimact").Replace("_", "&");
			mainwindow_Ui.MenuSimSaveFile.ToolTip = GlobalObjUI.LMan.GetString("savesimfileact").Replace("_", "");
			mainwindow_Ui.MenuSimSaveFile.Text = GlobalObjUI.LMan.GetString("savesimfileact").Replace("_", "&");
			mainwindow_Ui.MenuSimSaveSim.ToolTip = GlobalObjUI.LMan.GetString("savesimact").Replace("_", "");
			mainwindow_Ui.MenuSimSaveSim.Text = GlobalObjUI.LMan.GetString("savesimact").Replace("_", "&");
			mainwindow_Ui.MenuSimDeleteAll.Text = GlobalObjUI.LMan.GetString("deletesimact").Replace("_", "&");
			mainwindow_Ui.MenuSimDisconnect.ToolTip = GlobalObjUI.LMan.GetString("closesimact").Replace("_", "");
			mainwindow_Ui.MenuSimDisconnect.Text = GlobalObjUI.LMan.GetString("closesimact").Replace("_", "&");
			
			mainwindow_Ui.MenuReaderItem.Title = GlobalObjUI.LMan.GetString("readermenulbl").Replace("_", "&");
			
			mainwindow_Ui.MenuAboutItem.Title = GlobalObjUI.LMan.GetString("helpmenulbl").Replace("_", "&");
			mainwindow_Ui.MenuAboutInfo.ToolTip = GlobalObjUI.LMan.GetString("infoact").Replace("_", "");
			mainwindow_Ui.MenuAboutInfo.Text = GlobalObjUI.LMan.GetString("infoact").Replace("_", "&");
			
			// Frame labels
			mainwindow_Ui.FrameFile.Title = GlobalObjUI.LMan.GetString("framefile"); 
			mainwindow_Ui.FrameSim.Title =  GlobalObjUI.LMan.GetString("framesim");
			
			
			// Setup column headers
			List<string> columnHeaders = new List<string>();
			columnHeaders.Add(" ");
			columnHeaders.Add(GlobalObjUI.LMan.GetString("descnumber"));
			columnHeaders.Add(GlobalObjUI.LMan.GetString("phonenumber"));
			
			mainwindow_Ui.LstFileContacts.SetHeaderLabels(columnHeaders);
			mainwindow_Ui.LstFileContacts.SetColumnWidth(1, 150);
			mainwindow_Ui.LstFileContacts.SetColumnWidth(2, 150);
			
			mainwindow_Ui.LstSimContacts.SetHeaderLabels(columnHeaders);
			mainwindow_Ui.LstSimContacts.SetColumnWidth(1, 150);
			mainwindow_Ui.LstSimContacts.SetColumnWidth(2, 150);
			
			mainwindow_Ui.LstFileContacts.HideColumn(0);
			mainwindow_Ui.LstSimContacts.HideColumn(0);
			
			
			// loop for each managed readers type
			foreach(IReader rdr in GlobalObj.ReaderManager.Values)
			{
				allReaders.AddRange(rdr.Readers);
			}
			
			// Update readers list on gui			
			QAction action_Reader;
			QActionGroup readersGrp = new QActionGroup(this);
			
			for (int r=0; r<allReaders.Count; r++)
			{
				action_Reader = new QAction(allReaders[r], mainwindow_Ui.MenuReaderItem);
				action_Reader.ObjectName = "action_Reader_" + r.ToString();
				action_Reader.SetVisible(true);
				action_Reader.IconVisibleInMenu = false;
				action_Reader.Checkable = true;
				action_Reader.SetActionGroup(readersGrp);
				
				if (r==0)
				{
					action_Reader.SetChecked(true);
				}
				else
				{
					action_Reader.SetChecked(false);
				}
				mainwindow_Ui.MenuReaderItem.AddAction(action_Reader);
				
				Connect( action_Reader, SIGNAL("activated()"), this, SLOT("ActionChangeReader()"));
				
			}
			
			// check for available readers
			if (allReaders.Count > 0)
			{
				// select first reader
				GlobalObj.SelectedReader = allReaders[0];
			}
			
			// Add progressbar to statusbar
			PBar = new QProgressBar(mainwindow_Ui.StatusBar);
			PBar.SetFixedWidth(180);
			PBar.SetFixedHeight(20);
			PBar.SetVisible(false);
			mainwindow_Ui.StatusBar.AddPermanentWidget(PBar);

			CreateFileMenu();
			CreateSimMenu();
		}




        /// <summary>
        /// Sim event arrived
        /// </summary>
        private void SimEvent(object sender, EventArgs eArgs)
        {
                // recall qt update method
                notify.WakeupMain();
        }

		
		
		
		
		
		
		private void CreateFileMenu()
		{
			menuFileItem = new QMenu(mainwindow_Ui.LstFileContacts);
			
			menuFileActions = new List<QAction>();
			menuFileActions.Add(new QAction(new QIcon(":/toolbar/resources/qt/list-add.png"),
				GlobalObjUI.LMan.GetString("addcontacts"),
				menuFileItem));
			
			menuFileActions.Add(new QAction(new QIcon(":/toolbar/resources/qt/edit-delete.png"),
				GlobalObjUI.LMan.GetString("delcontacts"),
				menuFileItem));
			
			menuFileActions.Add(new QAction(new QIcon(":/toolbar/resources/qt/go-down.png"),
				GlobalObjUI.LMan.GetString("copycontactstosim"),
				menuFileItem));
			
			menuFileItem.AddActions(menuFileActions);
			
			menuFileActions[0].ObjectName = "fileadd";
			menuFileActions[1].ObjectName = "filedel";
			menuFileActions[2].ObjectName = "filecopy";
			
			foreach(QAction qa in menuFileActions)
			{
				qa.SetVisible(true);
				qa.IconVisibleInMenu=true;
			}

		}
		
		
		
		
		
		
		private void CreateSimMenu()
		{
			menuSimItem = new QMenu(mainwindow_Ui.LstSimContacts);
			
			menuSimActions = new List<QAction>();
			menuSimActions.Add(new QAction(new QIcon(":/toolbar/resources/qt/list-add.png"),
				GlobalObjUI.LMan.GetString("addcontacts"),
				menuSimItem));
			
			menuSimActions.Add(new QAction(new QIcon(":/toolbar/resources/qt/edit-delete.png"),
				GlobalObjUI.LMan.GetString("delcontacts"),
				menuSimItem));
			
			menuSimActions.Add(new QAction(new QIcon(":/toolbar/resources/qt/go-up.png"),
				GlobalObjUI.LMan.GetString("copycontactstofile"),
				menuSimItem));
			
			menuSimItem.AddActions(menuSimActions);
			
			menuSimActions[0].ObjectName = "simadd";
			menuSimActions[1].ObjectName = "simdel";
			menuSimActions[2].ObjectName = "simcopy";
			
			foreach(QAction qa in menuSimActions)
			{
				qa.SetVisible(true);
				qa.IconVisibleInMenu=true;
			}

		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		#endregion Private Methods
		
		
		
		
		
		
		
		
		
		
		
		
		
		

		
		
	}
}

