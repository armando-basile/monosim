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
		
		

		
		
		
		
		
		
		/// <summary>
		/// Del contact
		/// </summary>
		private void PopupFileDel()
		{
			if (mainwindow_Ui.LstFileContacts.SelectedItems().Count == 0)
			{
				// no selected rows
				return;
			}
			
			if (!DeleteContactQuestion())
			{
				return;
			}
			
			// loop for all selected items
			List<QTreeWidgetItem> selContacts = mainwindow_Ui.LstFileContacts.SelectedItems();

			int posw = -1;
			for (int qwidx=selContacts.Count-1; qwidx>=0; qwidx--)
			{
				posw = mainwindow_Ui.LstFileContacts.IndexOfTopLevelItem(selContacts[qwidx]);				
				mainwindow_Ui.LstFileContacts.TakeTopLevelItem(posw);
				GlobalObjUI.FileContacts.SimContacts.RemoveAt(posw);
			}
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
			List<QTreeWidgetItem> selContacts = mainwindow_Ui.LstSimContacts.SelectedItems();

			int posw = -1;
			for (int qwidx=selContacts.Count-1; qwidx>=0; qwidx--)
			{
				posw = mainwindow_Ui.LstSimContacts.IndexOfTopLevelItem(selContacts[qwidx]);				
				mainwindow_Ui.LstSimContacts.TakeTopLevelItem(posw);
				GlobalObjUI.SimContacts.SimContacts.RemoveAt(posw);
			}
		}
		

		
		
		

		
		
		private void PopupFileMoveToSim()
		{
			// loop for all selected items			
			List<QTreeWidgetItem> selContacts = mainwindow_Ui.LstFileContacts.SelectedItems();
			
			List<string> cntValues = new List<string>();
			foreach (QTreeWidgetItem fromw in selContacts)
			{
				cntValues = new List<string>();
				cntValues.Add(" ");
				cntValues.Add(fromw.Text(1));
				cntValues.Add(fromw.Text(2));
				new QTreeWidgetItem(mainwindow_Ui.LstSimContacts, cntValues);
			}
			
			foreach (QTreeWidgetItem qtwi in selContacts)
			{
				GlobalObjUI.SimContacts.SimContacts.Add(new Contact(qtwi.Text(1), qtwi.Text(2)));
			}			
		}
		
		
		
		
		
		
		private void PopupSimMoveToFile()
		{
			// loop for all selected items			
			List<QTreeWidgetItem> selContacts = mainwindow_Ui.LstSimContacts.SelectedItems();
			
			List<string> cntValues = new List<string>();
			foreach (QTreeWidgetItem fromw in selContacts)
			{
				cntValues = new List<string>();
				cntValues.Add(" ");
				cntValues.Add(fromw.Text(1));
				cntValues.Add(fromw.Text(2));
				new QTreeWidgetItem(mainwindow_Ui.LstFileContacts, cntValues);
			}
			
			foreach (QTreeWidgetItem qtwi in selContacts)
			{
				GlobalObjUI.FileContacts.SimContacts.Add(new Contact(qtwi.Text(1), qtwi.Text(2)));
			}
		}		
		
		
		

		
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

