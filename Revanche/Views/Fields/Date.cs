using System;

namespace Revanche.Views.Fields
{
	/// <summary>
	/// Edit field for TIMESTAMP type. Shows preview with the date, launches a calendar when clicked.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Date : Gtk.Bin, ValuedField
	{
		private long timestamp;

		public object Value
		{
			get{
				return this.timestamp;
			}
		}

		public event EventHandler Tabbed;
		public event EventHandler BackTabbed;

		public Date(long timestamp)
		{
			this.timestamp=timestamp;
			this.Build();
			this.Refresh();
		}

		public void Refresh()
		{
			DateTime dt=new DateTime(this.timestamp);
			label.LabelProp=dt.ToString(Config.DATE_FORMAT+" "+Config.TIME_FORMAT);
		}

		protected void handlerClicked(object sender,EventArgs e)
		{
			Dialogs.Date d=new Dialogs.Date(this.timestamp);
			d.Show();
			d.Destroyed+=handlerSelected;
		}

		private void handlerSelected(object sender,EventArgs e)
		{
			this.timestamp=(sender as Dialogs.Date).Timestamp;
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

