using System;
using Gtk;
	
public class clsMsgDialog
{
	
	[Glade.Widget] Gtk.Dialog 	msgDialog;
	[Glade.Widget] Gtk.Label 	lblInfo;
	
	
	public clsMsgDialog(Window w, DialogFlags df, ref int theRet, ref clsLanguageHelper myTranslator)
	{
		Glade.XML rxml = new Glade.XML (null, "msgdialog.glade", "msgDialog", null);
		rxml.Autoconnect (this);
		
		this.msgDialog.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");
		this.msgDialog.TransientFor = w;
		//this.msgDialog.Modal=true;
		this.msgDialog.Title = myTranslator.readTranslatedString(105) ;
		this.lblInfo.Markup = myTranslator.readTranslatedString(104);
		
		theRet = msgDialog.Run();		 
		this.msgDialog.Destroy();		
		return;		
	}

	public clsMsgDialog(Window w, DialogFlags df, ref int theRet, string Title, string Message)
	{
		Glade.XML rxml = new Glade.XML (null, "msgdialog.glade", "msgDialog", null);
		rxml.Autoconnect (this);
		
		this.msgDialog.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");
		this.msgDialog.TransientFor = w;
		//this.msgDialog.Modal=true;
		this.msgDialog.WidthRequest = 400;
		this.msgDialog.HeightRequest = 150;
		this.msgDialog.Title = Title;
		this.lblInfo.Markup = Message;
		
		theRet = msgDialog.Run();		 
		this.msgDialog.Destroy();		
		return;		
	}

}
