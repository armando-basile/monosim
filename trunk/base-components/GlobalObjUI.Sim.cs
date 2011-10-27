using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Reflection;

using log4net;
using comexbase;

namespace monosimbase
{
	
	
	public static partial class GlobalObjUI
	{
		
		// Attributes
		private static string simCommand = "";
		private static string simResponse = "";
		private static string simExpResponse = "";
		private static bool simRespOk = false;

		private static List<string> ADNrecords = new List<string>();
		
		
		
		// Events
		public static event MonosimEventHandler MonosimEvent;
		
		
		#region Public Methods
		
		
		
		
		
		
		/// <summary>
		/// Select sim contacts list.
		/// </summary>
		public static string SelectSimContactsList()
		{
			string retCmd = GetSimPinStatus();
			
			if (retCmd != "")
			{
				// error detected
				return retCmd;
			}
			
			if (GlobalObjUI.SimPin1Status)
			{
				// Pin1 enabled, should be disabled
				return lMan.GetString("needpindisable");
			}
			
			
			retCmd = ReadIccId();
			
			if (retCmd != "")
			{
				// error detected
				return retCmd;
			}
			
			retCmd = ReadADN();
			
			if (retCmd != "")
			{
				// error detected
				return retCmd;
			}			
			
			
			return "";
		}
		
		
		
		
		/// <summary>
		/// Read sim contacts list.
		/// </summary>
		public static void ReadSimContactsList()
		{
			string emptyRecord = new string('F', SimADNRecordLen * 2) + "9000";
			string expRecord = new string('?', SimADNRecordLen * 2) + "9000";
			string retCmd = "";
			
			SimADNRecordNoEmpty = 0;
			ADNrecords = new List<string>();
			SimADNRecordEmptyID = new List<int>();
			SimContacts = new Contacts();
			
			// loop for each ADN records
			for (int l=1; l<=SimADNRecordCount; l++)
			{
				// Set record id
				SimADNPosition = l;
				
				// Prepare sim command
				simCommand = "A0B2" + l.ToString("X2") + "04" + SimADNRecordLen.ToString("X2");
				simExpResponse = expRecord;
				simResponse = "";
				simRespOk = false;
				retCmd = SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
				log.Debug("GlobalObjUI.Sim::ReadSimContactsList: READ ADN REC " + 
					      l.ToString("d3") + " " + simResponse);
				
				if (retCmd != "")
				{
					// error detected
					SimADNError = retCmd;
					SimADNStatus = 3;
					// send notify to gui
					MonosimEvent(new object(), new EventArgs());
					return;
				}
				
				
				if (!simRespOk)
				{
					// wrong response
					SimADNError = "WRONG RESPONSE: [" + simExpResponse + "] - [" + simResponse + "]";
					SimADNStatus = 3;
					// send notify to gui
					MonosimEvent(new object(), new EventArgs());
					return;
				}
				
				
				// check for not empty records
				if (simResponse != emptyRecord)
				{
					// increment counter of not empty records
					SimADNRecordNoEmpty++;
					
					// update records list
					ADNrecords.Add(simResponse.Substring(0, simResponse.Length-4));
				}
				else
				{
					// Add to Empty record list
					SimADNRecordEmptyID.Add(l);
				}
				
				// send notify to gui
				MonosimEvent(new object(), new EventArgs());				
			}
			
			
			SimADNStatus = 2;
			
			// send notify to gui
			MonosimEvent(new object(), new EventArgs());			
			
		}
		
		
		
		
		
