using System;
using System.Text.RegularExpressions;

namespace Revanche.Views.Objects
{
	/// <summary>
	/// UI component which renders an identifiable's properties. Does this as a flat list using Displays.* objects. Also shows notes in tooltips.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Properties : Gtk.Bin
	{
		private Models.Identifiable model;

		public Properties()
		{
			this.Build();
		}

		public void Initialize(Models.Identifiable model)
		{
			this.model=model;
		}

		public void Refresh()
		{
			foreach(Gtk.Widget child in this.table1.Children){
				child.Destroy();
			}
			string[] properties=model.GetModelInfo().GetPropertyNames();
			this.table1.NRows=(uint)properties.Length;
			this.table1.RowSpacing=Config.PROPERTY_PADDING;
			uint counter=0;
			foreach(string key in properties){
				Gtk.Label label=new Gtk.Label("<b>"+(new Regex("(.)([A-Z])")).Replace(key,"$1 $2")+"</b>");
				label.SetAlignment(0,0.5f);
				label.UseMarkup=true;
				Gtk.Widget widget=label;
				this.table1.Attach(widget,0,1,counter,counter+1);
				(this.table1[widget] as Gtk.Table.TableChild).XOptions=Gtk.AttachOptions.Fill;
				string propertyNote=model.PropertyNote(key);
				if(model.GetTypeInfo(key)!=Types.RevType.GetRegisteredType("COLOR")){
					widget=new Displays.Text(model.Property(key)??"",propertyNote!="");
				} else{
					widget=new Displays.Color(model.Property(key)as string,propertyNote!="");
				}
				widget.TooltipText=propertyNote;
				this.table1.Attach(widget,1,2,counter,counter+1);
				(this.table1[widget] as Gtk.Table.TableChild).XOptions=Gtk.AttachOptions.Expand|Gtk.AttachOptions.Fill;
				counter++;
			}
			this.ShowAll();
		}
	}
}

