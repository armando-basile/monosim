using System;
using Glade;
using Gtk;
using Gdk;

using comexbase;

namespace comexgtk
{
	
	public class AboutDialogClass
	{
		
		
        [Glade.Widget]  Gtk.Window             AboutDialog = null;
        [Glade.Widget]  Gtk.Image              imgLogo = null;
        [Glade.Widget]  Gtk.Label              lblTitle = null;
		[Glade.Widget]  Gtk.Label              lblAbout = null;
		[Glade.Widget]  Gtk.Label              lblThanks = null;
        [Glade.Widget]  Gtk.Notebook           tabInfo = null;
        [Glade.Widget]  Gtk.Button             btnOk = null;
        [Glade.Widget]  Gtk.Viewport           vpTitle = null;
        [Glade.Widget]  Gtk.Viewport           vpLogo = null;

		
		
		
		
		
		
		
		#region Properties
		
		
		/// <summary>
		/// GtkWindow Title
		/// </summary>
		public string Title
		{
			get {	return AboutDialog.Title; 	}
			set {	AboutDialog.Title = value; 	}
		}
		
		
		
		/// <summary>
		/// Description area (use markup format)
		/// </summary>
		public string Description
		{
			set {	lblTitle.Markup = value; 	}
		}
		
		
		
		/// <summary>
		/// About area (use markup format)
		/// </summary>
		public string About
		{
			set {	lblAbout.Markup = value; 	}
		}
		
		
		
		
		/// <summary>
		/// Thanks area (use markup format)
		/// </summary>
		public string Thanks
		{
			set {	lblThanks.Markup = value; 	}
		}
		
		
		
		
		#endregion Properties
		
		
		
		
		
		
		
		
		
		/// <summary>
        /// Constructor
        /// </summary>
        public AboutDialogClass()
        {                
            // Instance glade xml object using glade file
            Glade.XML gxml =  new Glade.XML("AboutDialog.glade", "AboutDialog");
            
            // Aonnect glade xml object to this Gtk.Dialog
            gxml.Autoconnect(this);
            
            // Update Gtk graphic objects
            UpdateGraphicObjects();
            
            // Update Event Handlers
            UpdateReactors();                       
                       
        }

		
		
		
		
		
		
		
		
		
		#region Public Methods
		
		
		
		
		
		/// <summary>
        /// Set Parend gtk widget
        /// </summary>
        public void SetParent(ref Gtk.Window parent)
        {
        	AboutDialog.TransientFor = parent;
			AboutDialog.DestroyWithParent = true;
        }
		
		
		
		
		
		/// <summary>
		/// Show GtkWindow object
		/// </summary>
		public void Show()
		{
			tabInfo.ShowAll();
			AboutDialog.Show();
		}
		
		
		
		
		
		
		#endregion Public Methods
		
		
		
		
		
		
		
		
		
		
		
		
		
		#region Reactors
		
		
		/// <summary>
		/// Close GtkWindows
		/// </summary>
		public void ActionCancel(object sender, EventArgs args)
		{
			AboutDialog.Destroy();
            AboutDialog.Dispose();
		}
		
		
		
		#endregion Reactors

		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		#region Private Methods
		
		
		
		/// <summary>
		/// Update Gtk objects properties
		/// </summary>
		private void UpdateGraphicObjects()
		{
			// Set dialog icon
            AboutDialog.Icon = Gdk.Pixbuf.LoadFromResource("monosim.png");
            
			// Set description area back color
			vpTitle.ModifyBg(StateType.Normal, new Gdk.Color(255,255,255));
            
			// Create and add tab for About info
            AddLabelTab(ref lblAbout, GlobalObj.LMan.GetString("about"));
            
			// Create and add tab for Thanks info
            AddLabelTab(ref lblThanks, GlobalObj.LMan.GetString("thanks"));
            
            imgLogo.Pixbuf = Gdk.Pixbuf.LoadFromResource("monosim.png");
            vpLogo.ModifyBg(StateType.Normal, new Gdk.Color(255,255,255));
            
            lblTitle.ModifyBg(StateType.Normal, new Gdk.Color(255,255,255));
            
            //Gdk.Geometry geo = new Gdk.Geometry();
            //geo.MinHeight = 380;
            //geo.MinWidth = 500;
            //AboutDialog.SetGeometryHints(tabInfo, geo, Gdk.WindowHints.MinSize);

		}
		
		
		
		
		
		
		/// <summary>
		/// Update Gtk objects reactors
		/// </summary>
		private void UpdateReactors()
		{
			AboutDialog.DeleteEvent += ActionCancel;
			btnOk.Clicked += ActionCancel;

		}
		
		
		
		
		
		
		
		
		/// <summary>
		/// Add tab to notebook object
		/// </summary>
		private void AddLabelTab(ref Gtk.Label lblObj, string title)
		{
			// Create and add tab for Thanks info
            ScrolledWindow sw = new ScrolledWindow();
            Viewport vp = new Viewport();            
			sw.AddWithViewport(vp);
            lblObj = new Label();
            lblObj.SetPadding(4,4);
            lblObj.SetAlignment((float)0, (float)0);            
            vp.ModifyBg(StateType.Normal, new Gdk.Color(255,255,255));            
            vp.Add(lblObj);            
            tabInfo.AppendPage(sw, new Gtk.Label(title));
		}
		
		
		
		
		
		
		
		
		
		
		
		
		
		#endregion Private Methods
		
		
		
		
		
		
		
		
		
		
		
		
		
		
		
	}
}

