
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
		private string retStr = "";
		private string ATR = "";
		private ThreadNotify notify = null;
		private System.Threading.Thread simThread = null;
		private List<string> allReaders = new List<string>();
		private bool isEnd = false;
		private bool isReading = false;
		
		
		
		#region Private Methods
		
		
		
		
		
		/// <summary>
		/// Information about
		/// </summary>
		private void OpenInfo()
		{
			AboutDialogClass adc = new AboutDialogClass();
			adc.Show();
			
		}
		
		
		
		
		
		/// <summary>
		/// Show settings dialog
		/// </summary>
		private void OpenSettings()
		{
			SettingsDialogClass sdc = new SettingsDialogClass();
			sdc.Show();
		}
		
		
		
		
		
		
		/// <summary>
		/// Perform change of selected reader
		/// </summary>
		private void UpdateSelectedReader(string newSelReader)
		{
			GlobalObj.CloseConnection();
			GlobalObj.SelectedReader = newSelReader;
			mainwindow_Ui.StatusBar.ShowMessage(GlobalObj.LMan.GetString("selreader") + ": " + newSelReader);
		}
		
		
		
		
		
		
		/// <summary>
		/// Create new contact file
		/// </summary>
		private void NewContactsFile()
		{
			mainwindow_Ui.LstFileContacts.Clear();
			GlobalObjUI.FileContacts = new Contacts();
			GlobalObjUI.ContactsFilePath = "";
			UpdateFileControls(true);
		}
		
		
		
		

		/// <summary>
		/// Update file widgets status
		/// </summary>
		private void UpdateFileControls(bool isSensitive)
		{
			mainwindow_Ui.MenuFileNew.Enabled = !isSensitive;
			mainwindow_Ui.MenuFileOpen.Enabled = !isSensitive;
			mainwindow_Ui.MenuFileSaveFile.Enabled = isSensitive;			
			mainwindow_Ui.MenuFileClose.Enabled = isSensitive;
			mainwindow_Ui.LstFileContacts.Enabled = isSensitive;
			
			
			if (isSensitive)
			{
				// add filename to frame label
				mainwindow_Ui.FrameFile.Title = GlobalObjUI.LMan.GetString("framefile");
				if (GlobalObjUI.ContactsFilePath != "")
				{
					mainwindow_Ui.FrameFile.Title = GlobalObjUI.LMan.GetString("framefile")+
						" [" + Path.GetFileNameWithoutExtension(GlobalObjUI.ContactsFilePath) + 
					" - size: " + GlobalObjUI.FileContacts.SimContacts.Count.ToString() + "]"; 					
				}
				
				// check for sim power on
				if (mainwindow_Ui.LstSimContacts.Enabled)
				{
					mainwindow_Ui.MenuFileSaveSim.Enabled = isSensitive;
				}
				else
				{
					mainwindow_Ui.MenuFileSaveSim.Enabled = false;
				}
			}
			else
			{
				// clear frame label
				mainwindow_Ui.FrameFile.Title = GlobalObjUI.LMan.GetString("framefile");
				mainwindow_Ui.MenuFileSaveSim.Enabled = isSensitive;
			}
		}
		
		
		
		
		/// <summary>
		/// Close contacts file
		/// </summary>
		private void CloseContactsFile()
		{
			mainwindow_Ui.LstFileContacts.Clear();
			GlobalObjUI.ContactsFilePath = "";
			UpdateFileControls(false);
		}
		
		
		
		

		/// <summary>
		/// Update sim widgets status
		/// </summary>
		private void UpdateSimControls(bool isSensitive)
		{
			mainwindow_Ui.MenuSimConnect.Enabled = !isSensitive;
			mainwindow_Ui.MenuSimSaveSim.Enabled = isSensitive;
			mainwindow_Ui.MenuSimSaveFile.Enabled = isSensitive;
			mainwindow_Ui.MenuSimDeleteAll.Enabled = isSensitive;
			mainwindow_Ui.MenuSimPin.Enabled = isSensitive;
			mainwindow_Ui.MenuSimDisconnect.Enabled = isSensitive;
			mainwindow_Ui.LstSimContacts.Enabled = isSensitive;
			
			if (isSensitive)
			{
				// add iccid to frame label
				mainwindow_Ui.FrameSim.Title = GlobalObjUI.LMan.GetString("framesim") + " [" +
					GlobalObjUI.SimIccID + " - size: " + GlobalObjUI.SimADNRecordCount.ToString() + "]"; 
				
				mainwindow_Ui.StatusBar.ShowMessage(GlobalObjUI.LMan.GetString("recordnoempty") + 
					GlobalObjUI.SimADNRecordNoEmpty.ToString());
				
				// check for File area enabled
				if (mainwindow_Ui.LstFileContacts.Enabled)
				{
					mainwindow_Ui.MenuFileSaveSim.Enabled = isSensitive;
				}
				else
				{
					mainwindow_Ui.MenuFileSaveSim.Enabled = false;
				}
				
			}
			else
			{
				// clear frame label
				mainwindow_Ui.FrameSim.Title = GlobalObjUI.LMan.GetString("framesim");
				mainwindow_Ui.MenuFileSaveSim.Enabled = isSensitive;
			}
			
		}

		
		
		
		/// <summary>
		/// Open contacts file.
		/// </summary>
		private void OpenContactsFile()
		{
			GlobalObjUI.ContactsFilePath = "";
			
			// New dialog for select contacts file 
			string selectedFile = QFileDialog.GetOpenFileName(this, 
                                                        GlobalObjUI.LMan.GetString("openfileact"),
                                                        null,
                                                        "*.monosim");
            
			if (string.IsNullOrEmpty(selectedFile))
            {
            	return;
            }
			
			// path of a right file returned
			GlobalObjUI.ContactsFilePath = selectedFile;
			
			// Update gui
			UpdateFileControls(false);	
					
			// Clear ListView
			mainwindow_Ui.LstFileContacts.Clear();			
			MainClass.QtWait();
			
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
				MainClass.ShowMessage(this, "ERROR", Ex.Message,MainClass.MessageType.Error);
				return;
			}
			
			// loop to append data readed from file			
			List<string> rowContent = null;
			foreach(Contact cnt in GlobalObjUI.FileContacts.SimContacts)
			{
				rowContent = new List<string>();
				rowContent.Add(" ");
				rowContent.Add(cnt.Description);
				rowContent.Add(cnt.PhoneNumber);
				new QTreeWidgetItem(mainwindow_Ui.LstFileContacts, rowContent);				
			}
			
			UpdateFileControls(true);
			
		}
		
		
		
		

		/// <summary>
		/// Perform sim card connection and contacts read.
		/// </summary>
		private void SimConnect()
		{
			MainClass.QtWait();
			
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
				MainClass.ShowMessage(this, "ERROR", retStr, MainClass.MessageType.Error);
				return;
			}
			
			// read sim contacts and fill list
			retStr = GlobalObjUI.SelectSimContactsList();
			
			// check for error
			if (retStr != "")
			{
				// error on reading contacts list
				GlobalObj.CloseConnection();
				MainClass.ShowMessage(this, "ERROR", retStr, MainClass.MessageType.Error);
				return;
			}
			
			ScanSimBefore();
			mainwindow_Ui.LstSimContacts.Clear();
			
			// Reset status values
			GlobalObjUI.SimADNStatus = 1;
			GlobalObjUI.SimADNPosition = 0;
			GlobalObjUI.SimADNError = "";
			
            // Start thread for reading process
			isEnd = false;
			isReading = true;
            simThread = new System.Threading.Thread(new 
				System.Threading.ThreadStart(GlobalObjUI.ReadSimContactsList));
            simThread.Start();

		}
		
		
		
		
		
		
		/// <summary>
		/// Set gui widgets before sim scan
		/// </summary>
		private void ScanSimBefore()
		{
			// Setup ProgressBar
			PBar.SetMinimum(0);
			PBar.SetMaximum(GlobalObjUI.SimADNRecordCount);
			PBar.SetValue(0);
			PBar.SetVisible(true);
			mainwindow_Ui.MainMenu.Enabled = false;
			mainwindow_Ui.TopToolBar.Enabled = false;
			mainwindow_Ui.FrameSim.Enabled = false;
			mainwindow_Ui.FrameFile.Enabled = false;
			MainClass.QtWait();
		}

		
		
		/// <summary>
		/// Set gui widgets after sim scan
		/// </summary>
		private void ScanSimAfter()
		{
			PBar.SetVisible(false);
			mainwindow_Ui.MainMenu.Enabled = true;
			mainwindow_Ui.TopToolBar.Enabled = true;
			mainwindow_Ui.FrameSim.Enabled = true;
			mainwindow_Ui.FrameFile.Enabled = true;
			MainClass.QtWait();
		}
		
		
		

        
        /// <summary>
        /// Updates during sim contacts reading
        /// </summary>
        private void ReadingUpdate()
        {
            PBar.SetValue(GlobalObjUI.SimADNPosition);
            mainwindow_Ui.StatusBar.ShowMessage(GlobalObjUI.LMan.GetString("readcontact") + 
                                  GlobalObjUI.SimADNPosition.ToString("d3"));
            MainClass.QtWait();
            
            
            if (GlobalObjUI.SimADNStatus == 3 && !isEnd)
            {
				isEnd = true;
                // End with errors
                MainClass.ShowMessage(this, "ERROR", GlobalObjUI.SimADNError, MainClass.MessageType.Error);
                
				// Update gui widgets properties
                ScanSimAfter();
				
                // update gui widgets with results
                UpdateSimControls(false);
            }
            
            if (GlobalObjUI.SimADNStatus == 2 && !isEnd)
            {
				isEnd = true;
				
                // Extract contacts from records
                retStr = GlobalObjUI.FromRecordsToContacts();
                
                if (retStr != "")
                {
                    // error detected
                    MainClass.ShowMessage(this, "ERROR", retStr, MainClass.MessageType.Error);
                    
					// Update gui widgets properties
                	ScanSimAfter();
					
                    // update gui widgets with results
                    UpdateSimControls(false);
                }
                else
                {
                    // update ListView
					List<string> rowContent = null;
                    foreach(Contact cnt in GlobalObjUI.SimContacts.SimContacts)
                    {
						rowContent = new List<string>();
						rowContent.Add(" ");
						rowContent.Add(cnt.Description);
						rowContent.Add(cnt.PhoneNumber);
						new QTreeWidgetItem(mainwindow_Ui.LstSimContacts, rowContent);
                    }
                    
					// Update gui widgets properties
                	ScanSimAfter();
					
                    // update gui widgets with results
                    UpdateSimControls(true);
                }
            }
        }

		
		
		

		/// <summary>
		/// Disconnect sim card from reader
		/// </summary>
		private void SimDisconnect()
		{
			GlobalObj.CloseConnection();
			UpdateSimControls(false);
			mainwindow_Ui.LstSimContacts.Clear();
			MainClass.QtWait();
			
		}
		
		
		
		
		/// <summary>
		/// Save file contacts on file.
		/// </summary>
		private void SaveContactsFile()
		{
			QMessageBox mdlg = null;
			
			if (GlobalObjUI.ContactsFilePath != "")
			{
				mdlg = new QMessageBox(QMessageBox.Icon.Question, 
					MainClass.AppNameVer + " - " + GlobalObjUI.LMan.GetString("savefileact"),
					GlobalObjUI.LMan.GetString("override") + "\r\n" + 
					Path.GetFileNameWithoutExtension(GlobalObjUI.ContactsFilePath),
					0x00400400 , this);
				
				int respType = mdlg.Exec();

				if (respType == 0x00000400)
				{
					// override (Ok)
					mdlg.Close();
					mdlg.Dispose();
					mdlg = null;
					
					WriteContactsOnFile(GlobalObjUI.ContactsFilePath, GlobalObjUI.FileContacts.SimContacts);
					return;
				}

				mdlg.Close();
				mdlg.Dispose();
				mdlg = null;

			}
			
			// select new file to save
			string fileToSave = ChooseFileToSave(GlobalObjUI.LMan.GetString("savefileact"));
			if (fileToSave == "")
			{
				// no file selected
				return;
			}
			
			WriteContactsOnFile(fileToSave, GlobalObjUI.FileContacts.SimContacts);
			GlobalObjUI.ContactsFilePath = fileToSave;
			
		}
		
		
		
		
		
		
		/// <summary>
		/// Save sim contacts on file
		/// </summary>
		private void SaveContactsSimOnFile()
		{
			string fileToSave = ChooseFileToSave(GlobalObjUI.LMan.GetString("savesimfileact"));
			if (fileToSave == "")
			{
				// no file selected
				return;
			}
			
			WriteContactsOnFile(fileToSave, GlobalObjUI.SimContacts.SimContacts);
		}
		
		
		
		

		/// <summary>
		/// Choose file to save contacts.
		/// </summary>
		private string ChooseFileToSave(string dialogTitle)
		{
			// New dialog for select contacts file 
			string fileToSave = QFileDialog.GetOpenFileName(this, 
                                GlobalObjUI.LMan.GetString("openfileact"),
                                null,
                                "*.monosim");
            
			if (string.IsNullOrEmpty(fileToSave))
            {
            	return "";
            }
			
			
			if (fileToSave.Substring(fileToSave.Length-8) != ".monosim")
			{
				fileToSave += ".monosim";
			}
			
			return fileToSave;
		}
		
		
		
		
		
		
		

		
		/// <summary>
		/// Write contacts on file
		/// </summary>
		private void WriteContactsOnFile(string filePath, List<Contact> contacts)
		{
			
			try
			{
				// save contacts
				StreamWriter sw = new StreamWriter(filePath,false);
				
				foreach(Contact cnt in contacts)
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
				log.Error("MainWindowClass::WriteContactsOnFile: " + Ex.Message + "\r\n" + Ex.StackTrace);
				MainClass.ShowMessage(this, "ERROR", Ex.Message, MainClass.MessageType.Error);
				return;
			}
			
			MainClass.ShowMessage(this, "INFO", GlobalObjUI.LMan.GetString("filesaved"), MainClass.MessageType.Info);
		}
		
		
		
		

		private void SaveContactsFileOnSim()
		{
			// check for contacts description chars len
			string retCheck = GlobalObjUI.CheckAlphaCharsLen(GlobalObjUI.FileContacts);
			if (retCheck != "")
			{
				MainClass.ShowMessage(this, "ERROR",
					GlobalObjUI.LMan.GetString("maxlenexceeded")
					.Replace("'description'", "'<b>" + retCheck + "</b>'")
					+ "<b>" + GlobalObjUI.SimADNMaxAlphaChars.ToString() + "</b>", 
					MainClass.MessageType.Warning);
				
				return;
			}
			
			SelectWriteModeDialogClass swmdc = 
				new SelectWriteModeDialogClass(GlobalObjUI.LMan.GetString("savefilesimact"));
			
			int retMode = swmdc.Show();
			
			if (retMode < 1)
			{
				// cancel button pressed
				return;
			}
			
			log.Debug("MainWindowClass::SaveContactsFileOnSim: SELECTED SIM WRITE MODE: " + retMode.ToString());
			if (retMode == 1)
			{			
				WriteContactsOnSim(GlobalObjUI.FileContacts, true);	
			}
			else
			{
				WriteContactsOnSim(GlobalObjUI.FileContacts, false);	
			}
		}		
		
		
		
		
		

		private void SaveContactsSim()
		{
			// check for contacts description chars len
			string retCheck = GlobalObjUI.CheckAlphaCharsLen(GlobalObjUI.SimContacts);
			if (retCheck != "")
			{
				MainClass.ShowMessage(this, "ERROR",
					GlobalObjUI.LMan.GetString("maxlenexceeded")
					.Replace("'description'", "'<b>" + retCheck + "</b>'")
					+ "<b>" + GlobalObjUI.SimADNMaxAlphaChars.ToString() + "</b>", 
					MainClass.MessageType.Warning);
				
				return;
			}
			
			SelectWriteModeDialogClass swmdc = 
				new SelectWriteModeDialogClass(GlobalObjUI.LMan.GetString("savesimact"));
			
			int retMode = swmdc.Show();
			
			if (retMode < 1)
			{
				// cancel button pressed
				return;
			}
			
			log.Debug("MainWindowClass::SaveContactsSim: SELECTED SIM WRITE MODE: " + retMode.ToString());
			if (retMode == 1)
			{			
				WriteContactsOnSim(GlobalObjUI.SimContacts, true);	
			}
			else
			{
				WriteContactsOnSim(GlobalObjUI.SimContacts, false);	
			}
		}
		
		
		
		
		/// <summary>
		/// Write passed contacts on sim card (append or override)
		/// </summary>
		private void WriteContactsOnSim(Contacts contacts, bool isAppend)
		{
			// check for space on sim
			if (!isAppend && (contacts.SimContacts.Count > GlobalObjUI.SimADNRecordCount))
			{
				// No enough space on sim
				MainClass.ShowMessage(this, "ERROR", 
					GlobalObjUI.LMan.GetString("nosimspace"), MainClass.MessageType.Error);
				return;
			}
			
			if (isAppend && (contacts.SimContacts.Count > (GlobalObjUI.SimADNRecordCount - 
				                                           GlobalObjUI.SimADNRecordNoEmpty)))
			{
				// No enough space on sim
				MainClass.ShowMessage(this, "ERROR", 
					GlobalObjUI.LMan.GetString("nosimspace"), MainClass.MessageType.Error);
				return;
			}
			
			SimUpdate(contacts, isAppend);			
		}
		
		
		
		
		/// <summary>
		/// Start sim update thread
		/// </summary>
		private void SimUpdate(Contacts cnts, bool isAppend)
		{
			ScanSimBefore();
			
			// Reset status values
			GlobalObjUI.SimADNStatus = 1;
			GlobalObjUI.SimADNPosition = 0;
			GlobalObjUI.SimADNError = "";
			
            // Start thread for reading process
			isEnd = false;
			isReading = false;
			System.Threading.ThreadStart threadStart = delegate() {
				GlobalObjUI.WriteSimContactsList(cnts, isAppend);
			};
            simThread = new System.Threading.Thread(threadStart);
            simThread.Start();

		}
		
		
		
		

		
		/// <summary>
		/// Updates during sim contacts writing
		/// </summary>
		private void WritingUpdate()
		{
			PBar.Value = GlobalObjUI.SimADNPosition;
			mainwindow_Ui.StatusBar.ShowMessage(GlobalObjUI.LMan.GetString("writecontact") + 
				              GlobalObjUI.SimADNPosition.ToString("d3"));
			MainClass.QtWait();
			
			
			if (GlobalObjUI.SimADNStatus == 3 && !isEnd)
			{
				// End with errors
				isEnd=true;
				MainClass.ShowMessage(this, "ERROR", GlobalObjUI.SimADNError, MainClass.MessageType.Error);
				SimConnect();
				return;
			}
			
			// check for sim write ended
			if (GlobalObjUI.SimADNStatus == 2 && !isEnd)
			{
				isEnd=true;
				SimConnect();
			}
		}		
		
		
		
		private void SimChangePin()
		{
			// check for Pin1 check attempts
			if (GlobalObjUI.SimPin1Attempts == 1)
			{
				// Pin1 one attempt
				MainClass.ShowMessage(this, GlobalObjUI.LMan.GetString("pinsimact"),
					GlobalObjUI.LMan.GetString("pinsimchk3"), MainClass.MessageType.Warning);
				return;
			}
			else if (GlobalObjUI.SimPin1Attempts == 0)
			{
				// Pin1 no more attempt
				MainClass.ShowMessage(this, GlobalObjUI.LMan.GetString("pinsimact"),
					GlobalObjUI.LMan.GetString("pinsimchk4"), MainClass.MessageType.Warning);
				return;
			}
			
			// Change Pin1 dialog
			ChangePinStatusDialogClass cpsdc = new ChangePinStatusDialogClass(this);
			string pin1 = cpsdc.Show();
			
			if (pin1 == null)
			{
				// cancel button pressed
				return;
			}
			
			// Perform Pin1 status change
			retStr = GlobalObjUI.SetPinStatus(!GlobalObjUI.SimPin1Status, pin1);
			
			if (retStr != "")
			{
				// error detected during Pin1 status change
				MainClass.ShowMessage(this, GlobalObjUI.LMan.GetString("pinsimact"),
					retStr, MainClass.MessageType.Error);
				return;
			}
			
			// Pin1 status changed, reconnect sim now
			MainClass.ShowMessage(this, GlobalObjUI.LMan.GetString("pinsimact"),
					GlobalObjUI.LMan.GetString("pinsimdone"), MainClass.MessageType.Info);
			
			// Force sim disconnect
			SimDisconnect();
		}
		
		
		
		
		private void DeleteContactsSim()
		{
			QMessageBox dmsg = new QMessageBox(QMessageBox.Icon.Question,
				MainClass.AppNameVer + " - " + GlobalObjUI.LMan.GetString("deletesimact"),
				GlobalObjUI.LMan.GetString("suredeletesim"),
				(uint)QMessageBox.StandardButton.Yes | (uint)QMessageBox.StandardButton.No);
			
			int respType = dmsg.Exec();
			
			dmsg.Close();
			dmsg.Dispose();
			
			
			if (respType != (uint)QMessageBox.StandardButton.Yes)
			{
				return;
			}


			// Delete sim
			ScanSimBefore();
			
			// Reset status values
			GlobalObjUI.SimADNStatus = 1;
			GlobalObjUI.SimADNPosition = 0;
			GlobalObjUI.SimADNError = "";
			
            // Start thread for reading process
            isReading = false;
			isEnd = false;
            simThread = new System.Threading.Thread(new System.Threading.ThreadStart(GlobalObjUI.DeleteAllSimContactsList));
            simThread.Start();

			return;
			

		}
		
		
		
		
		#endregion Private Methods
		
		
		
		
	}
}
