using System;
using System.Text;
using Glade;
using Gtk;
using Gdk;

using comexbase;
using monosimbase;

namespace monosimgtk
{
	
	public class ChangePinStatusDialogClass
	{
		
		[Glade.Widget]  Gtk.Dialog             ChangePinStatusDialog = null;
		[Glade.Widget]  Gtk.Label              LblTitle = null;
		[Glade.Widget]  Gtk.Label              LblPin1 = null;
		[Glade.Widget]  Gtk.Label              LblPin1check = null;
		[Glade.Widget]  Gtk.Entry              TxtPin1 = null;
		[Glade.Widget]  Gtk.Entry              TxtPin1check = null;
		
		// Attributes
		private Gtk.Window mainWin = null;
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		public ChangePinStatusDialogClass (Gtk.Window parent)
		{
			mainWin = parent;
			SetupDialog();
		}
		
		
		
		/// <summary>
		/// Show Dialog to change Pin1 status
		/// </summary>
		public string Show()
		{
			int respType = -1;
			string pin1 = "", pin1check = "";
			int retNumber = 0;
			
			while (1==1)
			{
				SetupDialog();
				respType = ChangePinStatusDialog.Run();
				pin1 = TxtPin1.Text.Trim();
				pin1check = TxtPin1check.Text.Trim();
				ChangePinStatusDialog.Destroy();
				
				if (respType != 0)
				{
					// Cancel button pressed
					return null;
				}
				
				// check data entry
				if (pin1 == "" || pin1check == "")
				{
					// send warning message
					MainClass.ShowMessage(mainWin, "ERROR", 
						                  GlobalObjUI.LMan.GetString("pinsimchk2"),
						                  MessageType.Warning);
				}
				else if (pin1.Trim() != pin1check.Trim())
				{
					// send warning message
					MainClass.ShowMessage(mainWin, "ERROR", 
						                  GlobalObjUI.LMan.GetString("pinsimchk1"),
						                  MessageType.Warning);
				}
				else if (pin1.Trim().Length > 8 || pin1.Trim().Length < 4)
				{
					// send warning message
					MainClass.ShowMessage(mainWin, "ERROR", 
						                  GlobalObjUI.LMan.GetString("pinsimchk2"),
						                  MessageType.Warning);
				}
				else if (!int.TryParse(pin1.Trim(), out retNumber))
				{
					// send warning message
					MainClass.ShowMessage(mainWin, "ERROR", 
						                  GlobalObjUI.LMan.GetString("pinsimchk2"),
						                  MessageType.Warning);
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
			Glade.XML gxml =  new Glade.XML("ChangePinStatusDialog.glade", "ChangePinStatusDialog");
            gxml.Autoconnect(this);
			
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
			
			ChangePinStatusDialog.Icon = Gdk.Pixbuf.LoadFromResource("monosim.png");
			ChangePinStatusDialog.Title = MainClass.AppNameVer + " - " + GlobalObjUI.LMan.GetString("pinsimact");
			LblTitle.Markup = title;
			LblPin1.Text = GlobalObjUI.LMan.GetString("pinsimlbl");
			LblPin1check.Text = GlobalObjUI.LMan.GetString("pinsimchklbl");
			
			ChangePinStatusDialog.TransientFor = mainWin;
			ChangePinStatusDialog.DestroyWithParent = true;
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
            if (pinValue.Length < 8) {
                pinHexValue += new string('F', (8 - pinValue.Length)*2);
            }
			return pinHexValue;
		}
		
		
	}
	
}



