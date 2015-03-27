using System;

namespace Revanche.Views.Fields
{
	/// <summary>
	/// Edit field for DECIMAL type. SpinButton that allows decimal inputs.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Decimal : Gtk.Bin, ValuedField
	{
		public object Value
		{
			get{
				return this.spinbutton.Value;
			}
		}

		public event EventHandler Tabbed;
		public event EventHandler BackTabbed;

		public Decimal(double value)
		{
			this.Build();
			this.spinbutton.Value=value;
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
			this.spinbutton.GrabFocus();
		}
	}
}

