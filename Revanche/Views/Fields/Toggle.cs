using System;

namespace Revanche.Views.Fields
{
	/// <summary>
	/// Edit field for BOOLEAN type. Toggle button that shows "Yes" or "No".
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Toggle : Gtk.Bin, ValuedField
	{
		public object Value
		{
			get{
				return this.togglebutton.Active;
			}
		}

		public event EventHandler Tabbed;
		public event EventHandler BackTabbed;

		public Toggle(bool value)
		{
			this.Build();
			this.togglebutton.Active=value;
			this.togglebutton.Label=value?"Yes":"No";
		}

		protected void handlerClicked(object sender,EventArgs e)
		{
			this.togglebutton.Label=this.togglebutton.Active?"Yes":"No";
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
			this.togglebutton.GrabFocus();
		}
	}
}

