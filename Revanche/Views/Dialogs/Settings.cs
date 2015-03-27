using System;

namespace Revanche.Views.Dialogs
{
	/// <summary>
	/// Dialog which displays Revanche settings (i.e. edits the Config object)
	/// </summary>
	public partial class Settings : Gtk.Dialog
	{
		public Settings()
		{
			this.Build();
			this.combobox1.Active=!Config.BUTTON_ICONS?1:!Config.BUTTON_TEXT?0:2;
			this.entry1.Text=Config.ROOT;
			this.combobox2.Active=Config.NOTE_STYLE=="i"?0:1;
			this.spinbutton1.Value=Config.PROPERTY_PADDING;
			this.entry2.Text=Config.DATE_FORMAT;
			this.entry3.Text=Config.TIME_FORMAT;
		}

		protected void handlerCancel(object sender,EventArgs e)
		{
			this.Destroy();
		}

		protected void handlerOkay(object sender,EventArgs e)
		{
			switch(this.combobox1.Active){
				case 0:
					Config.BUTTON_ICONS=true;
					Config.BUTTON_TEXT=false;
					break;
				case 1:
					Config.BUTTON_ICONS=false;
					Config.BUTTON_TEXT=true;
					break;
				case 2:
					Config.BUTTON_ICONS=true;
					Config.BUTTON_TEXT=true;
					break;
			}
			Config.ROOT=this.entry1.Text;
			Config.NOTE_STYLE=this.combobox2.Active==0?"i":"u";
			Config.PROPERTY_PADDING=(uint)this.spinbutton1.ValueAsInt;
			Config.DATE_FORMAT=this.entry2.Text;
			Config.TIME_FORMAT=this.entry3.Text;
			Config.Save();
			this.Destroy();
		}
	}
}

