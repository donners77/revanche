
// This file has been generated by the GUI designer. Do not modify.
namespace Revanche.Views.Fields
{
	public partial class Select
	{
		private global::Revanche.Views.Base.NonStupidComboBox combobox;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Revanche.Views.Fields.Select
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Revanche.Views.Fields.Select";
			// Container child Revanche.Views.Fields.Select.Gtk.Container+ContainerChild
			this.combobox = new Revanche.Views.Base.NonStupidComboBox();
			this.combobox.Name = "combobox";
			this.Add (this.combobox);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
			this.combobox.KeyPressEvent += new global::Gtk.KeyPressEventHandler (this.handlerKey);
		}
	}
}
