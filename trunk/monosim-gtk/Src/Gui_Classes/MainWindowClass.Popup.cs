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
		
		/// <summary>
		/// Add new contact action
		/// </summary>
		private void PopupFileAdd()
		{
			NewContactDialogClass ncdc = new NewContactDialogClass(MainWindow, 0, "", "");
			Contact newContact = ncdc.Show();

			if (newContact == null)
			{
				return;
			}
			
			// Add contact to file contacts 
			GlobalObjUI.FileContacts.SimContacts.Add(newContact);
			lstFileContacts.AppendValues(newContact.Description, newContact.PhoneNumber);
		}
		
		
		
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
		/// Add new contact action
		/// </summary>
		private void PopupSimAdd()
		{
			NewContactDialogClass ncdc = new NewContactDialogClass(MainWindow, 
				GlobalObjUI.SimADNMaxAlphaChars,"", "");
			Contact newContact = ncdc.Show();

			if (newContact == null)
			{
				return;
			}
			
			// Add contact to file contacts 
			GlobalObjUI.SimContacts.SimContacts.Add(newContact);
			lstSimContacts.AppendValues(newContact.Description, newContact.PhoneNumber);
		}
		
		
		
		
		
		
		
		
		
		/// <summary>
		/// Del contact
		/// </summary>
		private void PopupSimDel()
		{
			if (LstSimContacts.Selection.CountSelectedRows() == 0)
			{
				// no selected rows
				return;
			}
			
			if (!DeleteContactQuestion())
			{
				return;
			}
			
			// loop for all selected items
			TreePath[] tps = LstSimContacts.Selection.GetSelectedRows();
			Contacts contacts = GlobalObjUI.SimContacts;
			RemoveItems(ref contacts, ref lstSimContacts, tps);
			
		}
		
		
		
		
		
		
		
		
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
		
		
		
		
		
		/// <summary>
		/// Are you sure dialog
		/// </summary>
		private bool DeleteContactQuestion()
		{
			MessageDialog mdlg = new MessageDialog(MainWindow, DialogFlags.Modal, 
				MessageType.Question, ButtonsType.YesNo, GlobalObjUI.LMan.GetString("suredelcontact"));
			mdlg.TransientFor = MainWindow;
			mdlg.Title = MainClass.AppNameVer + " - " + GlobalObjUI.LMan.GetString("delcontacts");

			ResponseType respType = (ResponseType)mdlg.Run();
			mdlg.Destroy();
			mdlg.Dispose();
			mdlg = null;
			
			if (respType == ResponseType.Yes)
			{
				return true;
			}
			return false;			
		}
		
		
		
		
		
		
		
		
		private void RemoveItems(ref Contacts cnts, ref ListStore lstore, TreePath[] tPath)
		{
			
			for(int p=(tPath.Length-1); p>=0; p--)
			{
				// remove contact from list
				cnts.SimContacts.RemoveAt(tPath[p].Indices[0]);
			}
			
			for(int p=(tPath.Length-1); p>=0; p--)
			{
				// remove contact from list
				TreeIter ti;
				bool isIter = lstore.GetIter(out ti, tPath[p]);
				if (isIter)
				{
					lstore.Remove(ref ti);
				}
			}
		}
		
		
		
		
	}
}

