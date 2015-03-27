using System;

namespace Revanche.Views.Fields
{
	/// <summary>
	/// Edit field for all types of Content.ENUMERATED and Content.NULLABLE_ENUMERATED. ComboBox which can hold either strings (normal) or objects (reparenting)
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Select : Gtk.Bin, ValuedField
	{
		public object Value
		{
			get{
				return this.combobox.GetActive();
			}
		}

		public event EventHandler Tabbed;
		public event EventHandler BackTabbed;

		public Select(object[] values,object value,Type type)
		{
			this.Build();
			this.combobox.SetContents(values,type);
			this.combobox.SetActive(value);
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
			this.combobox.GrabFocus();
		}
	}
}

