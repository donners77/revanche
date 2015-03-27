using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Revanche.Views.Dialogs
{
	/// <summary>
	/// Dialog which edits an Identifiable's properties and displays all Fields.* widgets
	/// </summary>
	public partial class Edit : Gtk.Dialog
	{
		private Models.Identifiable model;
		private Objects.Identifiable parent;
		private Dictionary<string,ValuedField> fields;

		public Edit(Models.Identifiable model,Objects.Identifiable parent)
		{
			this.model=model;
			this.parent=parent;
			this.fields=new Dictionary<string, ValuedField>();
			this.Build();
			this.Title="Edit "+parent.Title;
			string[] properties=model.GetModelInfo().GetPropertyNames();

			bool reparentable=model.GetModelInfo().Parent!=null&&Array.IndexOf(
				Models.RevModel.GetRegisteredModel(model.GetModelInfo().Parent).Collection,
				model.GetModelInfo().Id
			)>=0;

			this.table1.NRows=(uint)(properties.Length+(reparentable?1:0));
			uint counter=0;

			string lastKey=null;

			if(reparentable){
				Gtk.Label label=new Gtk.Label("<b>Parent</b>");
				label.SetAlignment(0,0.5f);
				label.UseMarkup=true;
				this.table1.Attach(label,0,1,counter,counter+1);
				(this.table1[label] as Gtk.Table.TableChild).XOptions=Gtk.AttachOptions.Fill;
				List<Models.Identifiable> options=new List<Models.Identifiable>(Models.Identifiable.GetAllOfModelType(model.GetModelInfo().Parent));
				options.Reverse();
				options.Add(null);
				options.Reverse();
				ValuedField widget=new Fields.Select(options.ToArray(),model.Parent,typeof(Models.Identifiable));
				this.table1.Attach(widget as Gtk.Widget,1,2,counter,counter+1);
				(this.table1[widget as Gtk.Widget] as Gtk.Table.TableChild).XOptions=Gtk.AttachOptions.Expand|Gtk.AttachOptions.Fill;
				// YAML loaded data will never have spaces in the keys, hence the spaces here
				fields.Add("_ Parent _",widget);
				lastKey="_ Parent _";
				counter++;
			}
			foreach(string key in properties){
				Gtk.Label label=new Gtk.Label("<b>"+(new Regex("(.)([A-Z])")).Replace(key,"$1 $2")+"</b>");
				label.SetAlignment(0,0.5f);
				label.UseMarkup=true;
				this.table1.Attach(label,0,1,counter,counter+1);
				(this.table1[label] as Gtk.Table.TableChild).XOptions=Gtk.AttachOptions.Fill;
				ValuedField widget;
				if(model.GetTypeInfo(key).Id=="COLOR"){
					widget=new Fields.Color(model.Property(key) as string);
				} else if(model.GetTypeInfo(key).Id=="BOOLEAN"){
					widget=new Fields.Toggle((model.Property(key) as bool?)??false);
				} else if(model.GetTypeInfo(key).ContentType==Revanche.Types.Content.ENUMERATED){
					widget=new Fields.Select(model.GetTypeInfo(key).Values,(model.Property(key)??"").ToString(),typeof(string));
				} else if(model.GetTypeInfo(key).Id=="DECIMAL"){
					widget=new Fields.Decimal((model.Property(key) as double?)??0.0);
				} else if(model.GetTypeInfo(key).Id=="INTEGER"){
					widget=new Fields.Integer((model.Property(key) as int?)??0);
				} else if(model.GetTypeInfo(key).Id=="TIMESTAMP"){
					widget=new Fields.Date((model.Property(key) as long?)??0L);
				} else{
					// safest if we don't know the right one
					widget=new Fields.Text((model.Property(key)??"").ToString());
				}
				string lastKeyLocal=lastKey;
				if(lastKeyLocal!=null){
					this.fields[lastKeyLocal].Tabbed+=(object sender, EventArgs e) => {
						widget.Focus();
					};
					widget.BackTabbed+=(object sender, EventArgs e) => {
						this.fields[lastKeyLocal].Focus();
					};
				}
				this.table1.Attach(widget as Gtk.Widget,1,2,counter,counter+1);
				(this.table1[widget as Gtk.Widget] as Gtk.Table.TableChild).XOptions=Gtk.AttachOptions.Expand|Gtk.AttachOptions.Fill;
				this.fields[key]=widget;
				lastKey=key;
				counter++;
			}
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
				model.SetProperty(key,this.fields[key].Value);
				counter++;
			}

			if(this.fields.ContainsKey("_ Parent _")&&this.model.Parent!=this.fields["_ Parent _"].Value as Models.Identifiable){
				this.model.Reparent(this.fields["_ Parent _"].Value as Models.Identifiable);
			}

			this.parent.Refresh();
			this.Destroy();
		}
	}
}

