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

		
		

		
		#region Reactors
		
		
		/// <summary>
		/// Close GtkWindows
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionCancel(object sender, EventArgs args)
		{
			GlobalObj.CloseConnection();
			
			MainWindow.Destroy();
            MainWindow.Dispose();
			Application.Quit();
		}
		
		
		
		/// <summary>
		/// Open Info about window
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionAbout(object sender, EventArgs args)
		{
			AboutDialogClass adc = new AboutDialogClass();
			adc.SetParent(ref MainWindow);			
			adc.About = GlobalObjUI.LMan.GetString("infodesc").Replace("\t", "")
				.Replace("<br>", "")
				.Replace("&nbsp;", " ");
			
			adc.Thanks = GlobalObjUI.LMan.GetString("thanksdesc").Replace("\t", "")
				.Replace("<br>", "")
				.Replace("&nbsp;", " ") + monosimbase.GlobalConst.ThanksTo;
			
			adc.Description = "<b>Monosim Gtk</b> - " + GlobalObjUI.LMan.GetString("description");
			
			adc.Title = GlobalObjUI.LMan.GetString("frmabout");

			adc.Show();
		}
		
		
		
		

		

		
		
		/// <summary>
		/// Create new contacts file
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionFileNew(object sender, EventArgs args)
		{
			NewContactsFile();
		}
		
		
		
		/// <summary>
		/// Open contacts file
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionFileOpen(object sender, EventArgs args)
		{
			OpenContactsFile();
		}

		

		/// <summary>
		/// Save contacts file
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionFileSave(object sender, EventArgs args)
		{
			SaveContactsFile();
		}
		
		
		
		
		/// <summary>
		/// Save contacts file on sim card
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionFileSaveSim(object sender, EventArgs args)
		{
			SaveContactsFileOnSim();
		}
		
		
		
		
		/// <summary>
		/// Close opened contacts file
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionFileClose(object sender, EventArgs args)
		{
			CloseContactsFile();
		}
		
		
		
		
		
		
		
		
		
		
		/// <summary>
		/// Connection to sim
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionSimConnect(object sender, EventArgs args)
		{
			SimConnect();
		}
		
		
		
		
		/// <summary>
		/// Change Pin on sim
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionSimChangePin(object sender, EventArgs args)
		{
			SimChangePin();
		}
		
		
		
		/// <summary>
		/// Save
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionSimSaveFile(object sender, EventArgs args)
		{
			SaveContactsSimOnFile();
		}
		
		
		
		/// <summary>
		/// Save
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionSimSave(object sender, EventArgs args)
		{
			SaveContactsSim();
		}
		
		
		/// <summary>
		/// Delete
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionSimDeleteAll(object sender, EventArgs args)
		{
			DeleteContactsSim();
		}
		
		
		
		/// <summary>
		/// Disconnect sim from reader
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionSimDisconnect(object sender, EventArgs args)
		{
			SimDisconnect();
		}
		
		
		
		
		/// <summary>
		/// ChangeReader
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionChangeReader(object sender, ButtonReleaseEventArgs args)
		{
			string newReader = ((AccelLabel)(((RadioMenuItem)(sender)).Children[0])).Text;
			log.Info("Changing reader to " + newReader);
			UpdateSelectedReader(newReader);
		}
		
		
		
		
		/// <summary>
		/// Open settings dialog
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionSettings(object sender, EventArgs args)
		{
			SettingsDialogClass sdc = new SettingsDialogClass();
			sdc.SetParent(ref MainWindow);
			sdc.Show();
		}
		
		
		
		
		
		
		
		
		/// <summary>
		/// Add contacts to file 
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionFileAdd(object sender, EventArgs args)
		{
			PopupFileAdd();
		}
		
		

		
		
		/// <summary>
		/// Del contacts to file 
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionFileDel(object sender, EventArgs args)
		{
			PopupFileDel();
		}

		
		
		/// <summary>
		/// Add contacts to sim from file 
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionFileMoveToSim(object sender, EventArgs args)
		{
			PopupFileMoveToSim();
		}

		
		
		
		/// <summary>
		/// Add contacts to sim 
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionSimAdd(object sender, EventArgs args)
		{
			PopupSimAdd();
		}
		
		

		
		
		/// <summary>
		/// Del contacts on sim
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionSimDel(object sender, EventArgs args)
		{
			PopupSimDel();
		}

		
		
		/// <summary>
		/// Add contacts from sim to file
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionSimMoveToFile(object sender, EventArgs args)
		{
			PopupSimMoveToFile();
		}

		
		
		
		
		
		
		/// <summary>
		/// Show Popup menu for file contacts list
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionListFileContactsButtonPress(object sender, ButtonPressEventArgs a)
		{
			Gdk.EventButton ev = a.Event;
			
			// check for mouse right button
			if (a.Event.Button == 3)
			{
				// check for items presence
				if (lstFileContacts.IterNChildren() == 0)
				{
					PopMenuFileDel.Sensitive = false;
					PopMenuFileVSim.Sensitive = false;
				}
				else
				{
					PopMenuFileDel.Sensitive = true;
					
					// check for contacts file presence
					if (LstSimContacts.Sensitive)
					{
						PopMenuFileVSim.Sensitive = true;
					}
					else
					{
						PopMenuFileVSim.Sensitive = false;
					}
				}

				PopMenuFile.Popup(null, null, null, ev.Button, ev.Time);
				PopMenuFile.ShowAll();
			}
		}
		
		
		
		
		/// <summary>
		/// Show Popup menu for sim contacts list
		/// </summary>
		[GLib.ConnectBefore]
		public void ActionListSimContactsButtonPress(object sender, ButtonPressEventArgs a)
		{
			Gdk.EventButton ev = a.Event;

			// check for mouse right button
			if (a.Event.Button == 3)
			{
				// check for items presence
				if (lstSimContacts.IterNChildren() == 0)
				{
					PopMenuSimDel.Sensitive = false;
					PopMenuSimVFile.Sensitive = false;
				}
				else
				{
					PopMenuSimDel.Sensitive = true;
					
					// check for sim card presence
					if (LstFileContacts.Sensitive)
					{
						PopMenuSimVFile.Sensitive = true;
					}
					else
					{
						PopMenuSimVFile.Sensitive = false;
					}
				}
				
				PopMenuSim.Popup(null, null, null, ev.Button, ev.Time);
				PopMenuSim.ShowAll();
			}
		}
		
		
		
		
		/// <summary>
		/// Sim event arrived
		/// </summary>
		private void SimEvent(object sender, EventArgs eArgs)
		{
			// recall gtk update method
			notify.WakeupMain();
		}
		
		
		
		
		
		
		#endregion Reactors

		
		
		
		
		
		
	}
}

