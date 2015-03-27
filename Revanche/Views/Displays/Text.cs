using System;

namespace Revanche.Views.Displays
{
	/// <summary>
	/// Widget that displays data in the main Identifiable view. No fancy handling needed since we aren't editing the values, just show them as strings.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Text : Gtk.Bin
	{
		public Text(object data,bool note)
		{
			this.Build();
			string text="";
			if(data.GetType()==typeof(bool)){
				if(data.Equals(true)){
					text="Yes";
				} else if(data.Equals(false)){
					text="No";
				}
			} else if(data.GetType()==typeof(long)){
				DateTime dt=new DateTime((data as long?)??0L);
				text=dt.ToString(Config.DATE_FORMAT+" "+Config.TIME_FORMAT);
			} else
				text=data.ToString();

			if(note){
				text="<"+Config.NOTE_STYLE+">"+text+"</"+Config.NOTE_STYLE+">";
			}
			this.label.LabelProp=text;
		}
	}
}

