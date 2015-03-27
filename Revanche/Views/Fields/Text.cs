using System;

namespace Revanche.Views.Fields
{
	/// <summary>
	/// Edit field for TEXT type. Starts as a single line, automagically grows vertically as the user types, and then conjures a scrollbar if it gets too tall.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Text : Gtk.Bin, ValuedField
	{
		Gtk.ScrolledWindow sw;
		bool usingSW;

		public object Value
		{
			get{
				return this.textview1.Buffer.Text;
			}
		}

		public event EventHandler Tabbed;
		public event EventHandler BackTabbed;

		public Text(string text)
		{
			this.usingSW=false;
			this.sw=new Gtk.ScrolledWindow();
			sw.HeightRequest=100;
			this.Build();
			this.textview1.Buffer.Text=text;
			handleSizeChange(null,EventArgs.Empty);
			this.textview1.Buffer.Changed+=handleSizeChange;
		}

		protected void handleSizeChange(object sender,EventArgs e)
		{
			if(textview1.Requisition.Height>100&&!usingSW){
				this.alignment1.Remove(textview1);
				this.alignment1.Add(sw);
				sw.Add(textview1);
				usingSW=true;
			} else if(textview1.Requisition.Height<=100&&usingSW){
				sw.Remove(textview1);
				this.alignment1.Remove(sw);
				this.alignment1.Add(textview1);
				usingSW=false;
			}
			this.ShowAll();
			this.textview1.GrabFocus();
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
			this.textview1.GrabFocus();
		}
	}
}

