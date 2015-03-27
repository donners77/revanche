using System;

namespace Revanche.Views.Dialogs
{
	/// <summary>
	/// Dialog which displays a calendar for date selection.
	/// </summary>
	public partial class Date : Gtk.Dialog
	{
		private long timestamp;

		public long Timestamp
		{
			get{
				return this.timestamp;
			}
		}

		public Date(long timestamp)
		{
			this.timestamp=timestamp;
			this.Build();
		}

		protected void handlerOkay(object sender,EventArgs e)
		{
			this.timestamp=this.calendar3.GetDate().Ticks+(System.DateTime.Now.Ticks-System.DateTime.Today.Ticks);
			this.Destroy();
		}

		protected void handlerCancel(object sender,EventArgs e)
		{
			this.Destroy();
		}
	}
}

