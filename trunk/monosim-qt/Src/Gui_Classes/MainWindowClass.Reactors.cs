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
        public void ActionSimDisconnect()
		{
			SimDisconnect();
		}
		
		
		/*		
		[Q_SLOT]		
		public void ActionOpen()
		{
			OpenCommandFile();			
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
			Connect( mainwindow_Ui.MenuSimDisconnect, SIGNAL("activated()"), this, SLOT("ActionSimDisconnect()"));
			
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

