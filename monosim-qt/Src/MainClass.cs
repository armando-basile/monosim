using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using Qyoto;

using comexbase;
using monosimbase;

namespace monosimqt
{
	public class MainClass: Qt
	{
		
		
		// Log4Net object
        private static readonly ILog log = LogManager.GetLogger(typeof(monosimqt.MainClass));
		
		// Attributes
		private static string retStr = "";
		
		
		// Properties
		public static string AppNameVer = "";
		
		
		
		public enum MessageType
		{
			Info=0,
			Warning=1,
			Error=2,
		}
		
		
		
		#region Public Methods
		
		
		
		[STAThread]
		public static void Main (string[] args)
		{
			AppNameVer = Assembly.GetExecutingAssembly().GetName().Name + " " +
					     Assembly.GetExecutingAssembly().GetName().Version.ToString();
			
			// check for help request
			if (new List<string>(args).Contains("--help"))
			{
				Console.WriteLine(GetHelpMsg());
				return;
			}


			// Init resource class manager			
			Q_INIT_RESOURCE("ResManager");
			
			
			// Create new Qyoto Application
			new QApplication(args);

			
			retStr = GlobalObj.Initialize(args);
			
			// check for problems detected
			if (retStr != "")
			{
				// check for problem type
				if (!retStr.Contains("SCARD_"))
				{
					// error detected (not scard problem)
					ShowMessage(null, "ERROR", retStr, MessageType.Error);
					QApplication.Quit();					
					return;
				}
				else
				{
					// warning (scard problem, can use serial reader)
					ShowMessage(null, "WARNING", retStr, MessageType.Warning);
				}
				
			}
			
			try
			{
				// try to set language
				GlobalObjUI.SetLanguage("monosim-qt");
			}
			catch (Exception Ex)
			{
				// error detected
				log.Error("GlobalObjUI::SetLanguage: " + Ex.Message + "\r\n" + Ex.StackTrace);
				ShowMessage(null, "LANGUAGE SET ERROR", Ex.Message, MessageType.Error);
				return;
			}
			
			// Create new Qyoto Desktop Object
			QDesktopWidget qdw = new QDesktopWidget();
			
			// Create MainWindow class manager
			MainWindowClass mwc = new MainWindowClass();
			
			int wWidth = Convert.ToInt32(mwc.Width() / 2);
			int wHeight = Convert.ToInt32(mwc.Height() / 2);
			int dWidth = Convert.ToInt32(qdw.Width() / 2);
			int dHeight = Convert.ToInt32(qdw.Height() / 2);
			
			mwc.Move(dWidth - wWidth, dHeight - wHeight - 20);
			
			mwc.Show();
			
			
			// Run Qyoto Application
			QApplication.Exec();
			
		}
		
		
		
		
		
		/// <summary>
		/// Show Message Window
		/// </summary>
		public static void ShowMessage(QWidget parent, string title, string message, MessageType mt)
		{
			// Gui Message
			if (mt == MainClass.MessageType.Info)
			{
				QMessageBox.Information(parent, title, message);
			}
			else if (mt == MainClass.MessageType.Warning)
			{
				QMessageBox.Warning(parent, title, message);
			}
			else if (mt == MainClass.MessageType.Error)
			{
				QMessageBox.Critical(parent, title, message);
			}			
		}
		
		
		
		
		
		/// <summary>
		/// Wait for pending QT processes
		/// </summary>
		public static void QtWait()
		{
			// Update GUI...
			QApplication.ProcessEvents();
		}
		
		
		
		
		
		
		#endregion Public Methods
		
		
		
		
		
		
		#region Private Methods
		
		
		
		/// <summary>
		/// Get help message to send to console
		/// </summary>
		private static string GetHelpMsg()
		{
			string msg = AppNameVer + " - Qt application to manage sim card contacts\r\n" + 
				         GlobalObj.AppNameVer + " - base component\r\n\r\n";
			msg += "   usage:\r\n";
			msg += "   --log-console     enable log into console\r\n";
			msg += "   --log-file        enable log into file comex.log into home folder\r\n";
			msg += "   --help            show this message\r\n";
			
			return msg;
		}
		
		
		
		
		
		
		
		#endregion Private Methods
		
		
		
	}
	
}


