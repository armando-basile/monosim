using System;
using System.Text;
using Qyoto;

using comexbase;
using monosimbase;

namespace monosimqt
{
	
	
	
	

	
	/// <summary>
	/// Select write mode dialog widget
	/// </summary>
	public class SelectWriteModeDialogWidget: QDialog
	{
		
		// Attributes
		private Ui.SelectWriteModeDialog swDialog_Ui;
		
		
		
		/// <summary>
		/// Constructor
		/// </summary>
		public SelectWriteModeDialogWidget()
		{
			string title = GlobalObjUI.LMan.GetString("simwritemode");
			title = title.Replace("&nbsp;", " ");
			swDialog_Ui = new Ui.SelectWriteModeDialog();
			swDialog_Ui.SetupUi(this);
			swDialog_Ui.LblTitle.Text = title;
			swDialog_Ui.BtnOverride.Text = GlobalObjUI.LMan.GetString("simoverride");
            swDialog_Ui.BtnAppend.Text = GlobalObjUI.LMan.GetString("simappend");
			swDialog_Ui.BtnBoxCancel.AddButton(swDialog_Ui.BtnAppend, QDialogButtonBox.ButtonRole.NoRole);
			swDialog_Ui.BtnBoxCancel.AddButton(swDialog_Ui.BtnOverride, QDialogButtonBox.ButtonRole.YesRole);


		}
		
		
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	public class SelectWriteModeDialogClass
	{
		
		
		private string wintitle = "";
		private QMainWindow mainWin = null;
		private SelectWriteModeDialogWidget swmDialogWidget = null;
		
		public SelectWriteModeDialogClass(QMainWindow parent, string winTitle)
		{
			wintitle = winTitle;
			mainWin = parent;
			SetupDialog();
		}
		
		
		
		
		public int Show()
		{
			SetupDialog();
			int respType = -1;
			respType = swmDialogWidget.Exec();
			swmDialogWidget.Close();
			swmDialogWidget.Dispose();
			swmDialogWidget = null;
			
			Console.WriteLine("ret: " + respType.ToString("X16"));
			
			return -1;
		}
		
		
		
		
		/// <summary>
		/// Setup Dialog
		/// </summary>
		private void SetupDialog()
		{
			swmDialogWidget = new SelectWriteModeDialogWidget();
			swmDialogWidget.SetParent(mainWin);
			swmDialogWidget.WindowTitle = wintitle;

			
		}
		
		
		
	}
}

