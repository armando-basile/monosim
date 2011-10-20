using System;
using System.Text;
using Glade;
using Gtk;
using Gdk;

using comexbase;
using monosimbase;

namespace monosimgtk
{
	public class SelectWriteModeDialogClass
	{
		[Glade.Widget]  Gtk.Dialog             SelectWriteModeDialog = null;
		[Glade.Widget]  Gtk.Label              LblTitle = null;
		[Glade.Widget]  Gtk.Button             BtnOverride = null;
		[Glade.Widget]  Gtk.Button             BtnAppend = null;
		
		
		
		public SelectWriteModeDialogClass(Gtk.Window parent, string winTitle)
		{
			Glade.XML gxml =  new Glade.XML("SelectWriteModeDialog.glade", "SelectWriteModeDialog");
            gxml.Autoconnect(this);
			
			SelectWriteModeDialog.Icon = Gdk.Pixbuf.LoadFromResource("monosim.png");
			SelectWriteModeDialog.TransientFor = parent;
			SelectWriteModeDialog.DestroyWithParent = true;
			
			SelectWriteModeDialog.Title = MainClass.AppNameVer + " - " + winTitle;
			
			string title = GlobalObjUI.LMan.GetString("simwritemode");
			title = title.Replace("<br>", "").Replace("&nbsp;", " ");
			
			LblTitle.Markup = title;

			BtnOverride.Image = new Gtk.Image(Stock.Remove, IconSize.Button);
			BtnAppend.Image = new Gtk.Image(Stock.Add, IconSize.Button);
			BtnOverride.Label = GlobalObjUI.LMan.GetString("simoverride");
			BtnAppend.Label = GlobalObjUI.LMan.GetString("simappend");
		}
		
		
		
		
		public int Show()
		{
			int respType = -1;
			respType = SelectWriteModeDialog.Run();
			SelectWriteModeDialog.Destroy();
			return respType;
		}
		
		
		
		
		
	}
}

