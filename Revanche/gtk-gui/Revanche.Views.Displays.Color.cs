
// This file has been generated by the GUI designer. Do not modify.
namespace Revanche.Views.Displays
{
	public partial class Color
	{
		private global::Gtk.HBox hbox6;
		private global::Gtk.Label label;
		private global::Gtk.Image image;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Revanche.Views.Displays.Color
			global::Stetic.BinContainer.Attach (this);
			this.Name = "Revanche.Views.Displays.Color";
			// Container child Revanche.Views.Displays.Color.Gtk.Container+ContainerChild
			this.hbox6 = new global::Gtk.HBox ();
			this.hbox6.Name = "hbox6";
			this.hbox6.Spacing = 6;
			// Container child hbox6.Gtk.Box+BoxChild
			this.label = new global::Gtk.Label ();
			this.label.Name = "label";
			this.label.LabelProp = global::Mono.Unix.Catalog.GetString ("label7");
			this.label.UseMarkup = true;
			this.label.Wrap = true;
			this.hbox6.Add (this.label);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.label]));
			w1.PackType = ((global::Gtk.PackType)(1));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child hbox6.Gtk.Box+BoxChild
			this.image = new global::Gtk.Image ();
			this.image.Name = "image";
			this.hbox6.Add (this.image);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox6 [this.image]));
			w2.PackType = ((global::Gtk.PackType)(1));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			this.Add (this.hbox6);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Hide ();
		}
	}
}
