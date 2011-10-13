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
		
		
		
		public SelectWriteModeDialogClass(Gtk.Window parent)
		{
			Glade.XML gxml =  new Glade.XML("SelectWriteModeDialog.glade", "SelectWriteModeDialog");
            gxml.Autoconnect(this);

			SelectWriteModeDialog.TransientFor = parent;
			SelectWriteModeDialog.DestroyWithParent = true;
			
			SelectWriteModeDialog.Title = MainClass.AppNameVer + " - Work in progress";
			
			string title = "work in progress";			
			
			LblTitle.Markup = title;

			BtnOverride.Image = new Gtk.Image(Pixbuf.LoadFromResource("chip_16.png"));
			BtnAppend.Image = new Gtk.Image(Pixbuf.LoadFromResource("chip_16.png"));
			BtnOverride.Label = "Override.";
			BtnAppend.Label = "Append.";
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

