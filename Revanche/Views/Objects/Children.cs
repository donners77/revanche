using System;

namespace Revanche.Views.Objects
{
	/// <summary>
	/// UI component which renders an identifiable's Children. Creates a new Objects.Identifiable for each one, lists them in a Vbox.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Children : Gtk.Bin
	{
		private Models.Identifiable model;
		private Views.Objects.Identifiable parent;

		public Children()
		{
			this.Build();
		}

		public void Initialize(Models.Identifiable model,Views.Objects.Identifiable parent)
		{
			this.model=model;
			this.parent=parent;
		}

		public void Refresh()
		{
			foreach(Gtk.Widget child in this.vbox.Children){
				child.Destroy();
			}
			string[] children=model.GetModelInfo().Children;
			foreach(string key in children){
				Identifiable child=new Identifiable(model.Child(key),parent);
				this.vbox.PackStart(child);
				child.Show();
			}
			this.Show();
		}
	}
}

