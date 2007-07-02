using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;

	/// <summary>
	/// <p>Helper class for serialization of [clsSettings] static class </p>
	/// </summary>
	public static class clsSerializationHelper
	{
		
		
		/// <summary>
		/// <p>This method load the parameters from a config file 
		/// and update the static class members.</p>
		/// </summary>
		public static void load(string fileXmlName)
		{
			// set Config File Path			
			string myConfigFilePath = "";			
			myConfigFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
			myConfigFilePath = myConfigFilePath.Replace(System.IO.Path.GetFileName(myConfigFilePath), "");
			
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
				SoapFormatter formatter = new SoapFormatter();
				using (FileStream file = new FileStream(fileXmlPath, FileMode.Open, FileAccess.Read))
	  			{
	    			// Deserializaion...	    			
	    			serialClass = formatter.Deserialize(file) as clsSerializableObject;
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
			myConfigFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString();
			myConfigFilePath = myConfigFilePath.Replace(System.IO.Path.GetFileName(myConfigFilePath), "");
			
			string fileXmlPath = (myConfigFilePath + fileXmlName);
			
			// Create Copy Object
			clsSerializableObject serialClass = new clsSerializableObject();			
			
			// Fill Copied Object
			serialClass.defaultLanguage = clsSettings.defaultLanguage ;
			
			// Write XML file with copied object
			SoapFormatter formatter = new SoapFormatter();
			using (FileStream file = new FileStream(fileXmlPath, FileMode.Create, FileAccess.Write))
  			{
    			// Serializaion...
    			formatter.Serialize(file, serialClass);
			}
			
		}

	}
