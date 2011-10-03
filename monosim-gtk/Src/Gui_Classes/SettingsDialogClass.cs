using System;
using Glade;
using Gtk;
using Gdk;

using comexbase;

namespace monosimgtk
{
	
	public class SettingsDialogClass
	{
		
		
        [Glade.Widget]  Gtk.Window             	SettingsDialog = null;
        [Glade.Widget]  Gtk.ComboBox            CmbPortSpeed = null;
        [Glade.Widget]  Gtk.ComboBox            CmbPortSpeedReset = null;
		[Glade.Widget]  Gtk.ComboBox            CmbStopBits = null;
		[Glade.Widget]  Gtk.ComboBox            CmbParity = null;
        [Glade.Widget]  Gtk.ComboBox           	CmbDataBits = null;
        [Glade.Widget]  Gtk.ComboBox            CmbConvention = null;
        [Glade.Widget]  Gtk.Button           	BtnCancel = null;
        [Glade.Widget]  Gtk.Button           	BtnOk = null;

		
		
		/// <summary>
		/// Constructor
		/// </summary>
		public SettingsDialogClass ()
		{
			// Instance glade xml object using glade file
            Glade.XML gxml =  new Glade.XML("SettingsDialog.glade", "SettingsDialog");
            
            // Aonnect glade xml object to this Gtk.Dialog
            gxml.Autoconnect(this);
            
            // Update Gtk graphic objects
            UpdateGraphicObjects();
            
            // Update Event Handlers
            UpdateReactors();          
		}
		
		
		
		
		
		
		#region Public Methods
		
		
		/// <summary>
        /// Set Parend gtk widget
        /// </summary>
        public void SetParent(ref Gtk.Window parent)
        {
        	SettingsDialog.TransientFor = parent;
			SettingsDialog.DestroyWithParent = true;
        }
		
		
		
		
		
		/// <summary>
		/// Show GtkWindow object
		/// </summary>
		public void Show()
		{
			SettingsDialog.Show();
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		#endregion Public Methods
		
		
		
		
		
		
		
		
		
		
		#region Reactors
		
		
		/// <summary>
		/// Close GtkWindows
		/// </summary>
		public void ActionCancel(object sender, EventArgs args)
		{
			SettingsDialog.Destroy();
            SettingsDialog.Dispose();
		}
		
		
		
		/// <summary>
		/// Update settings
		/// </summary>
		public void ActionOk(object sender, EventArgs args)
		{
			UpdateSettings();
			
			SettingsDialog.Destroy();
            SettingsDialog.Dispose();
		}
		
		
		
		#endregion Reactors

		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		#region Private Methods
		
		
		
		/// <summary>
		/// Update Gtk objects properties
		/// </summary>
		private void UpdateGraphicObjects()
		{
			// Set dialog icon
            SettingsDialog.Icon = Gdk.Pixbuf.LoadFromResource("monosim.png");
            SettingsDialog.Title = GlobalObj.LMan.GetString("frmset");

			
			// PortSpeed
			SetItem(ref CmbPortSpeed, SerialSettings.PortSpeed.ToString());
			SetItem(ref CmbPortSpeedReset, SerialSettings.PortSpeedReset.ToString());
			SetItem(ref CmbDataBits, SerialSettings.DataBits.ToString());
			SetItem(ref CmbStopBits, SerialSettings.StopBits.ToString());
			SetItem(ref CmbParity, SerialSettings.Parity);
			
			// Convention
			if (SerialSettings.IsDirectConvention)
			{
				// Direct
				SetItem(ref CmbConvention, "Direct");
			}
			else
			{
				// Inverse
				SetItem(ref CmbConvention, "Inverse");
			}
			
			
		}
		
		
		
		
		
		
		/// <summary>
		/// Update Gtk objects reactors
		/// </summary>
		private void UpdateReactors()
		{
			SettingsDialog.DeleteEvent += ActionCancel;
			BtnCancel.Clicked += ActionCancel;
			BtnOk.Clicked += ActionOk;

		}
		
		
		
		
		
		
		private void SetItem(ref ComboBox combo, string itemValue)
		{
			Gtk.TreeIter iter;
        	bool existsItem = combo.Model.GetIterFirst (out iter);
			combo.SetActiveIter(iter);
			
			while (existsItem)
			{
				// check for item
				if ( (string)combo.Model.GetValue(iter, 0) == itemValue)
				{
					// select item
					combo.SetActiveIter(iter);
					break;
				}
				
				existsItem = combo.Model.IterNext (ref iter);
			}
		}
		
		
		
		
		
		

		/// <summary>
		/// Update serial port settings
		/// </summary>
		private void UpdateSettings()
		{
			SerialSettings.PortSpeedReset = int.Parse(CmbPortSpeedReset.ActiveText);
			SerialSettings.PortSpeed = int.Parse(CmbPortSpeed.ActiveText);
			SerialSettings.DataBits = int.Parse(CmbDataBits.ActiveText);
			SerialSettings.StopBits = int.Parse(CmbStopBits.ActiveText);
			SerialSettings.Parity = CmbParity.ActiveText;
			

			if (CmbConvention.ActiveText == "Direct")
			{
				SerialSettings.IsDirectConvention = true;
			}
			else
			{
				SerialSettings.IsDirectConvention = false;
			}
			
			GlobalObj.WriteSettings();
		}
		
		
		
		
		
		
		#endregion Private Methods
		
		
		
		
		
		
	}
}

 
 
