
using System;
using Qyoto;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using comexbase;

using log4net;

namespace monosimqt
{
	
	
	public class SettingsDialogClass: QDialog
	{
		
		// ATTRIBUTES
		Ui.SettingsDialog settingsdialog_UI;
		
		// Log4Net object
        // private static readonly ILog log = LogManager.GetLogger(typeof(SettingsDialogClass));
		
		
		// CONSTRUCTOR
		public SettingsDialogClass()
		{
			
			// Create new settings object
			settingsdialog_UI = new Ui.SettingsDialog();
			
			// Configure layout of this new QDialog with 
			// settingsdialog_UI objects and data			
			settingsdialog_UI.SetupUi(this);
			
			// Update Qt graphic objects
			UpdateGraphicObjects();

			
			// Update reactors (eventhandler)
			UpdateReactors();			
			
			
		}
		
		
		
		
		
		private void UpdateGraphicObjects()
		{
			
			// fill graphic objects informations
			this.SetVisible(false);
			this.WindowTitle = GlobalObj.LMan.GetString("frmset");
			settingsdialog_UI.FrameSettings.Title = "";
			
			
			settingsdialog_UI.Buttons.Clear();
			settingsdialog_UI.Buttons.AddButton(QDialogButtonBox.StandardButton.Ok);
			settingsdialog_UI.Buttons.AddButton(QDialogButtonBox.StandardButton.Cancel);
			
			int index = 0;
			
			// PortSpeed
			index = settingsdialog_UI.CmbPortSpeed.FindText(SerialSettings.PortSpeed.ToString());
			if (index >= 0)
			{
				settingsdialog_UI.CmbPortSpeed.SetCurrentIndex(index);
			}
			
			// PortSpeedReset
			index = settingsdialog_UI.CmbPortSpeedReset.FindText(SerialSettings.PortSpeedReset.ToString());
			if (index >= 0)
			{
				settingsdialog_UI.CmbPortSpeedReset.SetCurrentIndex(index);
			}
			
			// DataBits
			index = settingsdialog_UI.CmbDataBits.FindText(SerialSettings.DataBits.ToString());
			if (index >= 0)
			{
				settingsdialog_UI.CmbDataBits.SetCurrentIndex(index);
			}
			
			// StopBits
			index = settingsdialog_UI.CmbStopBits.FindText(SerialSettings.StopBits.ToString());
			if (index >= 0)
			{
				settingsdialog_UI.CmbStopBits.SetCurrentIndex(index);
			}
			
			// Parity
			index = settingsdialog_UI.CmbParity.FindText(SerialSettings.Parity.ToString());
			if (index >= 0)
			{
				settingsdialog_UI.CmbParity.SetCurrentIndex(index);
			}
			
			// Convention
			if (SerialSettings.IsDirectConvention)
			{
				// Direct
				settingsdialog_UI.CmbConvention.SetCurrentIndex(0);
			}
			else
			{
				// Inverse
				settingsdialog_UI.CmbConvention.SetCurrentIndex(1);
			}
			
		}
		
		
		
		
		private void UpdateReactors()
		{
			// Configure events reactors
			Connect( settingsdialog_UI.Buttons, SIGNAL("clicked(QAbstractButton*)"), this, SLOT("ActionExit(QAbstractButton*)"));			
			
		}
		
		
		
		
		#region Q_SLOTS
		
		[Q_SLOT]
		public void ActionExit(QAbstractButton buttonPressed)
		{
			if (buttonPressed == settingsdialog_UI.Buttons.Buttons()[0])
			{
				// Update settings
				UpdateSettings();
			}
				
			this.Close();			
		}
		
		
		
		
		
		
		
		#endregion Q_SLOTS
		
		
		
		
		
		
		
		/// <summary>
		/// Update serial port settings
		/// </summary>
		private void UpdateSettings()
		{
			SerialSettings.PortSpeedReset = int.Parse(settingsdialog_UI.CmbPortSpeedReset.CurrentText);
			SerialSettings.PortSpeed = int.Parse(settingsdialog_UI.CmbPortSpeed.CurrentText);
			SerialSettings.DataBits = int.Parse(settingsdialog_UI.CmbDataBits.CurrentText);
			SerialSettings.StopBits = int.Parse(settingsdialog_UI.CmbStopBits.CurrentText);
			SerialSettings.Parity = settingsdialog_UI.CmbParity.CurrentText;
			

			if (settingsdialog_UI.CmbConvention.CurrentText == "Direct")
			{
				SerialSettings.IsDirectConvention = true;
			}
			else
			{
				SerialSettings.IsDirectConvention = false;
			}
			
			GlobalObj.WriteSettings();
		}
		
		
		
		
		
		
		
	}
}


 
