using System;
using Gtk;
	
public class clsNewItemDlg
{
	
	[Glade.Widget] Gtk.Dialog 			NewItemDlg;
	[Glade.Widget] Gtk.Button			cmdOk;
	[Glade.Widget] Gtk.Button			cmdCancel;
	[Glade.Widget] Gtk.Entry			txtDescription;
	[Glade.Widget] Gtk.Entry			txtNumber;
	
	public clsNewItemDlg(Window w, DialogFlags df, ref string[] dataContact)
	{
		Glade.XML rxml = new Glade.XML (null, "newitemwin.glade", "NewItemDlg", null);
		rxml.Autoconnect (this);
		dataContact = new string[0];
		
		this.NewItemDlg.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");
		this.NewItemDlg.TransientFor = w;
		this.NewItemDlg.Title = "New Contacts";
		
		int theKey = this.NewItemDlg.Run();
		Console.WriteLine("theKey = " + theKey.ToString());
		this.NewItemDlg.Destroy();
		
		if (theKey != 1)
			return;	

		if (txtDescription.Text.Trim() == "" || txtNumber.Text.Trim() == "")
			return;
		
		dataContact = new string[2];
		dataContact[0] = txtDescription.Text;
		dataContact[1] = txtNumber.Text;
		
	}


}
