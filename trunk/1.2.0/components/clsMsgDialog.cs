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


}
