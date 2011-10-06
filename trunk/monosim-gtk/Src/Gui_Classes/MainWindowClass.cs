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
		
		
		// Attributes
		private string retStr = "";
		private string ATR = "";
		private string simCommand = "";
		private string simResponse = "";
		private string simExpResponse = "";
		private bool simRespOk = false;
		private List<string> ADNrecords = new List<string>();
		
		
		
		
		
		
		
		
		#region Private methods
		
		
		
		
		/// <summary>
		/// Perform change of selected reader
		/// </summary>
		private void UpdateSelectedReader(string newSelReader)
		{
			GlobalObj.CloseConnection();
			GlobalObj.SelectedReader = newSelReader;
			StatusBar.Push(1, GlobalObj.LMan.GetString("selreader") + ": " + newSelReader);
		}
		
		
		
		
		
		
		
		private void NewContactsFile()
		{
			lstFileContacts.Clear();
			LblFile.Markup = "<b>" + GlobalObjUI.LMan.GetString("framefile") + "</b>";
			GlobalObjUI.ContactsFilePath = "";
			LstFileContacts.Sensitive = true;
		}
		
		
		
		private void OpenContactsFile()
		{
			GlobalObjUI.ContactsFilePath = "";
			
			// New dialog for select contacts file 
			Gtk.FileChooserDialog FileBox = new Gtk.FileChooserDialog(GlobalObjUI.LMan.GetString("openfileact"), 
			                                MainWindow,
			                                FileChooserAction.Open, 
			                                GlobalObjUI.LMan.GetString("cancellbl"), Gtk.ResponseType.Cancel,
                                            GlobalObjUI.LMan.GetString("openlbl"), Gtk.ResponseType.Accept);
			
			// Filter for using only monosim files
			Gtk.FileFilter myFilter = new Gtk.FileFilter(); 
			myFilter.AddPattern("*.monosim");
			myFilter.Name = "monosim files";
			FileBox.AddFilter(myFilter);
			
			// Manage result of dialog box
			FileBox.Icon = Gdk.Pixbuf.LoadFromResource("monosim.png");
			int retFileBox = FileBox.Run();
			if ((ResponseType)retFileBox == Gtk.ResponseType.Accept)
			{	
				// path of a right file returned
				GlobalObjUI.ContactsFilePath = FileBox.Filename.ToString();
				
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
			
		}
		
		
		
		
		private void SaveContactsFile()
		{
			
		}
		
		
		
		
		private void SaveContactsFileOnSim()
		{
			
			
		}		

		
		
		private void CloseContactsFile()
		{
			lstFileContacts.Clear();
			LblFile.Markup = "<b>" + GlobalObjUI.LMan.GetString("framefile") + "</b>";
			GlobalObjUI.ContactsFilePath = "";
			LstFileContacts.Sensitive = false;
		}
		
		
		
		/// <summary>
		/// Perform sim card connection and contacts read.
		/// </summary>
		private void SimConnect()
		{
			MainClass.GtkWait();
			
			if (GlobalObj.IsPowered)
			{
				// Disconnect card if needed
				GlobalObj.CloseConnection();
			}
			
			// Connect to smartcard
			retStr = GlobalObj.AnswerToReset(ref ATR);
			
			// check for error
			if (retStr != "")
			{
				// error on answer to reset
				log.Error("MainWindowClass::SimConnect: " + retStr);
				MainClass.ShowMessage("ERROR", retStr, MessageType.Error);
				return;
			}
			
			// read sim contacts and fill list
			retStr = UpdateSimContactsList();
			
			// check for error
			if (retStr != "")
			{
				// error on reading contacts list
				GlobalObj.CloseConnection();
				MainClass.ShowMessage("ERROR", retStr, MessageType.Error);
				return;
			}
			
			
			// update gui widgets with results
			
		}
		
		
		/// <summary>
		/// Update sim contacts list.
		/// </summary>
		private string UpdateSimContactsList()
		{
			bool pin1Status = false;
			string retCmd = GetSimPinStatus(out pin1Status);
			
			if (retCmd != "")
			{
				// error detected
				return retCmd;
			}
			
			if (pin1Status)
			{
				return GlobalObjUI.LMan.GetString("needpindisable");
			}
			
			
			return "";
		}
		
		
		
		/// <summary>
		/// Get sim pin1 status (enabled=true or disabled=false)
		/// </summary>
		private string GetSimPinStatus(out bool pinStatus)
		{
			pinStatus = false;
			
			// Select Master File
			simCommand = "A0A40000023F00";
			simExpResponse = "9F??";
			string retCmd = GlobalObjUI.SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
			log.Debug("MainWindowClass::GetSimPinStatus: SELECT MF " + simResponse);
			
			if (retCmd != "")
			{	
				return retCmd ;
			}
			
			if (!simRespOk)
			{	
				return "WRONG RESPONSE [" + simExpResponse + "] " + "[" + simResponse + "]";
			}
			

			// Get Response
			simCommand = "A0C00000" + simResponse.Substring(2,2);
			simExpResponse = new string('?', (Convert.ToInt32(simResponse.Substring(2,2), 16) * 2)) + "9000";
			retCmd = GlobalObjUI.SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
			log.Debug("MainWindowClass::GetSimPinStatus: GET RESPONSE " + simResponse);
			
			if (retCmd != "")
			{	
				return retCmd ;
			}
			
			if (!simRespOk)
			{	
				return "WRONG RESPONSE [" + simExpResponse + "] " + "[" + simResponse + "]";
			}
			
			// check for Pin1 status
			if ( Convert.ToInt32(simResponse.Substring(26, 1), 16) < 8)
			{
				// Enabled
				pinStatus = true;
			}
			else
			{
				// Disabled
				pinStatus = false;
			}
			
			return "";
			
		}
		
		
		
		
		
		
		
		
		
		private void SimDisconnect()
		{
			
		}
		
		
		private void SimChangePin()
		{
			
		}
		
		
		
		private void SaveContactsSim()
		{
			
		}
		
		
		
		
		
		private void SaveContactsSimOnFile()
		{
			
		}
		
		
		
		
		private void DeleteContactsSim()
		{
			
		}
		
		
		
		
		
		
		
		#endregion Private methods
		
		
	}
	
}
