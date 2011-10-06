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
		private static string languageFolder = "";
		private static string languageTag = "";
		private static string retStr = "";
		private static LanguageManager lMan = null;
		
		
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
			if (di.GetFiles(envLang + ".xml").Length == 1)
			{
				// language file exists, use it
				languageTag = envLang;
			}
			else
			{
				// language file don't exists, use en-US as default
				languageTag = "en-US";
			}
			
			log.Debug("GlobalObjUI::SetLanguage: LanguagePath=" + languageFolder + Path.DirectorySeparatorChar + languageTag + ".xml");
			lMan = new LanguageManager(languageFolder + Path.DirectorySeparatorChar + languageTag + ".xml");
			
			return;
		}
		
		
		
		
		/// <summary>
		/// Exchange data with smartcard and check response with expected data, 
		/// you can use '?' digit to skip check in a specific position.
		/// </summary>
		public static string SendReceiveAdv(string command, 
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
		
		
		
		
		
		#endregion Public Methods
		
		
		
		
		
		
		
	}
	
	
}