using System;

namespace Revanche.Views.Displays
{
	/// <summary>
	/// Widget that displays colors in the main Identifiable view. Pretty much like Text, but additionally shows a preview of the color.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Color : Gtk.Bin
	{
		public Color(string color,bool note)
		{
			this.Build();
			if(color!=null){
				Types.RevType colorType=Types.RevType.GetRegisteredType("COLOR");
				uint hex=UInt32.Parse(colorType.NamedValues[color] as string,System.Globalization.NumberStyles.HexNumber);
				Gdk.Pixbuf p=new Gdk.Pixbuf(Gdk.Colorspace.Rgb,true,8,16,16);
				p.Fill((hex<<8)|0xffu);
				image.Pixbuf=p;
			}

			string text=color??"Unspecified";

			if(note){
				text="<"+Config.NOTE_STYLE+">"+text+"</"+Config.NOTE_STYLE+">";
			}
			label.LabelProp=text;
		}
	}
}

