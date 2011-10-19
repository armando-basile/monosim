
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
		Ui.MainWindow mainwindow_Ui;
		
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
		
		
		
		
		
		
		
		
		
		#region Q_SLOTS
		

		[Q_SLOT]
		public void ActionExit()
		{
			QApplication.CloseAllWindows();			
			QApplication.Quit();
		}

		
		
		
		[Q_SLOT]
		public void ActionChangeReader()
		{
			QAction sender = (QAction)Sender();			
			string newReader = sender.Text;
			
			log.Info("ActionChangeReader " + newReader);
			//UpdateSelectedReader(newReader);
		}
		
		
		
		
		[Q_SLOT]
		public void ActionSettingsSerial()
		{
			OpenSettings();
		}
		
		
		
		
		
		[Q_SLOT]		
		public void ActionInfo()
		{
			OpenInfo();
		}
		
		
		
		
		
		
		/*		
		[Q_SLOT]		
		public void ActionOpen()
		{
			OpenCommandFile();			
		}
		
		
		
		[Q_SLOT]
		public void ActionClose()
		{
			CloseCommandFile();
		}
		

		
		
		
		[Q_SLOT]
		public void ActionATR()
		{
			GetATR();
		}
		
		
		
		
		
		[Q_SLOT]
		public void ActionSendCommand()
		{
			ExchangeData();
		}
		

		
		
		
		[Q_SLOT]
		public void ActionAddCommand(QListWidgetItem qlwi)
		{
			// check for double click
			GetCommandFromList();

		}
		
		
		
		
		[Q_SLOT]
		public void ActionExecCommand()
		{
			QShortcut sender = (QShortcut)Sender();
			
			if (sender.Key.ToString() == "F5")
			{
				// Update text of command
				GetCommandFromList();
			}
			else if (sender.Key.ToString() == "F6")
			{
				// Update text of command,  send it and receive response
				GetCommandFromList();
				ExchangeData();
			}

		}
		
		*/
		#endregion Q_SLOTS
		
		
		
		
		
		
		
		
		
	

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
			columnHeaders.Add(GlobalObjUI.LMan.GetString("descnumber"));
			columnHeaders.Add(GlobalObjUI.LMan.GetString("phonenumber"));
			
			mainwindow_Ui.LstFileContacts.SetVerticalHeaderLabels(columnHeaders);
			mainwindow_Ui.LstSimContacts.SetVerticalHeaderLabels(columnHeaders);
			
			mainwindow_Ui.LstFileContacts.SetColumnWidth(0, 150);
			mainwindow_Ui.LstFileContacts.SetColumnWidth(1, 150);
			mainwindow_Ui.LstSimContacts.SetColumnWidth(0, 150);
			mainwindow_Ui.LstSimContacts.SetColumnWidth(1, 150);

			
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
			
		}
			
		
		
		
		
		
		private void UpdateReactors()
		{

			// Configure events reactors
			Connect( mainwindow_Ui.MenuFileExit, SIGNAL("activated()"), this, SLOT("ActionExit()"));
			
			Connect( mainwindow_Ui.MenuFileSettings, SIGNAL("activated()"), this, SLOT("ActionSettingsSerial()"));
			Connect( mainwindow_Ui.MenuAboutInfo, SIGNAL("activated()"), this, SLOT("ActionInfo()"));
/*
			Connect( mainwindow_Ui.action_Open, SIGNAL("activated()"), this, SLOT("ActionOpen()"));			
			Connect( mainwindow_Ui.action_Close, SIGNAL("activated()"), this, SLOT("ActionClose()"));
			
			
			Connect( mainwindow_Ui.action_ATR, SIGNAL("activated()"), this, SLOT("ActionATR()"));	
			Connect( mainwindow_Ui.BtnSend, SIGNAL("clicked()"), this, SLOT("ActionSendCommand()"));	
			Connect( mainwindow_Ui.LstCommands, SIGNAL("itemDoubleClicked(QListWidgetItem*)"),this,SLOT("ActionAddCommand(QListWidgetItem*)"));
			
			QShortcut qsc = new QShortcut(new QKeySequence("F5"), mainwindow_Ui.LstCommands);
			Connect( qsc, SIGNAL("activated()"),this,SLOT("ActionExecCommand()"));
			
			qsc = new QShortcut(new QKeySequence("F6"), mainwindow_Ui.LstCommands);
			Connect( qsc, SIGNAL("activated()"),this,SLOT("ActionExecCommand()"));
*/
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		#endregion Private Methods
		
		
		
		
		
		
		
		
		
		
		
		
		
		

		
		
	}
}

