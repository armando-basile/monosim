using System;
using Gtk;
	
public class clsInputDialog
{
	
	[Glade.Widget] Gtk.Dialog 	msgDialog;
	[Glade.Widget] Gtk.Label 	lblInfo;
	[Glade.Widget] Gtk.Entry 	txtPIN;
	
	
	public clsInputDialog(Window w, DialogFlags df, ref int theRet, string Title, string Message, ref string PIN)
	{
		Glade.XML rxml = new Glade.XML (null, "inputdialog.glade", "msgDialog", null);
		rxml.Autoconnect (this);
		
		this.msgDialog.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");
		this.msgDialog.TransientFor = w;
		//this.msgDialog.Modal=true;
		this.msgDialog.WidthRequest = 400;
		this.msgDialog.HeightRequest = 180;
		this.msgDialog.Title = Title;
		this.lblInfo.Markup = Message;
		
		theRet = msgDialog.Run();
		PIN = txtPIN.Text;
		this.msgDialog.Destroy();		
		return;		
	}

}
