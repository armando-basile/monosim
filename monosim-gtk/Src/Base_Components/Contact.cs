using System;

namespace monosimbase
{
	public class Contact
	{
		// Properties
		public string Description {get; set;}
		public string PhoneNumber {get; set;}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public Contact (string description, string phoneNumber)
		{
			Description = description;
			PhoneNumber = phoneNumber;
		}
	}
}

