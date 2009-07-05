using System;
using Gtk;
using System.Reflection;
	
public class clsInfoDialog
{
	
	[Glade.Widget] Gtk.Dialog 		infoWindow;
	[Glade.Widget] Gtk.Button	 	logoApp;
	[Glade.Widget] Gtk.Image 		monologo;
	[Glade.Widget] Gtk.Label    	lblName;
	[Glade.Widget] Gtk.Label    	lblInfo;	
	[Glade.Widget] Gtk.TextView		txtInfo;
	[Glade.Widget] Gtk.Button   	cmdExit;
	[Glade.Widget] Gtk.Button	 	linkmono;
	
	
	
	public clsInfoDialog(Window w, DialogFlags df, ref clsLanguageHelper myTranslator)
	{
		Glade.XML rxml = new Glade.XML (null, "infowin.glade", "infoWindow", null);
		rxml.Autoconnect (this);
		
		this.infoWindow.Icon = Gdk.Pixbuf.LoadFromResource("monosim_16.png");
		Image monoL = new Image(Gdk.Pixbuf.LoadFromResource("monologo.png"));
		Image simL = new Image(Gdk.Pixbuf.LoadFromResource("monosim_48.png"));
		linkmono.Image = monoL;
		
		logoApp.Image = simL;
		
		this.infoWindow.TransientFor = w;
		//this.infoWindow.Modal=true;
		this.infoWindow.Title = myTranslator.readTranslatedString(13);
		this.lblName.Markup = "<b><big><big>" + Assembly.GetExecutingAssembly().GetName().Name.ToString() + "</big></big></b>   " + 
		                      Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + 
	                  		  "." + Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + 
	                          "." + Assembly.GetExecutingAssembly().GetName().Version.Build.ToString();
		
		//cmdExit.Label = myTranslator.readTranslatedString(36);
		lblInfo.Markup = "realized by hman (hmandevteam@gmail.com)" + Environment.NewLine + 
		                 "to manage sim card contacts using standard" + Environment.NewLine + 
		                 "pcsc smartcard reader. ";
		infoWindow.Run();		 
		infoWindow.Destroy();		
		return;		
	}


	public void On_Linkmono_Press(object sender, EventArgs a)
	{
		System.Diagnostics.Process.Start("http://www.mono-project.com");
	}

	public void On_Logoapp_Press(object sender, EventArgs a)
	{
		System.Diagnostics.Process.Start("http://www.integrazioneweb.com/monosim");
	}

}
