using System;
using Gtk;
	
public class clsReadersDlg
{
	
	[Glade.Widget] Gtk.Dialog 			readersDlg;
	[Glade.Widget] Gtk.Label			lblInfo;
	[Glade.Widget] Gtk.Button			cmdOk;
	[Glade.Widget] Gtk.Button			cmdCancel;
	[Glade.Widget] Gtk.ComboBox			cmbReaders;
	[Glade.Widget] Gtk.VBox				vbox1;
	
	string tmpReader = "";
	
	public clsReadersDlg(Window w, DialogFlags df, ref string[] ourReaders,  ref string readerName, ref clsLanguageHelper myTranslator)
	{
		Glade.XML rxml = new Glade.XML (null, "readers.glade", "readersDlg", null);
		rxml.Autoconnect (this);

		this.readersDlg.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");
		this.readersDlg.TransientFor = w;
		this.readersDlg.Title = myTranslator.readTranslatedString(124);
		readerName = "0";
		lblInfo.Markup = myTranslator.readTranslatedString(123);
        //myTranslator
        
		Gtk.ComboBox newCmb = ComboBox.NewText();
		
		for (int n=0; n<ourReaders.Length; n++)
			newCmb.AppendText(ourReaders[n]);
		
		vbox1.PackStart(newCmb, false, true, 4);
		
		newCmb.Active = 0;
		newCmb.Show();
		
		int theKey = this.readersDlg.Run();
		Console.WriteLine("theKey = " + theKey.ToString());
		this.readersDlg.Destroy();
		
		if (theKey == 1)
			readerName = newCmb.ActiveText;

	}
	
	
	
	public void on_cmdOk_button_press_event(object sender, ButtonPressEventArgs a)
	{

	}

}
