using System;
using System.Collections.Generic;
using System.IO;
using Glade;
using Gtk;
using Gdk;
using Pango;

using log4net;

using comexbase;
using monosimbase;


namespace monosimgtk
{
	
	
	public partial class MainWindowClass
	{
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		#region Private methods
		
		
		
		
		/// <summary>
		/// Perform change of selected reader
		/// </summary>
		private void UpdateSelectedReader(string newSelReader)
		{
			
		}
		
		
		
		
		
		
		
		private void NewContactsFile()
		{
			lstFileContacts.Clear();
			LblFile.Markup = "<b>" + GlobalObjUI.LMan.GetString("framefile") + "</b>";
			GlobalObjUI.ContactsFilePath = "";
			LstFileContacts.Sensitive = true;
		}
		
		
		
		private void OpenContactsFile()
		{
			GlobalObjUI.ContactsFilePath = "";
			
			// New dialog for select contacts file 
			Gtk.FileChooserDialog FileBox = new Gtk.FileChooserDialog(GlobalObjUI.LMan.GetString("openfileact"), 
			                                MainWindow,
			                                FileChooserAction.Open, 
			                                GlobalObjUI.LMan.GetString("cancellbl"), Gtk.ResponseType.Cancel,
                                            GlobalObjUI.LMan.GetString("openlbl"), Gtk.ResponseType.Accept);
			
			// Filter for using only monosim files
			Gtk.FileFilter myFilter = new Gtk.FileFilter(); 
			myFilter.AddPattern("*.monosim");
			myFilter.Name = "monosim files";
			FileBox.AddFilter(myFilter);
			
			// Manage result of dialog box
			FileBox.Icon = Gdk.Pixbuf.LoadFromResource("monosim.png");
			int retFileBox = FileBox.Run();
			if ((ResponseType)retFileBox == Gtk.ResponseType.Accept)
			{	
				// path of a right file returned
				GlobalObjUI.ContactsFilePath = FileBox.Filename.ToString();
				
				FileBox.Destroy();
				FileBox.Dispose();				
			}
			else
			{
				// nothing returned				
				FileBox.Destroy();
				FileBox.Dispose();
				return;
			}
			
		}
		
		
		private void SaveContactsFile()
		{
			
		}
		
		
		
		
		private void SaveContactsFileOnSim()
		{
			
			
		}		

		
		
		private void CloseContactsFile()
		{
			lstFileContacts.Clear();
			LblFile.Markup = "<b>" + GlobalObjUI.LMan.GetString("framefile") + "</b>";
			GlobalObjUI.ContactsFilePath = "";
			LstFileContacts.Sensitive = false;
		}
		
		
		
		
		private void SimConnect()
		{
			
		}
		
		
		
		private void SimDisconnect()
		{
			
		}
		
		
		private void SimChangePin()
		{
			
		}
		
		
		
		private void SaveContactsSim()
		{
			
		}
		
		
		
		
		
		private void SaveContactsSimOnFile()
		{
			
		}
		
		
		
		
		private void DeleteContactsSim()
		{
			
		}
		
		
		
		
		
		
		
		#endregion Private methods
		
		
	}
	
}
