using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Revanche.Views.Dialogs
{
	/// <summary>
	/// Dialog which allows the display and editing of notes.
	/// </summary>
	public partial class Note : Gtk.Dialog
	{
		private Models.Identifiable model;
		private Objects.Identifiable parent;
		private Dictionary<string,ValuedField> fields;
		private ValuedField general;

		public Note(Models.Identifiable model,Objects.Identifiable parent)
		{
			this.model=model;
			this.parent=parent;
			this.fields=new Dictionary<string, ValuedField>();
			this.Build();
			this.Title="Notes about "+parent.Title;
			string[] properties=model.GetModelInfo().GetPropertyNames();
			this.table1.NRows=(uint)properties.Length+2;
			uint counter=0;
			foreach(string key in properties){
				Gtk.Label label=new Gtk.Label("<b>"+(new Regex("(.)([A-Z])")).Replace(key,"$1 $2")+"</b>");
				label.SetAlignment(0,0.5f);
				label.UseMarkup=true;
				this.table1.Attach(label,0,1,counter,counter+1);
				(this.table1[label] as Gtk.Table.TableChild).XOptions=Gtk.AttachOptions.Fill;
				ValuedField widget=new Fields.Text((model.PropertyNote(key)??"").ToString());
				this.table1.Attach(widget as Gtk.Widget,1,2,counter,counter+1);
				(this.table1[widget as Gtk.Widget] as Gtk.Table.TableChild).XOptions=Gtk.AttachOptions.Expand|Gtk.AttachOptions.Fill;
				this.fields[key]=widget;
				counter++;
			}
			Gtk.Label generalLabel=new Gtk.Label("<b>General Notes</b>");
			generalLabel.UseMarkup=true;
			this.table1.Attach(generalLabel,0,2,counter,counter+1);
			counter++;
			this.general=new Fields.Text((model.Notes??"").ToString());
			this.table1.Attach(general as Gtk.Widget,0,2,counter,counter+1);
			counter++;
			this.ShowAll();
		}

		protected void handlerCancel(object sender,EventArgs e)
		{
			this.Destroy();
		}

		protected void handlerOkay(object sender,EventArgs e)
		{
			string[] properties=this.model.GetModelInfo().GetPropertyNames();
			uint counter=0;
			foreach(string key in properties){
				model.SetPropertyNote(key,this.fields[key].Value as string);
				counter++;
			}
			model.Notes=this.general.Value as string;
			this.parent.Refresh();
			this.Destroy();
		}
	}
}

