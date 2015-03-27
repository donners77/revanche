using System;
using System.Collections.Generic;

namespace Revanche.Views.Dialogs
{
	/// <summary>
	/// Dialog which displays the COLOR type palette.
	/// </summary>
	public partial class Color : Gtk.Window
	{
		private string color;

		public string Selected
		{
			get{
				return this.color;
			}
		}

		public Color(string current):base(Gtk.WindowType.Toplevel)
		{
			this.TypeHint=Gdk.WindowTypeHint.Dialog;
			this.color=current;
			this.Build();
			List<string> colors=new List<string>(Types.RevType.GetRegisteredType("COLOR").NamedValues.Keys);

			Gtk.HBox columns=new Gtk.HBox();

			object[] rowsizes=Types.RevType.GetRegisteredType("COLOR").Values;
			int rowcounter=0;
			int counter=0;
			foreach(string name in colors){
				if(counter==Int32.Parse(rowsizes[rowcounter] as string)){
					this.rows.PackStart(columns,true,true,0);
					columns=new Gtk.HBox();
					counter=0;
					rowcounter++;
				}
				int hex=this.getRgbColor(name);
				Gtk.Image i=new Gtk.Image();
				Gdk.Pixbuf p=new Gdk.Pixbuf(Gdk.Colorspace.Rgb,true,8,32,32);
				p.Fill((((uint)hex)<<8)|0xffu);
				i.Pixbuf=p;
				Gtk.EventBox e=new Gtk.EventBox();
				e.Child=i;
				e.ButtonReleaseEvent+=handlerSet;
				e.TooltipText=name;
				columns.PackStart(e,true,true,0);
				counter++;
			}
			this.ShowAll();
		}

		private int getRgbColor(string name)
		{
			return Int32.Parse(
				(Types.RevType.GetRegisteredType("COLOR").NamedValues[name] as string)??"0",
				System.Globalization.NumberStyles.HexNumber
			);
		}

		protected void handlerSet(object sender,EventArgs e)
		{
			if(sender==this.buttonUnset){
				this.color=null;
			} else{
				this.color=(sender as Gtk.EventBox).TooltipText;
			}
			this.Destroy();
		}
	}
}

