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
		private int result = -1;
		
		// Properties
		public int ResultSelection { get { return result; }}
		
		
		[Q_SLOT]
		public void ActionExit()
		{
			result = -1;
			Close();
		}

		
		[Q_SLOT]
		public void ActionOverride()
		{
			result = 2;
			Close();
		}
		
		
		[Q_SLOT]
		public void ActionAppend()
		{
			result = 1;
			Close();
		}

		
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
			swDialog_Ui.BtnCancel.Text = GlobalObjUI.LMan.GetString("cancellbl");

			// Configure events reactors
			Connect( swDialog_Ui.BtnCancel, SIGNAL("clicked()"), this, SLOT("ActionExit()"));
			Connect( swDialog_Ui.BtnOverride, SIGNAL("clicked()"), this, SLOT("ActionOverride()"));
			Connect( swDialog_Ui.BtnAppend, SIGNAL("clicked()"), this, SLOT("ActionAppend()"));
		}
		
		
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	public class SelectWriteModeDialogClass
	{
		
		private string wintitle = "";
		private SelectWriteModeDialogWidget swmDialogWidget = null;
		
		public SelectWriteModeDialogClass(string winTitle)
		{
			wintitle = winTitle;
			SetupDialog();
		}
		
		
		
		
		public int Show()
		{
			SetupDialog();
			int respType = -1;
			respType = swmDialogWidget.Exec();
			respType = swmDialogWidget.ResultSelection;
			swmDialogWidget.Close();
			swmDialogWidget.Dispose();
			swmDialogWidget = null;
			
			return respType;
		}
		
		
		
		
		/// <summary>
		/// Setup Dialog
		/// </summary>
		private void SetupDialog()
		{
			swmDialogWidget = new SelectWriteModeDialogWidget();
			swmDialogWidget.WindowTitle = wintitle;
			
		}
		
		
		
	}
}

