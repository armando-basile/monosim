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
		private ThreadNotify notify = null;
		private System.Threading.Thread simThread = null;
		
		
		
		
		
		
		
		
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
			GlobalObjUI.ContactsFilePath = "";
			UpdateFileControls(true);
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
				GlobalObjUI.ContactsFilePath = FileBox.Filename;
				
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
			
			
			// Update gui
			UpdateFileControls(false);
			lstFileContacts.Clear();
			MainClass.GtkWait();
			
			try
			{
				GlobalObjUI.FileContacts = new Contacts();
				StreamReader sr = new StreamReader(GlobalObjUI.ContactsFilePath);
				string descRow = sr.ReadLine();			
				string phoneRow = "";
				while (!sr.EndOfStream)
				{
					phoneRow = sr.ReadLine();
					// check for right values
					if (descRow.Trim() != "" && phoneRow.Trim() != "")
					{
						GlobalObjUI.FileContacts.SimContacts.Add(new Contact(descRow, phoneRow));
					}
					
					// read new contact description
					descRow = sr.ReadLine();
				}
				sr.Close();
				sr.Dispose();
				sr = null;			
				
			}
			catch (Exception Ex)
			{
				log.Error("MainWindowClass::OpenContactsFile: " + Ex.Message + "\r\n" + Ex.StackTrace);
				MainClass.ShowMessage(MainWindow, "ERROR", Ex.Message, MessageType.Error);
				return;
			}
			
			// loop to append data readed from file
			foreach(Contact cnt in GlobalObjUI.FileContacts.SimContacts)
			{
				lstFileContacts.AppendValues(new string[]{cnt.Description, cnt.PhoneNumber});
			}
			
			UpdateFileControls(true);
			
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
			GlobalObjUI.ContactsFilePath = "";
			UpdateFileControls(false);
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
				MainClass.ShowMessage(MainWindow, "ERROR", retStr, MessageType.Error);
				return;
			}
			
			// read sim contacts and fill list
			retStr = GlobalObjUI.SelectSimContactsList();
			
			// check for error
			if (retStr != "")
			{
				// error on reading contacts list
				GlobalObj.CloseConnection();
				MainClass.ShowMessage(MainWindow, "ERROR", retStr, MessageType.Error);
				return;
			}
			
			ScanSimBefore();
			
			// Reset status values
			GlobalObjUI.SimADNStatus = 1;
			GlobalObjUI.SimADNPosition = 0;
			GlobalObjUI.SimADNError = "";
			
            // Start thread for reading process
            notify = new ThreadNotify(new ReadyEvent(ReadingUpdate));
            simThread = new System.Threading.Thread(new System.Threading.ThreadStart(GlobalObjUI.ReadSimContactsList));
            simThread.Start();

		}
		
		
		
		
		
		
		
		
		
		
		
		/// <summary>
		/// Disconnect sim card from reader
		/// </summary>
		private void SimDisconnect()
		{
			GlobalObj.CloseConnection();
			UpdateSimControls(false);
			lstSimContacts.Clear();
			MainClass.GtkWait();
			
		}
		
		
		private void SimChangePin()
		{
			
		}
		
		
		
		private void SaveContactsSim()
		{
			
		}
		
		
		
		
		/// <summary>
		/// Save sim contacts on file
		/// </summary>
		private void SaveContactsSimOnFile()
		{
			string fileToSave = "";
			
			// New dialog to save sim contacts on file 
			Gtk.FileChooserDialog FileBox = new Gtk.FileChooserDialog(GlobalObjUI.LMan.GetString("savesimfileact"), 
			                                MainWindow,
			                                FileChooserAction.Save, 
			                                GlobalObjUI.LMan.GetString("cancellbl"), Gtk.ResponseType.Cancel,
                                            GlobalObjUI.LMan.GetString("savelbl"), Gtk.ResponseType.Accept);
			
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
				fileToSave = FileBox.Filename;
				
				string chkfile = fileToSave.PadLeft(9).ToLower();
				if (chkfile.Substring(chkfile.Length-8) != ".monosim")
				{
					fileToSave += ".monosim";
				}
				
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
			
			
			try
			{
				// save contacts
				StreamWriter sw = new StreamWriter(fileToSave,false);
				
				foreach(Contact cnt in GlobalObjUI.SimContacts.SimContacts)
				{
					sw.WriteLine(cnt.Description);
					sw.WriteLine(cnt.PhoneNumber);
				}
				
				sw.Close();
				sw.Dispose();
				sw = null;			
				
			}
			catch (Exception Ex)
			{
				log.Error("MainWindowClass::SaveContactsSimOnFile: " + Ex.Message + "\r\n" + Ex.StackTrace);
				MainClass.ShowMessage(MainWindow, "ERROR", Ex.Message, MessageType.Error);
				return;
			}
			
			MainClass.ShowMessage(MainWindow, "INFO", GlobalObjUI.LMan.GetString("filesaved"),MessageType.Info);
			
		}
		
		
		
		
		private void DeleteContactsSim()
		{
			
		}
		
		
		
		
		
		
		
		/// <summary>
		/// Updates during sim contacts reading
		/// </summary>
		private void ReadingUpdate()
		{
			PBar.Adjustment.Value = (double)GlobalObjUI.SimADNPosition;
			StatusBar.Push(1, GlobalObjUI.LMan.GetString("readcontact") + 
				              GlobalObjUI.SimADNPosition.ToString("d3"));
			MainClass.GtkWait();
			
			
			if (GlobalObjUI.SimADNStatus == 3)
			{
				// End with errors
				MainClass.ShowMessage(MainWindow, "ERROR", GlobalObjUI.SimADNError, MessageType.Error);
				
				// update gui widgets with results
				UpdateSimControls(false);
			}
			
			if (GlobalObjUI.SimADNStatus == 2)
			{
				// Extract contacts from records
				retStr = GlobalObjUI.FromRecordsToContacts();
				
				if (retStr != "")
				{
					// error detected
					MainClass.ShowMessage(MainWindow, "ERROR", retStr, MessageType.Error);
					
					// update gui widgets with results
					UpdateSimControls(false);
				}
				else
				{
					// update ListView
					foreach(Contact cnt in GlobalObjUI.SimContacts.SimContacts)
					{
						lstSimContacts.AppendValues(new string[]{cnt.Description, cnt.PhoneNumber });
					}
					
					// update gui widgets with results
					UpdateSimControls(true);
				}
			}

			// check for sim scan ended
			if (GlobalObjUI.SimADNStatus != 1)
			{
				// Update gui widgets properties
				ScanSimAfter();
			}
			
			
			
		}
		
		
		
		
		
		
		
		
		
		
		/// <summary>
		/// Update sim widgets status
		/// </summary>
		private void UpdateSimControls(bool isSensitive)
		{
			MenuSimConnect.Sensitive = !isSensitive;
			MenuSimSaveSim.Sensitive = isSensitive;
			MenuSimSaveFile.Sensitive = isSensitive;
			MenuSimDeleteAll.Sensitive = isSensitive;
			MenuSimPin.Sensitive = isSensitive;
			MenuSimDisconnect.Sensitive = isSensitive;
			
			TbOpenSim.Sensitive = !isSensitive;
			TbSaveSimSim.Sensitive = isSensitive;
			TbSaveSimFile.Sensitive = isSensitive;
			TbChangePin.Sensitive = isSensitive;
			TbCloseSim.Sensitive = isSensitive;
			LstSimContacts.Sensitive = isSensitive;
			
			if (isSensitive)
			{
				// add iccid to frame label
				LblSim.Markup = "<b>" + GlobalObjUI.LMan.GetString("framesim") + "</b> [" +
					GlobalObjUI.SimIccID + " - size: " + GlobalObjUI.SimADNRecordCount.ToString() + "]"; 
				
				StatusBar.Push(1, GlobalObjUI.LMan.GetString("recordnoempty") + 
					              GlobalObjUI.SimADNRecordNoEmpty.ToString());
			}
			else
			{
				// clear frame label
				LblSim.Markup = "<b>" + GlobalObjUI.LMan.GetString("framesim") + "</b>";
			}
		}

		
		
		
		

		/// <summary>
		/// Update file widgets status
		/// </summary>
		private void UpdateFileControls(bool isSensitive)
		{
			MenuFileOpen.Sensitive = !isSensitive;
			MenuFileSaveFile.Sensitive = isSensitive;
			MenuFileSaveSim.Sensitive = isSensitive;
			MenuFileClose.Sensitive = isSensitive;
			
			TbOpen.Sensitive = !isSensitive;
			TbSaveFile.Sensitive = isSensitive;
			TbSaveSim.Sensitive = isSensitive;
			TbClose.Sensitive = isSensitive;
			LstFileContacts.Sensitive = isSensitive;
			
			
			if (isSensitive)
			{
				// add filename to frame label
				LblFile.Markup = "<b>" + GlobalObjUI.LMan.GetString("framefile") + "</b> [" +
					Path.GetFileNameWithoutExtension(GlobalObjUI.ContactsFilePath) + 
					" - size: " + GlobalObjUI.FileContacts.SimContacts.Count.ToString() + "]"; 
			}
			else
			{
				// clear frame label
				LblFile.Markup = "<b>" + GlobalObjUI.LMan.GetString("framefile") + "</b>";
			}
		}
		
		
		
		
		
		
		/// <summary>
		/// Set gui widgets before sim scan
		/// </summary>
		private void ScanSimBefore()
		{
			// Setup ProgressBar
			PBar.Fraction = 0;
			PBar.Adjustment.Lower=0;
			PBar.Adjustment.Upper=GlobalObjUI.SimADNRecordCount;
			PBar.Adjustment.Value=0;
			PBar.Visible=true;
			MainMenu.Sensitive = false;
			TopToolBar.Sensitive = false;
			FrameSim.Sensitive = false;
			FrameFile.Sensitive = false;
			lstSimContacts.Clear();
			MainClass.GtkWait();
		}

		
		
		/// <summary>
		/// Set gui widgets after sim scan
		/// </summary>
		private void ScanSimAfter()
		{
			PBar.Visible = false;
			MainMenu.Sensitive = true;
			TopToolBar.Sensitive = true;
			FrameSim.Sensitive = true;
			FrameFile.Sensitive = true;
			MainClass.GtkWait();
		}
		
		
		
		
		
		
		#endregion Private methods
		
		
	}
	
}
