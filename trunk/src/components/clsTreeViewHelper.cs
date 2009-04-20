
using System;

public class clsTreeViewHelper
{
	
	public void TreeViewInit(ref Gtk.TreeView tvObj, 
	                         ref Gtk.ListStore lsObj, 
	                         ref string[] tvColumns)
	{
		
		Gtk.TreeViewColumn tvColumn = null;
		Gtk.CellRendererText theCell = null;
		
		System.Type[] lsParam = new System.Type[tvColumns.Length];
		
		// Add Columns to TreeView
		for (int j=0; j<tvColumns.Length; j++)
		{
			tvColumn = new Gtk.TreeViewColumn();		
			tvColumn.MinWidth = 150 ;
			tvColumn.Title = tvColumns[j];
			tvColumn.Resizable = true;
			
			theCell = new Gtk.CellRendererText();
			tvColumn.PackStart(theCell, true);
			tvColumn.AddAttribute(theCell, "text", j);
			
			tvObj.RulesHint = true;
			tvObj.AppendColumn(tvColumn);
			lsParam[j] = typeof(string);
		}
		
		
		Gtk.ListStore lsTmp = new Gtk.ListStore(lsParam);
		lsObj = lsTmp;
		tvObj.Model = lsObj;
		
		for (int j=0; j<tvColumns.Length; j++)
		{
			
			
		}
		
		
		
	}

}

