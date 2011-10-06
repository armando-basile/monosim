using System;
using System.IO;
using System.Xml;

namespace monosimbase
{
	/// <summary>
	/// Manage languages
	/// </summary>
	public class LanguageManager
	{
		
		// Attributes
		XmlDocument xmlDoc;
		
		
		/// <summary>
		/// Costructor
		/// </summary>
		public LanguageManager(string resourceFilePath)
		{
			// Load Resource file
			xmlDoc = new XmlDocument();
			xmlDoc.Load(resourceFilePath);				
			
		}

		
		
		/// <summary>
		/// Get value of requested key
		/// </summary>
		public string GetString(string keyName)
		{
			XmlNode xmlDocNode = xmlDoc.SelectSingleNode("/Settings/language/" + keyName);				
			return xmlDocNode.InnerText;
		}
		
		
		
	}

}