		public static void WriteSimContactsList(Contacts contacts, bool isAppend)
		{
			string emptyRecord = new string('F', SimADNRecordLen * 2);
			string expRecord = "9000";
			string retCmd = "";
			string retRecord = "";
			int adnRec = 0;
			
			// loop for each ADN records
			for (int cid=0; cid<contacts.SimContacts.Count; cid++)
			{
				if (!isAppend)
				{
					// first record
					adnRec = cid + 1;
				}
				else
				{
					// first empty record
					adnRec = SimADNRecordEmptyID[cid];
				}
				
				// Set record id
				SimADNPosition = adnRec;
				
				// Prepare sim command
				simCommand = "A0DC" + adnRec.ToString("X2") + "04" + SimADNRecordLen.ToString("X2");
					
				retStr = PrepareRecord(contacts.SimContacts[cid], out retRecord);
				if (retStr != "")
				{
					// error detected
					SimADNError = retStr;
					SimADNStatus = 3;
					// send notify to gui
					MonosimEvent(new object(), new EventArgs());
					return;
				}
				
				simCommand += retRecord;
				simExpResponse = expRecord;
				simResponse = "";
				simRespOk = false;
				retCmd = SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
				
				log.Debug("GlobalObjUI.Sim::WriteSimContactsList: WRITE ADN REC " + 
					      adnRec.ToString("d3") + " " + simCommand + " < " + simResponse);
				
				if (retCmd != "")
				{
					// error detected
					SimADNError = retCmd;
					SimADNStatus = 3;
					// send notify to gui
					MonosimEvent(new object(), new EventArgs());
					return;
				}
				
				
				if (!simRespOk)
				{
					// wrong response
					SimADNError = "WRONG RESPONSE: [" + simExpResponse + "] - [" + simResponse + "]";
					SimADNStatus = 3;

					// send notify to gui
					MonosimEvent(new object(), new EventArgs());
					return;
				}
				
				// send notify to gui
				MonosimEvent(new object(), new EventArgs());
			}
			
			if(!isAppend)
			{
				// delete old other records
				for (int rid=adnRec+1; rid<=GlobalObjUI.SimADNRecordCount; rid++)
				{
					if (!SimADNRecordEmptyID.Contains(rid))
					{
						// Set record id
						SimADNPosition = rid;
						
						simCommand = "A0DC" + rid.ToString("X2") + "04" + SimADNRecordLen.ToString("X2") +
							         emptyRecord;
						simExpResponse = expRecord;
						simResponse = "";
						simRespOk = false;
						retCmd = SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
						
						log.Debug("GlobalObjUI.Sim::WriteSimContactsList: WRITE ADN REC " + 
						      rid.ToString("d3") + " " + simCommand + " < " + simResponse);
					
						if (retCmd != "")
						{
							// error detected
							SimADNError = retCmd;
							SimADNStatus = 3;
							// send notify to gui
							MonosimEvent(new object(), new EventArgs());
							return;
						}
						
						
						if (!simRespOk)
						{
							// wrong response
							SimADNError = "WRONG RESPONSE: [" + simExpResponse + "] - [" + simResponse + "]";
							SimADNStatus = 3;
		
							// send notify to gui
							MonosimEvent(new object(), new EventArgs());
							return;
						}
						
						// send notify to gui
						MonosimEvent(new object(), new EventArgs());
						
					}
				}
			}
			
			SimADNStatus = 2;
			
			// send notify to gui
			MonosimEvent(new object(), new EventArgs());	
			
		}
		
		
		
