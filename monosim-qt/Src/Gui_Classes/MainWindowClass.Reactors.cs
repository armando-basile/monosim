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
			UpdateSelectedReader(newReader);
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
		
		
		
		[Q_SLOT]
		public void ActionFileNew()
		{
			NewContactsFile();
		}

		
		
		
		[Q_SLOT]
		public void ActionFileOpen()
		{
			OpenContactsFile();
		}
		
		
		
		[Q_SLOT]
		public void ActionFileSaveFile()
		{
			SaveContactsFile();
		}
		
		
		
        [Q_SLOT]
        public void ActionFileSaveSim()
        {
        	SaveContactsFileOnSim();
        }
		
		
		
		[Q_SLOT]
		public void ActionFileClose()
		{
			CloseContactsFile();
		}
		
		
		
        [Q_SLOT]
        public void ActionThreadNotify()
        {
			if (isReading)
			{
        		ReadingUpdate();
			}
			else
			{
				WritingUpdate();
			}
        }
		
		
		
		
        [Q_SLOT]
        public void ActionSimConnect()
        {
        	SimConnect();
        }
		
		
		
		[Q_SLOT]
        public void ActionSimSaveFile()
        {
        	SaveContactsSimOnFile();
        }

		
		
		[Q_SLOT]
        public void ActionSimSaveSim()
        {
        	SaveContactsSim();
        }
		
		
		
		[Q_SLOT]
        public void ActionSimChangePin()
		{
			SimChangePin();
		}
		
		
		
		[Q_SLOT]
        public void ActionSimDeleteAll()
		{
			DeleteContactsSim();
		}		
		
		
		
		[Q_SLOT]
        public void ActionSimDisconnect()
		{
			SimDisconnect();
		}
		
		
		
		
		[Q_SLOT]
        public void ActionFileContactsMenu(QPoint point)
		{
			menuFileActions[2].Enabled = false;
			menuFileActions[1].Enabled = false;
			if ((mainwindow_Ui.LstFileContacts.SelectedItems().Count > 0) &&
				(mainwindow_Ui.LstSimContacts.Enabled))
			{
				menuFileActions[2].Enabled = true;
			}

			if (mainwindow_Ui.LstFileContacts.SelectedItems().Count > 0)
			{
				menuFileActions[1].Enabled = true;
			}

			menuFileItem.Popup(mainwindow_Ui.LstFileContacts.MapToGlobal(point));
		}
		
		
		
		[Q_SLOT]
        public void ActionSimContactsMenu(QPoint point)
		{
			menuSimActions[2].Enabled = false;
			menuSimActions[1].Enabled = false;
			if ((mainwindow_Ui.LstSimContacts.SelectedItems().Count > 0) &&
				(mainwindow_Ui.LstFileContacts.Enabled))
			{
				menuSimActions[2].Enabled = true;
			}
			
			if (mainwindow_Ui.LstSimContacts.SelectedItems().Count > 0)
			{
				menuSimActions[1].Enabled = true;
			}
			
			menuSimItem.Popup(mainwindow_Ui.LstSimContacts.MapToGlobal(point));
		}		
		
		
		
		
		
		[Q_SLOT]
        public void ActionAddContact()
		{
			QAction sender = (QAction)Sender();
			
			if (sender.ObjectName == "fileadd")
			{
				PopupFileAdd();
			}
			else if (sender.ObjectName == "simadd")
			{
				PopupSimAdd();
			}
		}
		
		
		
		[Q_SLOT]
        public void ActionDelContact()
		{
			QAction sender = (QAction)Sender();
			
			if (sender.ObjectName == "filedel")
			{
				PopupFileDel();
			}
			else if (sender.ObjectName == "simdel")
			{
				PopupSimDel();
			}
		}
		
		
		
		[Q_SLOT]
        public void ActionCopyContact()
		{
			QAction sender = (QAction)Sender();
			
			if (sender.ObjectName == "filecopy")
			{
				PopupFileMoveToSim();
			}
			else if (sender.ObjectName == "simcopy")
			{
				PopupSimMoveToFile();
			}
		}
		
		
		
		
		#endregion Q_SLOTS
		
		
		
		
		
		
		
		
		
		
		
		#region Private Methods
		
		
		

		private void UpdateReactors()
		{
			
			notify = new ThreadNotify();
			GlobalObjUI.MonosimEvent += SimEvent;

			// Connect Signal
            Connect( notify, SIGNAL("UpdateGui()"), this, SLOT("ActionThreadNotify()"));  
			
			// Configure events reactors
			Connect( mainwindow_Ui.MenuFileNew, SIGNAL("activated()"), this, SLOT("ActionFileNew()"));
			Connect( mainwindow_Ui.MenuFileOpen, SIGNAL("activated()"), this, SLOT("ActionFileOpen()"));
			Connect( mainwindow_Ui.MenuFileSaveFile, SIGNAL("activated()"), this, SLOT("ActionFileSaveFile()"));
			Connect( mainwindow_Ui.MenuFileSaveSim, SIGNAL("activated()"), this, SLOT("ActionFileSaveSim()"));
			Connect( mainwindow_Ui.MenuFileClose, SIGNAL("activated()"), this, SLOT("ActionFileClose()"));
			Connect( mainwindow_Ui.MenuFileSettings, SIGNAL("activated()"), this, SLOT("ActionSettingsSerial()"));
			Connect( mainwindow_Ui.MenuFileExit, SIGNAL("activated()"), this, SLOT("ActionExit()"));
			
			Connect( mainwindow_Ui.MenuSimConnect, SIGNAL("activated()"), this, SLOT("ActionSimConnect()"));
			Connect( mainwindow_Ui.MenuSimPin, SIGNAL("activated()"), this, SLOT("ActionSimChangePin()"));
			Connect( mainwindow_Ui.MenuSimSaveFile, SIGNAL("activated()"), this, SLOT("ActionSimSaveFile()"));
			Connect( mainwindow_Ui.MenuSimSaveSim, SIGNAL("activated()"), this, SLOT("ActionSimSaveSim()"));
			Connect( mainwindow_Ui.MenuSimDeleteAll, SIGNAL("activated()"), this, SLOT("ActionSimDeleteAll()"));
			Connect( mainwindow_Ui.MenuSimDisconnect, SIGNAL("activated()"), this, SLOT("ActionSimDisconnect()"));
			
			Connect( mainwindow_Ui.MenuAboutInfo, SIGNAL("activated()"), this, SLOT("ActionInfo()"));
			
			mainwindow_Ui.LstFileContacts.ContextMenuPolicy = Qt.ContextMenuPolicy.CustomContextMenu;
			Connect( mainwindow_Ui.LstFileContacts, SIGNAL("customContextMenuRequested(QPoint)"), this, SLOT("ActionFileContactsMenu(QPoint)"));
			
			mainwindow_Ui.LstSimContacts.ContextMenuPolicy = Qt.ContextMenuPolicy.CustomContextMenu;
			Connect( mainwindow_Ui.LstSimContacts, SIGNAL("customContextMenuRequested(QPoint)"), this, SLOT("ActionSimContactsMenu(QPoint)"));
			
			Connect( menuFileActions[0], SIGNAL("activated()"), this, SLOT("ActionAddContact()"));
			Connect( menuSimActions[0], SIGNAL("activated()"), this, SLOT("ActionAddContact()"));
			Connect( menuFileActions[1], SIGNAL("activated()"), this, SLOT("ActionDelContact()"));
			Connect( menuSimActions[1], SIGNAL("activated()"), this, SLOT("ActionDelContact()"));
			Connect( menuFileActions[2], SIGNAL("activated()"), this, SLOT("ActionCopyContact()"));
			Connect( menuSimActions[2], SIGNAL("activated()"), this, SLOT("ActionCopyContact()"));
		}
		
		
		
		
		
		
		
		
		
		
		#endregion Private Methods
		
		
		
		
		
		
		
		
		
	}
}

