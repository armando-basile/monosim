/*
 * Creato da SharpDevelop.
 * Utente: hman
 * Data: 28/06/2007
 * Ora: 11.29
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;

	/// <summary>
	/// Description of clsLanguageHelper.
	/// </summary>
	public class clsLanguageHelper
	{
		private string rootLangFolder = "";
		Dictionary<int, string> languageKeys = null;
		
		public clsLanguageHelper(string languageFolder)
		{
			// Set folder path of languages files 
			rootLangFolder = languageFolder;
		}
		
		public string[] getLanguagesName()
		{
			return Directory.GetFiles(rootLangFolder);			
		}

		public void readLanguageFile(string theLanguage)
		{
			string theLine = "";
			string languageFileName = rootLangFolder + Path.DirectorySeparatorChar + theLanguage;
			languageKeys = new Dictionary<int, string>();
			StreamReader sr = new StreamReader(languageFileName);
			theLine = sr.ReadLine();			
			
			while (theLine != null)
			{
				theLine = theLine.Trim();
				if (theLine != "")
					if (theLine.Substring(0,1) != "#")
						languageKeys.Add( int.Parse(theLine.Substring(0,6)), theLine.Substring(7) );						
				
				theLine = sr.ReadLine();
			}
			
			sr.Close();
			sr.Dispose();
			sr=null;
						
			return;
		}
		
		public string readTranslatedString(int stringCode)
		{
			if (languageKeys.ContainsKey(stringCode) == false)
				return "";
			
			return languageKeys[stringCode];
		}
		
		
	}