		/// <summary>
		/// Delete all sim contacts list
		/// </summary>
		public static void DeleteAllSimContactsList()
		{
			string emptyRecord = new string('F', SimADNRecordLen * 2);
			string expRecord = "9000";
			string retCmd = "";
			
			for (int rid=1; rid<=GlobalObjUI.SimADNRecordCount; rid++)
			{
				// if record isn't empty
				if (!SimADNRecordEmptyID.Contains(rid))
				{
					// Set record id
					SimADNPosition = rid;
					
					simCommand = "A0DC" + rid.ToString("X2") + "04" + SimADNRecordLen.ToString("X2") +
						         emptyRecord;
					simExpResponse = expRecord;
					simResponse = "";
					simRespOk = false;
					retCmd = SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
					
					log.Debug("GlobalObjUI.Sim::DeleteAllSimContactsList: WRITE ADN REC " + 
					      rid.ToString("d3") + " " + simCommand + " < " + simResponse);
				
					if (retCmd != "")
					{
						// error detected
						SimADNError = retCmd;
						SimADNStatus = 3;
						// send notify to gui
						MonosimEvent(new object(), new EventArgs());
						return;
					}
					
					
					if (!simRespOk)
					{
						// wrong response
						SimADNError = "WRONG RESPONSE: [" + simExpResponse + "] - [" + simResponse + "]";
						SimADNStatus = 3;
	
						// send notify to gui
						MonosimEvent(new object(), new EventArgs());
						return;
					}
					
					// send notify to gui
					MonosimEvent(new object(), new EventArgs());
				}
			}
			
			SimADNStatus = 2;
			
			// send notify to gui
			MonosimEvent(new object(), new EventArgs());
			
		}
		
		
		
		
		/// <summary>
		/// Enable/Disable Pin1 on sim
		/// </summary>
		public static string SetPinStatus(bool newStatus, string pin1Value)
		{
			string retSetPinStatus = "";
			
			// Prepare sim command
			simCommand = "A0";
			
			if (newStatus)
			{
				// set to enabled
				simCommand += "28";
			}
			else
			{
				// set to disables
				simCommand += "26";
			}
			
			simCommand += "000108" + pin1Value;
			simExpResponse = "9000";
			simResponse = "";
			simRespOk = false;
			retStr = SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
			log.Debug("GlobalObjUI.Sim::SetPinStatus: " + simResponse);
			
			if (retStr != "")
			{
				// error detected
				return retStr;
			}
			
			
			if (!simRespOk)
			{
				// wrong response
				retSetPinStatus = "WRONG RESPONSE: [" + simExpResponse + "] - [" + simResponse + "]\r\n";
			}
			
			// read again Pin1 status and attempts
			retStr = GetSimPinStatus();
			if (retStr != "")
			{
				log.Debug("GlobalObjUI.Sim::SetPinStatus: GetSimPinStatus: " + retStr);
				retSetPinStatus += "WRONG MF SCAN: " + retStr;
			}
			
			return retSetPinStatus;
		}
		
		
		
		
		
		#endregion Public Methods
		
		
		
		
		
		
		
		
		
		
		#region Private Methods
		
		
		
		
		/// <summary>
		/// Exchange data with smartcard and check response with expected data, 
		/// you can use '?' digit to skip check in a specific position.
		/// </summary>
		private static string SendReceiveAdv(string command, 
			                            ref string response, 
			                                string expResponse, 
			                            ref bool isVerified)
		{
			isVerified = false;
			response = "";
			
			// exchange data
			retStr = GlobalObj.SendReceive(command, ref response);
			
			if (retStr != "")
			{
				// error detected
				return retStr;
			}
			
			if (response.Length != expResponse.Length)
			{
				// two length are differents
				return "";
			}
			
			// loop for each digits
			for (int p=0; p<response.Length; p++)
			{
				if ((expResponse.Substring(p,1) != "?") &&
					(expResponse.Substring(p,1) != response.Substring(p,1)))
				{
					// data returned is different from expected
					return "";
				}
			}
			
			isVerified = true;
			return "";
			
		}
		
		
		
		
		/// <summary>
		/// Extract IccID value from bytes of 2FE2 file
		/// </summary>
		private static void ExtractIccID(string fileBytes)
		{
			// discart unused bytes (status words)
			fileBytes = fileBytes.Substring(0,20);
			
			retStr = "";
			
			for (int d=0; d<fileBytes.Length; d+=2)
			{
				retStr += fileBytes.Substring(d+1,1) + fileBytes.Substring(d,1);
			}
			
			SimIccID = retStr.Replace("F", "");
			
			return;
		}
		
		
		
		
		
		
		
		/// <summary>
		/// Get sim pin1 status (enabled=true or disabled=false)
		/// </summary>
		private static string GetSimPinStatus()
		{
			// Select Master File
			simCommand = "A0A40000023F00";
			simExpResponse = "9F??";
			string retCmd = SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
			log.Debug("GlobalObjUI::GetSimPinStatus: SELECT MF " + simResponse);
			
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
			retCmd = SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
			log.Debug("GlobalObjUI::GetSimPinStatus: GET RESPONSE " + simResponse);
			
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
				SimPin1Status = true;
				log.Debug("GlobalObjUI::GetSimPinStatus: PIN1 ENABLED");
			}
			else
			{
				// Disabled
				SimPin1Status = false;
				log.Debug("GlobalObjUI::GetSimPinStatus: PIN1 DISABLED");
			}
			
			// Get remaining attempts
			SimPin1Attempts = Convert.ToInt32(simResponse.Substring(37,1), 16);
			log.Debug("GlobalObjUI::GetSimPinStatus: PIN1 VERIFY ATTEMPTS: " + SimPin1Attempts.ToString());
			
