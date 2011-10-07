using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

using log4net;
using comexbase;

namespace monosimbase
{
	
	
	public static partial class GlobalObjUI
	{
		
		// Attributes
		private static string languageFolder = "";
		private static string languageTag = "";
		private static string retStr = "";
		private static LanguageManager lMan = null;
		
		private static int lenAlpha = 0;
		private static string alphaID = "";
    	private static string numLength = "";
    	private static string tonNpi = "";
    	private static string dialNum = "";
    	// private static string configId = "";
    	// private static string ext1Rec = "";    	
		private static int tonNpiNumber = 0;
		
		
		// Log4Net object
        private static readonly ILog log = LogManager.GetLogger(typeof(GlobalObjUI));
		
		
		/// <summary>
		/// Application folder path
		/// </summary>
		public static string AppPath { get; set; }
		
		
		
		
		
		#region Properties
		
				
		/// <summary>
		/// Return language manager object
		/// </summary>
		public static LanguageManager LMan { get { return lMan; } }
		
		
		
		public static string ContactsFilePath {get; set;}
		public static Contacts SimContacts {get; set;}
		public static Contacts FileContacts {get; set;}
		
		public static string SimIccID {get; set;}
		public static string SimADNError {get; set;}
		public static int SimADNStatus {get; set;}
		public static int SimADNPosition {get; set;}
		public static int SimADNRecordLen {get; set;}
		public static int SimADNFileLen {get; set;}
		public static int SimADNRecordCount {get; set;}
		public static int SimADNRecordNoEmpty {get; set;}
		public static int SimADNMaxAlphaChars {get; set;}
		
		
		#endregion Properties
		
		
		
		
		
		
		
		
		
		
		
		#region Public Methods
		
		
		

		/// <summary>
		/// Set language to use
		/// </summary>
		public static void SetLanguage()
		{
			// set application folder path
			string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;                        
			AppPath = new System.IO.FileInfo(dllPath).DirectoryName;
			
			string envLang = System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag;
			languageFolder = AppPath + Path.DirectorySeparatorChar + "Languages";
			
			// check for language folder
			if (!Directory.Exists(languageFolder))
			{
				// use share folder to search languages
				languageFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
					             Path.DirectorySeparatorChar + Assembly.GetExecutingAssembly().GetName().Name +
						         Path.DirectorySeparatorChar + "Languages";
				
				if (!Directory.Exists(languageFolder))
				{
					// no languages founded
					throw new Exception("no language folder founded... ");
				}
			}
			
			// check for language file
			DirectoryInfo di = new DirectoryInfo(languageFolder);
			if (di.GetFiles("monosim-" + envLang + ".xml").Length == 1)
			{
				// language file exists, use it
				languageTag = envLang;
			}
			else
			{
				// language file don't exists, use en-US as default
				languageTag = "en-US";
			}
			
			log.Debug("GlobalObjUI::SetLanguage: LanguagePath=" + languageFolder + Path.DirectorySeparatorChar + "monosim-" + languageTag + ".xml");
			lMan = new LanguageManager(languageFolder + Path.DirectorySeparatorChar + "monosim-" + languageTag + ".xml");
			
			return;
		}
		
		
		
		
		
		/// <summary>
		/// Extract contacts from ADN records
		/// </summary>
		public static string FromRecordsToContacts()
		{
			for (int r=0; r<ADNrecords.Count; r++)
			{
				// try to extract contact info
				retStr = DecodeSimADNRecord(ADNrecords[r]);
				
				if (retStr != "")
				{
					// error detected
					return retStr;
				}
			}
			
			return "";
		}
		
		
		
		
		
		
		
		
		
		
		
		#endregion Public Methods
		
		
		
		
		
		
		
		
		
		
		
		
		#region Private Methods
		
		
		/// <summary>
		/// Decode record to obtain contact infos
		/// </summary>
		private static string DecodeSimADNRecord(string recordValue)
		{
			try
			{
				lenAlpha = (recordValue.Length-28)/2;
				alphaID = recordValue.Substring(0, (lenAlpha * 2));
	    		numLength = recordValue.Substring((lenAlpha * 2), 2);
	    		tonNpi = recordValue.Substring((lenAlpha * 2) + 2, 2);
	    		dialNum = recordValue.Substring((lenAlpha * 2) + 4, 20);
	    		// configId = recordValue.Substring((lenAlpha * 2) + 24, 2);
	    		// ext1Rec = recordValue.Substring((lenAlpha * 2) + 26, 2);    	
				tonNpiNumber = Convert.ToInt32(tonNpi,16);
				
				alphaID = AsciiFromHex(alphaID);
				alphaID = alphaID.Replace(((char)255).ToString() , "");
		    	dialNum = SwapNumber(dialNum, Convert.ToInt32(numLength, 16) * 2); 
				
				if ((tonNpiNumber&16) > 0)
				{
					// international
					dialNum = "+" + dialNum;
				}
				
				log.Debug("GlobalObjUI::DecodeSimADNRecord: Contact = " + alphaID + " - " + dialNum);
				
			}
			catch (Exception Ex)
			{
				log.Error("GlobalObjUI::DecodeSimADNRecord: " + Ex.Message + "\r\n" + Ex.StackTrace);
				return Ex.Message;
			}
			
			// add phone number and description for new founded contact
			SimContacts.SimContacts.Add(new Contact(alphaID, dialNum));
			
			return "";
		}
		
		
		
		
		
		
		/// <summary>
		/// Get ASCII string from hexadecimal string
		/// </summary>
		private static string AsciiFromHex(string hexValue)
		{
			List<byte> bArray = new List<byte>();
			for (int b=0; b<hexValue.Length; b+=2)
			{
				// check for no empty byte
				if (hexValue.Substring(b,2) != "FF")
				{
					bArray.Add(byte.Parse(hexValue.Substring(b,2), System.Globalization.NumberStyles.AllowHexSpecifier));
				}
			}
			
			return UTF8Encoding.UTF8.GetString(bArray.ToArray());			
		}
		
		
		
		
		
		
		/// <summary>
		/// Swap phone number digits
		/// </summary>
		private static string SwapNumber(string inNumber, int numDigits)
		{
			string outNumber = "";
			
			// loop for each byte
			for (int k=0; k<numDigits - 2; k +=2)
			{
				// swap digits in byte
				outNumber += inNumber.Substring(k+1,1) + inNumber.Substring(k,1);
			}
			
			outNumber = outNumber.Replace("A", "*");
			outNumber = outNumber.Replace("B", "#");
			outNumber = outNumber.Replace("F", "");
			
			return outNumber;
		}
		
		
		
		
		
		
		
		
		
		
		
		
		#endregion Private Methods
		
		
		
		
		
		
	}
	
	
}