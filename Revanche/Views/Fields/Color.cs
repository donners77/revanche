using System;

namespace Revanche.Views.Fields
{
	/// <summary>
	/// Edit field for COLOR type. Shows a button with a color preview and name, launches a palette when clicked.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Color : Gtk.Bin, ValuedField
	{
		private string color;
		private Gdk.Pixbuf dropper;

		public object Value
		{
			get{
				return color;
			}
		}

		public event EventHandler Tabbed;
		public event EventHandler BackTabbed;

		public Color(string color)
		{
			this.Build();
			this.dropper=image.Pixbuf;
			this.color=color;
			this.Refresh();
		}

		public void Refresh()
		{
			if(color!=null){
				Types.RevType colorType=Types.RevType.GetRegisteredType("COLOR");
				uint hex=UInt32.Parse(colorType.NamedValues[color] as string,System.Globalization.NumberStyles.HexNumber);
				Gdk.Pixbuf p=new Gdk.Pixbuf(Gdk.Colorspace.Rgb,true,8,16,16);
				p.Fill((hex<<8)|(hex==0?0x00u:0xffu));
				image.Pixbuf=p;
			} else{
				image.Pixbuf=dropper;
			}
			label.LabelProp=color??"Unspecified";
		}

		protected void handlerClicked(object sender,EventArgs e)
		{
			Dialogs.Color d=new Dialogs.Color(this.color);
			d.Show();
			d.Destroyed+=handlerSelected;
		}

		private void handlerSelected(object sender,EventArgs e)
		{
			this.color=(sender as Dialogs.Color).Selected;
			this.Refresh();
		}

		protected void handlerKey(object o,Gtk.KeyPressEventArgs args)
		{
			if(args.Event.Key==Gdk.Key.Tab){
				if((args.Event.State&Gdk.ModifierType.ShiftMask)>0){
					if(BackTabbed!=null){
						BackTabbed(this,EventArgs.Empty);
					}
					if(Tabbed!=null){
						Tabbed(this,EventArgs.Empty);
					}
				}
			}
		}

		public void Focus()
		{
			this.button18.GrabFocus();
		}
	}
}

