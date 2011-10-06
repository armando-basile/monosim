using System;
using System.Collections.Generic;

namespace monosimbase
{
	public class Contacts
	{
		// Properties
		public List<Contact> SimContacts {get; set;}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public Contacts ()
		{
			SimContacts = new List<Contact>();
		}
		
		
		
		public bool Contains(string contactDescription)
		{
			if (FindIndex(contactDescription) >= 0)
			{
				return true;
			}
			return false;
		}
		
		
		
		public int FindIndex(string contactDescription)
		{
			for (int cnt=0; cnt<SimContacts.Count; cnt++ )
			{
				if (SimContacts[cnt].Description == contactDescription)
				{
					return cnt;
				}
			}
			return -1;
		}
		
		
		
		
		
		
	}
}

