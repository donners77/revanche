
// This file has been generated by the GUI designer. Do not modify.
namespace Revanche.Views.Dialogs
{
	public partial class Color
	{
		private global::Gtk.VBox rows;
		private global::Gtk.Button buttonUnset;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget Revanche.Views.Dialogs.Color
			this.Name = "Revanche.Views.Dialogs.Color";
			this.Title = global::Mono.Unix.Catalog.GetString ("Select Color");
			this.WindowPosition = ((global::Gtk.WindowPosition)(4));
			// Container child Revanche.Views.Dialogs.Color.Gtk.Container+ContainerChild
			this.rows = new global::Gtk.VBox ();
			this.rows.Name = "rows";
			this.rows.Spacing = 6;
			// Container child rows.Gtk.Box+BoxChild
			this.buttonUnset = new global::Gtk.Button ();
			this.buttonUnset.CanFocus = true;
			this.buttonUnset.Name = "buttonUnset";
			this.buttonUnset.UseUnderline = true;
			// Container child buttonUnset.Gtk.Container+ContainerChild
			global::Gtk.Alignment w1 = new global::Gtk.Alignment (0.5F, 0.5F, 0F, 0F);
			// Container child GtkAlignment.Gtk.Container+ContainerChild
			global::Gtk.HBox w2 = new global::Gtk.HBox ();
			w2.Spacing = 2;
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Image w3 = new global::Gtk.Image ();
			w3.Pixbuf = global::Stetic.IconLoader.LoadIcon (this, "gtk-clear", global::Gtk.IconSize.Menu);
			w2.Add (w3);
			// Container child GtkHBox.Gtk.Container+ContainerChild
			global::Gtk.Label w5 = new global::Gtk.Label ();
			w5.LabelProp = global::Mono.Unix.Catalog.GetString ("No Color");
			w5.UseUnderline = true;
			w2.Add (w5);
			w1.Add (w2);
			this.buttonUnset.Add (w1);
			this.rows.Add (this.buttonUnset);
			global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.rows [this.buttonUnset]));
			w9.Position = 0;
			w9.Expand = false;
			w9.Fill = false;
			this.Add (this.rows);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.DefaultWidth = 400;
			this.DefaultHeight = 300;
			this.Show ();
			this.buttonUnset.Clicked += new global::System.EventHandler (this.handlerSet);
		}
	}
}
