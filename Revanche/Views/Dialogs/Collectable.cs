using System;

namespace Revanche.Views.Dialogs
{
	/// <summary>
	/// Dialog which displays collectable or parentless models which may be constructed from the home screen.
	/// </summary>
	public partial class Collectable : Gtk.Dialog
	{
		public Collectable(string[] collectables)
		{
			this.Build();
			this.nonstupidcombobox1.SetContents(collectables,typeof(string));
			if(collectables.Length>0){
				this.nonstupidcombobox1.SetActive(collectables[0]);
			}
		}

		protected void handlerOkay(object sender,EventArgs e)
		{
			Models.Identifiable newChild=new Models.Identifiable(
				null,
				Models.RevModel.GetRegisteredModel(this.nonstupidcombobox1.GetActive() as string)
			);
			MainWindow.Instance.ShowView(newChild);
			this.Destroy();
		}

		protected void handlerCancel(object sender,EventArgs e)
		{
			this.Destroy();
		}
	}
}