			return "";
			
		}
		
		
		
		
		
		/// <summary>
		/// Read IccID file (2FE2) and extract value
		/// </summary>
		private static string ReadIccId()
		{
			// Select 2FE2 (ICCID)
			simCommand = "A0A40000022FE2";
			simExpResponse = "9F??";
			string retCmd = SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
			log.Debug("GlobalObjUI::ReadIccId: SELECT ICCID " + simResponse);
			
			if (retCmd != "")
			{	
				return retCmd ;
			}
			
			if (!simRespOk)
			{	
				return "WRONG RESPONSE [" + simExpResponse + "] " + "[" + simResponse + "]";
			}
			
			
			// Read 2FE2 (ICCID)
			simCommand = "A0B000000A";
			simExpResponse = new string('?', 20) + "9000";
			simRespOk = false;
			retCmd = SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
			log.Debug("GlobalObjUI::ReadIccId: READ ICCID " + simResponse);
			
			if (retCmd != "")
			{	
				return retCmd ;
			}
			
			if (!simRespOk)
			{	
				return "WRONG RESPONSE [" + simExpResponse + "] " + "[" + simResponse + "]";
			}
			
			// obtain IccID from reader bytes
			ExtractIccID(simResponse);
			
			log.Debug("GlobalObjUI::ReadIccId: READ ICCID SWAPPED " + SimIccID);
			
			return "";
		}
		
		
		
		
		/// <summary>
		/// Select ADN file on sim and extract main info
		/// </summary>
		private static string ReadADN()
		{
			// Select 7F10 (DF TELECOM)
			simCommand = "A0A40000027F10";
			simExpResponse = "9F??";
			string retCmd = SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
			log.Debug("GlobalObjUI::ReadADN: SELECT DF TELECOM " + simResponse);
			
			if (retCmd != "")
			{	
				return retCmd ;
			}
			
			if (!simRespOk)
			{	
				return "WRONG RESPONSE [" + simExpResponse + "] " + "[" + simResponse + "]";
			}
			
			
			// Select 6F3A (ADN)
			simCommand = "A0A40000026F3A";
			simExpResponse = "9F??";
			retCmd = SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
			log.Debug("GlobalObjUI::ReadADN: SELECT ADN " + simResponse);
			
			if (retCmd != "")
			{	
				return retCmd ;
			}
			
			if (!simRespOk)
			{	
				return "WRONG RESPONSE [" + simExpResponse + "] " + "[" + simResponse + "]";
			}			
			
			
			
			// Get Response 6F3A (ADN)
			simCommand = "A0C00000" + simResponse.Substring(2,2);
			simExpResponse = new string('?', Convert.ToInt32(simResponse.Substring(2,2), 16) * 2) + "9000";
			retCmd = SendReceiveAdv(simCommand, ref simResponse, simExpResponse, ref simRespOk);
			log.Debug("GlobalObjUI::ReadADN: GET RESPONSE " + simResponse);
			
			if (retCmd != "")
			{	
				return retCmd ;
			}
			
			if (!simRespOk)
			{	
				return "WRONG RESPONSE [" + simExpResponse + "] " + "[" + simResponse + "]";
			}	
			
			
			// Update ADN values
			SimADNRecordLen = Convert.ToInt32(simResponse.Substring(28, 2), 16);
        	SimADNFileLen = Convert.ToInt32(simResponse.Substring(4, 4), 16);
			SimADNRecordCount = GlobalObjUI.SimADNFileLen / GlobalObjUI.SimADNRecordLen;
        	SimADNMaxAlphaChars = GlobalObjUI.SimADNRecordLen - 14;
        	
			log.Debug("GlobalObjUI::ReadADN: File len ...... : " + SimADNFileLen.ToString());
			log.Debug("GlobalObjUI::ReadADN: Record len .... : " + SimADNRecordLen.ToString());
			log.Debug("GlobalObjUI::ReadADN: Record count .. : " + SimADNRecordCount.ToString());
			log.Debug("GlobalObjUI::ReadADN: Max Alpha chars : " + SimADNMaxAlphaChars.ToString());
			
			
			return "";
		}
		
		
		
		
		
		
		#endregion Private Methods
		
		
		
		
		
	}
}
