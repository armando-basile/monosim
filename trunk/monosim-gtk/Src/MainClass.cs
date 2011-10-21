using System;
using System.Reflection;
using System.Collections.Generic;

using log4net;
using log4net.Config;

using Gtk;
using Gdk;

using comexbase;
using monosimbase;


namespace monosimgtk
{
	
	public class MainClass
	{
		
		
		// Log4Net object
        private static readonly ILog log = LogManager.GetLogger(typeof(monosimgtk.MainClass));
		
		
		private static string retStr = "";
		
		public static string AppNameVer = "";
		
		
		
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
			
			Application.Init();
			
			retStr = GlobalObj.Initialize(args);
			
			// check for problems detected
			if (retStr != "")
			{
				// check for problem type
				if (!retStr.Contains("SCARD_"))
				{
					// error detected (not scard problem)
					ShowMessage(null, "ERROR", retStr, MessageType.Error);
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
				GlobalObjUI.SetLanguage("monosim-gtk");
			}
			catch (Exception Ex)
			{
				// error detected
				log.Error("GlobalObjUI::SetLanguage: " + Ex.Message + "\r\n" + Ex.StackTrace);
				ShowMessage(null, "LANGUAGE SET ERROR", Ex.Message, MessageType.Error);
				return;
			}
			
			
			// create new Gtk Gui for application and show it
			MainWindowClass mwc = new MainWindowClass();
			mwc.Show();
			Application.Run ();
		}
		
		
		
		
		
		/// <summary>
		/// Show Message Window
		/// </summary>
		public static void ShowMessage(Gtk.Window parent, string title, string message, MessageType mt)
		{
			
			// Gui Message
			MessageDialog mdl = new MessageDialog(parent, 
			                                      DialogFlags.DestroyWithParent, 
			                                      mt, 
			                                      ButtonsType.Ok,
			                                      true,
			                                      message);
			mdl.Show();
			mdl.Title = title;
	        mdl.Icon = Gdk.Pixbuf.LoadFromResource("monosim.png");
	        mdl.Run();
			mdl.Destroy();                  
	
		}
		
		
		
		
		/// <summary>
		/// Wait for pending GTK processes
		/// </summary>
		public static void GtkWait()
		{
			// Update GUI...
			while (Gtk.Application.EventsPending ())
			{
            	Gtk.Application.RunIteration ();
			}
		}
		
		
		
		
		
		
		#endregion Public Methods
		
		
		
		
		
		
		#region Private Methods
		
		
		
		/// <summary>
		/// Get help message to send to console
		/// </summary>
		private static string GetHelpMsg()
		{
			string msg = AppNameVer + " - GTK application to manage sim card contacts\r\n" + 
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
