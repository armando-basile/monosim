using System;
using Qyoto;

using comexbase;
using monosimbase;

namespace monosimqt
{
	
	
	
	
	
	
	
	
	
	
	/// <summary>
	/// New contact dialog widget
	/// </summary>
	public class NewContactDialogWidget: QDialog
	{
		
		// Attributes
		private Ui.NewContactDialog ncDialog_Ui;
		
		
		// Properties
		public string Desc 
		{
			get { return ncDialog_Ui.TxtDesc.Text; }
			set { ncDialog_Ui.TxtDesc.Text = value; }
		}
		
		public string Number 
		{
			get { return ncDialog_Ui.TxtNumber.Text; }
			set { ncDialog_Ui.TxtNumber.Text = value; }
		}
		
		public int MaxAlphaLen {set { ncDialog_Ui.TxtDesc.MaxLength = value; }}
		
		
		
		[Q_SLOT]
		public void ActionExit(QAbstractButton buttonPressed)
		{
			QDialogButtonBox.StandardButton sBtn = ncDialog_Ui.Buttons.standardButton(buttonPressed);
			
			if (sBtn == QDialogButtonBox.StandardButton.Ok)
			{
				Accept();
			}
			else
			{
				Reject();	
			}
		}
		
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		public NewContactDialogWidget()
		{
			ncDialog_Ui = new Ui.NewContactDialog();
			ncDialog_Ui.SetupUi(this);
			
			ncDialog_Ui.LblDesc.Text = GlobalObjUI.LMan.GetString("descnumber");
			ncDialog_Ui.LblNumber.Text = GlobalObjUI.LMan.GetString("phonenumber");
			this.WindowTitle = MainClass.AppNameVer + " - " + GlobalObjUI.LMan.GetString("phonenumber");
			
			string title = GlobalObjUI.LMan.GetString("addcontacts");
			ncDialog_Ui.LblTitle.Text = title;			
			ncDialog_Ui.TxtNumber.MaxLength = 21;
			
			Connect( ncDialog_Ui.Buttons, SIGNAL("clicked(QAbstractButton*)"), this, SLOT("ActionExit(QAbstractButton*)"));
		}
		
		
	}
	
	
	
	
	
	public class NewContactDialogClass
	{
		
		
		// Attributes
		private QMainWindow mainWin = null;
		private NewContactDialogWidget ncDialogWidget;
		private int maxAlphaChars = 0;
		private string txtDesc = "";
		private string txtNumber = "";
		
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		public NewContactDialogClass (QMainWindow parent, int maxAlphaCharacters,
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
				respType = ncDialogWidget.Exec();
				txtDesc = ncDialogWidget.Desc;
				txtNumber = ncDialogWidget.Number;
				ncDialogWidget.Close();
				ncDialogWidget.Dispose();
				
				if (respType != 0x00000400)
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
						                  MainClass.MessageType.Warning);
				}
				else if ((txtDesc.Length == 21) && 
					     (txtDesc.Substring(0,1) != "+"))
				{
					// number max len is 20 digits
					MainClass.ShowMessage(mainWin, "ERROR", 
						                  GlobalObjUI.LMan.GetString("maxnumlen"), 
						                  MainClass.MessageType.Warning);


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
			ncDialogWidget = new NewContactDialogWidget();
			if (maxAlphaChars > 0)
			{
				ncDialogWidget.MaxAlphaLen = maxAlphaChars;
			}
			
			ncDialogWidget.Desc = txtDesc;
			ncDialogWidget.Number = txtNumber;
		}
		
		
		
	}
	
}

