

using System;
using System.Xml;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("monosim")]
public class clsSerializableObject
{
	private string _defaultLanguage;
	
	[XmlElement("def_language")]
	public string defaultLanguage
    {
    	get { return _defaultLanguage; }
    	set { _defaultLanguage = value; }
    }
	
}
