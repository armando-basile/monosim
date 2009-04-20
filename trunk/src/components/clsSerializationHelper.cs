using System;
using System.IO;
using System.Xml.Serialization;


	/// <summary>
	/// <p>Helper class for serialization of [clsSettings] static class </p>
	/// </summary>
	public static class clsSerializationHelper
	{
		
		
	
		/// <summary>
	    /// Provide to see if we're on windows
	    /// </summary>
		public static bool IsWindowsMac()
		{
		    PlatformID platform = Environment.OSVersion.Platform;	    
		    return (platform == PlatformID.Win32NT | platform == PlatformID.Win32Windows |
		            platform == PlatformID.Win32S | platform == PlatformID.WinCE);    
		}
	
	
		/// <summary>
		/// <p>This method load the parameters from a config file 
		/// and update the static class members.</p>
		/// </summary>
		public static void load(string fileXmlName)
		{
			// set Config File Path			
			string myConfigFilePath = "";	
			
			if (IsWindowsMac() == true)
				myConfigFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString();
			else
				myConfigFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar.ToString();
				
			// myConfigFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
			// myConfigFilePath = myConfigFilePath.Replace(System.IO.Path.GetFileName(myConfigFilePath), "");
			
			string fileXmlPath = myConfigFilePath + fileXmlName;
			
			// Create Copy Object
			clsSerializableObject serialClass = new clsSerializableObject();
			
			if (File.Exists(fileXmlPath) == false)
			{	
				
				// Empty Static Class
				clsSettings.defaultLanguage = "english";
				
				// Empty Copied Class
				serialClass.defaultLanguage = "english";
				
				// Save XML file
				save(fileXmlName);
				
			}
			else
			{
				// deserialize the file
				XmlSerializer MySerializer = new XmlSerializer( typeof(clsSerializableObject) );
				
				using (FileStream file = new FileStream(fileXmlPath, FileMode.Open, FileAccess.Read))
	  			{
	    			// Deserializaion...	    			
	    			serialClass = (clsSerializableObject)MySerializer.Deserialize(file);
				}
			}
			
			// Fill static class objects
			clsSettings.defaultLanguage = serialClass.defaultLanguage;
		}

		
		/// <summary>
		/// <p>This method save the static class parameters in a config file.</p>
		/// </summary>
		public static void save(string fileXmlName)
		{
			
			// set Config File Path
			string myConfigFilePath = "";

			if (IsWindowsMac() == true)
				myConfigFilePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar.ToString();
			else
				myConfigFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + Path.DirectorySeparatorChar.ToString();

			// myConfigFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
			// myConfigFilePath = myConfigFilePath.Replace(System.IO.Path.GetFileName(myConfigFilePath), "");
			
			string fileXmlPath = (myConfigFilePath + fileXmlName);
			
			// Create Copy Object
			clsSerializableObject serialClass = new clsSerializableObject();			
			
			// Fill Copied Object
			serialClass.defaultLanguage = clsSettings.defaultLanguage ;
			
			// Write XML file with copied object
			XmlSerializer MySerializer = new XmlSerializer( typeof(clsSerializableObject) );
			using (FileStream file = new FileStream(fileXmlPath, FileMode.Create, FileAccess.Write))
  			{
    			// Serializaion...
    			MySerializer.Serialize(file, serialClass);
			}
			
		}

	}
