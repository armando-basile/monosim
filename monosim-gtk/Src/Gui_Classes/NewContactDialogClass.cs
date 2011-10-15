using System;
using Glade;
using Gtk;
using Gdk;

using comexbase;
using monosimbase;

namespace monosimgtk
{
	
	public class NewContactDialogClass
	{
		
		[Glade.Widget]  Gtk.Dialog             NewContactDialog = null;
		[Glade.Widget]  Gtk.Label              LblTitle = null;
		[Glade.Widget]  Gtk.Label              LblDesc = null;
		[Glade.Widget]  Gtk.Label              LblNumber = null;
		[Glade.Widget]  Gtk.Entry              TxtDesc = null;
		[Glade.Widget]  Gtk.Entry              TxtNumber = null;
		//[Glade.Widget]  Gtk.HButtonBox         Buttons = null;
		//[Glade.Widget]  Gtk.Button             BtnCancel = null;
		//[Glade.Widget]  Gtk.Button             BtnOk = null;
		
		// Attributes
		private Gtk.Window mainWin = null;
		private int maxAlphaChars = 0;
		private string txtDesc = "";
		private string txtNumber = "";
		
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		public NewContactDialogClass (Gtk.Window parent, int maxAlphaCharacters,
			string desc, string phone)
		{
			maxAlphaChars = maxAlphaCharacters;
			mainWin = parent;
			txtDesc = desc;
			txtNumber = phone;
			SetupDialog();
		}
		
		
		
		/// <summary>
		/// Show Dialog to add new contact and return it.
		/// </summary>
		public Contact Show()
		{
			int respType = -1;
			string txtDesc = "", txtNumber = "";
			
			while (1==1)
			{
				SetupDialog();
				respType = NewContactDialog.Run();
				txtDesc = TxtDesc.Text.Trim();
				txtNumber = TxtNumber.Text.Trim();
				NewContactDialog.Destroy();
				
				if (respType != 0)
				{
					// Cancel button pressed
					return null;
				}
				
				// check data entry
				if (txtDesc == "" || txtNumber == "")
				{
					// send warning message
					MainClass.ShowMessage(mainWin, "ERROR", 
						                  GlobalObjUI.LMan.GetString("fieldsreq"), 
						                  MessageType.Warning);
				}
				else if ((txtDesc.Length == 21) && 
					     (txtDesc.Substring(0,1) != "+"))
				{
					// number max len is 20 digits
					MainClass.ShowMessage(mainWin, "ERROR", 
						                  GlobalObjUI.LMan.GetString("maxnumlen"), 
						                  MessageType.Warning);


				}
				else
				{
					// Data are correct
					Contact cnt = new Contact(txtDesc, txtNumber);
					return cnt;
				}
			}
				
				
		}

		
		/// <summary>
		/// Setup Dialog
		/// </summary>
		private void SetupDialog()
		{
			Glade.XML gxml =  new Glade.XML("NewContactDialog.glade", "NewContactDialog");
            gxml.Autoconnect(this);
			NewContactDialog.Title = MainClass.AppNameVer + " - " + GlobalObjUI.LMan.GetString("addcontacts");
			LblTitle.Text = GlobalObjUI.LMan.GetString("addcontacts");
			LblDesc.Text = GlobalObjUI.LMan.GetString("descnumber");
			LblNumber.Text = GlobalObjUI.LMan.GetString("phonenumber");
			TxtDesc.MaxLength = maxAlphaChars;
			TxtDesc.Text = txtDesc;
			TxtNumber.Text = txtNumber;
			TxtNumber.MaxLength = 21;
			NewContactDialog.TransientFor = mainWin;
			NewContactDialog.DestroyWithParent = true;
		}
		
		
		
	}
	
}

