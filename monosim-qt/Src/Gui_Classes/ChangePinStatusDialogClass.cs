using System;
using System.Text;
using Qyoto;

using comexbase;
using monosimbase;

namespace monosimqt
{
	
	/// <summary>
	/// Change pin status dialog widget
	/// </summary>
	public class ChangePinStatusDialogWidget: QDialog
	{
		
		// Attributes
		private Ui.ChangePinStatusDialog cpsDialog_Ui;
		
		
		// Properties
		public string Pin1 {get { return cpsDialog_Ui.TxtPin1.Text; }}
		public string Pin1check {get { return cpsDialog_Ui.TxtPin1check.Text; }}
		
		
		[Q_SLOT]
		public void ActionExit(QAbstractButton buttonPressed)
		{
			QDialogButtonBox.StandardButton sBtn = cpsDialog_Ui.Buttons.standardButton(buttonPressed);
			
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
		public ChangePinStatusDialogWidget()
		{
			cpsDialog_Ui = new Ui.ChangePinStatusDialog();
			cpsDialog_Ui.SetupUi(this);
			
			cpsDialog_Ui.LblPin1.Text = GlobalObjUI.LMan.GetString("pinsimlbl");
			cpsDialog_Ui.LblPin1check.Text = GlobalObjUI.LMan.GetString("pinsimchklbl");
			this.WindowTitle = MainClass.AppNameVer + " - " + GlobalObjUI.LMan.GetString("pinsimact");
			
			string title = GlobalObjUI.LMan.GetString("pinsimsetlbl");
			
			// check for actual Pin1 status
			if (GlobalObjUI.SimPin1Status)
			{
				// set disables
				title += "<b>" + GlobalObjUI.LMan.GetString("pin1off") + "</b>";
			}
			else
			{
				// set enabled
				title += "<b>" + GlobalObjUI.LMan.GetString("pin1on") + "</b>";
			}
			
			cpsDialog_Ui.LblTitle.Text = title;
			Connect( cpsDialog_Ui.Buttons, SIGNAL("clicked(QAbstractButton*)"), this, SLOT("ActionExit(QAbstractButton*)"));
		}
		
		
	}
	
	
	
	
	
	
	
	public class ChangePinStatusDialogClass
	{
		
		// Attributes
		private ChangePinStatusDialogWidget cpsDialogWidget;
		private QMainWindow mainWin = null;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		public ChangePinStatusDialogClass (QMainWindow topWin)
		{
			mainWin = topWin;
			SetupDialog();
		}
		
		
		
		/// <summary>
		/// Show Dialog to change Pin1 status
		/// </summary>
		public string Show()
		{
			int respType = 0;
			string pin1 = "", pin1check = "";
			int retNumber = 0;
			
			while (1==1)
			{
				SetupDialog();
				respType = cpsDialogWidget.Exec();
				
				pin1 = cpsDialogWidget.Pin1;
				pin1check = cpsDialogWidget.Pin1check;
				cpsDialogWidget.Close();
				cpsDialogWidget.Dispose();
				
				if (respType != 0x01)
				{
					// Cancel button pressed
					return null;
				}
				
				// check data entry
				if (pin1.Trim() == "" || pin1check.Trim() == "")
				{
					// send warning message
					MainClass.ShowMessage(mainWin, "ERROR", 
						                  GlobalObjUI.LMan.GetString("pinsimchk2"),
						                  MainClass.MessageType.Warning);
				}
				else if (pin1.Trim() != pin1check.Trim())
				{
					// send warning message
					MainClass.ShowMessage(mainWin, "ERROR", 
						                  GlobalObjUI.LMan.GetString("pinsimchk1"),
						                  MainClass.MessageType.Warning);
				}
				else if (pin1.Trim().Length != 4)
				{
					// send warning message
					MainClass.ShowMessage(mainWin, "ERROR", 
						                  GlobalObjUI.LMan.GetString("pinsimchk2"),
						                  MainClass.MessageType.Warning);
				}
				else if (!int.TryParse(pin1.Trim(), out retNumber))
				{
					// send warning message
					MainClass.ShowMessage(mainWin, "ERROR", 
						                  GlobalObjUI.LMan.GetString("pinsimchk2"),
						                  MainClass.MessageType.Warning);
				}
				else
				{
					// Data are correct					
					return GetHexFromPin(pin1);
				}

				
			}
				
				
		}

		
		/// <summary>
		/// Setup Dialog
		/// </summary>
		private void SetupDialog()
		{
			cpsDialogWidget = new ChangePinStatusDialogWidget();

		}
		
		
		
		
		/// <summary>
		/// Get hexadecimal value of passed pin.
		/// </summary>
		private string GetHexFromPin(string pinValue)
		{
			string pinHexValue = "";
			
			for (int d=0; d<pinValue.Length; d++)
			{
				// from int to hexadecimal value on 2 digits

				pinHexValue += UTF8Encoding.UTF8.GetBytes(pinValue.Substring(d,1))[0].ToString("X2");
			}
			
			// padd with 4 bytes to 0xFF
			pinHexValue += new string('F', 8);
			
			return pinHexValue;
		}
		
		
	}
	
}



