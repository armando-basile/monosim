
using System;
using Qyoto;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

using comexbase;
using monosimbase;

using log4net;

namespace monosimqt
{
	
	
	public partial class MainWindowClass: QMainWindow
	{


		// Attributes
		private string retStr = "";
		private string ATR = "";
		private System.Threading.Thread simThread = null;
		private List<string> allReaders = new List<string>();
		
		
		
		
		
		#region Private Methods
		
		
		
		
		
		/// <summary>
		/// Information about
		/// </summary>
		private void OpenInfo()
		{
			AboutDialogClass adc = new AboutDialogClass();
			adc.Show();
			
		}
		
		
		
		
		
		/// <summary>
		/// Show settings dialog
		/// </summary>
		private void OpenSettings()
		{
			SettingsDialogClass sdc = new SettingsDialogClass();
			sdc.Show();
		}
		
		
		
		
		
		
		#endregion Private Methods
		
		
		
		
	}
}
