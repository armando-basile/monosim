using System;
using System.Collections.Generic;
using System.IO;
using Qyoto;

using log4net;

using comexbase;
using monosimbase;

namespace monosimqt
{
	public partial class MainWindowClass
	{
		
		/// <summary>
		/// Add new contact action
		/// </summary>
		private void PopupFileAdd()
		{
			NewContactDialogClass ncdc = new NewContactDialogClass(this, 0, "", "");
			Contact newContact = ncdc.Show();

			if (newContact == null)
			{
				return;
			}
			
			// Add contact to file contacts 
			GlobalObjUI.FileContacts.SimContacts.Add(newContact);
			List<string> contact = new List<string>();
			contact.Add(" ");
			contact.Add(newContact.Description);
			contact.Add(newContact.PhoneNumber);
			new QTreeWidgetItem(mainwindow_Ui.LstFileContacts, contact);	
		}
		
		
		


		
		
		/// <summary>
		/// Add new contact action
		/// </summary>
		private void PopupSimAdd()
		{
			NewContactDialogClass ncdc = new NewContactDialogClass(this,
				GlobalObjUI.SimADNMaxAlphaChars, "", "");
			Contact newContact = ncdc.Show();

			if (newContact == null)
			{
				return;
			}
			
			// Add contact to file contacts 
			GlobalObjUI.SimContacts.SimContacts.Add(newContact);
			List<string> contact = new List<string>();
			contact.Add(" ");
			contact.Add(newContact.Description);
			contact.Add(newContact.PhoneNumber);
			new QTreeWidgetItem(mainwindow_Ui.LstSimContacts, contact);	
		}
		
		

		
		
		
/*
		
		
		
		
		
		
		
		/// <summary>
		/// Del contact
		/// </summary>
		private void PopupFileDel()
		{
			if (LstFileContacts.Selection.CountSelectedRows() == 0)
			{
				// no selected rows
				return;
			}
			
			if (!DeleteContactQuestion())
			{
				return;
			}
			
			// loop for all selected items
			TreePath[] tps = LstFileContacts.Selection.GetSelectedRows();
			Contacts contacts = GlobalObjUI.FileContacts;
			RemoveItems(ref contacts, ref lstFileContacts, tps);
			
		}
		
		
		
		
		
		
		/// <summary>
		/// Del contact
		/// </summary>
		private void PopupSimDel()
		{
			if (mainwindow_Ui.LstSimContacts.SelectedItems().Count == 0)
			{
				// no selected rows
				return;
			}
			
			if (!DeleteContactQuestion())
			{
				return;
			}
			
			// loop for all selected items
			Contacts contacts = GlobalObjUI.SimContacts;
			for (int i=0; i<contacts.SimContacts.Count; i++)
			{

			}
			 
			
			RemoveItems(ref contacts, ref lstSimContacts, tps);
			
		}
		
*/
		
		
		
/*		
		
		
		private void PopupFileMoveToSim()
		{
			if (!LstSimContacts.Sensitive)
			{
				// ListView disabled
				return;
			}
			
			// loop for all selected items
			TreePath[] tps = LstFileContacts.Selection.GetSelectedRows();
			int pos = -1;
			for(int p=0; p<tps.Length; p++)
			{
				pos = tps[p].Indices[0];
				// add contact from list
				GlobalObjUI.SimContacts.SimContacts.Add(GlobalObjUI.FileContacts.SimContacts[pos]);
				lstSimContacts.AppendValues(GlobalObjUI.FileContacts.SimContacts[pos].Description,
					                        GlobalObjUI.FileContacts.SimContacts[pos].PhoneNumber);
			}
		}
		
		
		
		
		
		
		private void PopupSimMoveToFile()
		{
			if (!LstFileContacts.Sensitive)
			{
				// ListView disabled
				return;
			}
			
			// loop for all selected items
			TreePath[] tps = LstSimContacts.Selection.GetSelectedRows();
			int pos = -1;
			for(int p=0; p<tps.Length; p++)
			{
				pos = tps[p].Indices[0];
				// add contact from list
				GlobalObjUI.FileContacts.SimContacts.Add(GlobalObjUI.SimContacts.SimContacts[pos]);
				lstFileContacts.AppendValues(GlobalObjUI.SimContacts.SimContacts[pos].Description,
					                         GlobalObjUI.SimContacts.SimContacts[pos].PhoneNumber);
			}			
			
		}		
		
		
		
*/
		
		/// <summary>
		/// Are you sure dialog
		/// </summary>
		private bool DeleteContactQuestion()
		{
			QMessageBox mdlg = new QMessageBox(QMessageBox.Icon.Question, 
				MainClass.AppNameVer + " - " + GlobalObjUI.LMan.GetString("delcontacts"),
				GlobalObjUI.LMan.GetString("suredelcontact"), 
				(uint)QMessageBox.StandardButton.Yes | 
				(uint)QMessageBox.StandardButton.No);
			
			int respType = mdlg.Exec();
			mdlg.Close();
			mdlg.Dispose();
			mdlg = null;
			
			if (respType == (uint)QMessageBox.StandardButton.Yes)
			{
				return true;
			}
			return false;			
		}
		
		
		
		
		
		

		
		
	}
}

