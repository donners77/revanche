using System;

namespace Revanche.Views.Dialogs
{
	/// <summary>
	/// Dialog which confirms deletion of an object (for Collection objects, or refuses it for Children)
	/// </summary>
	public partial class Delete : Gtk.Dialog
	{
		private Models.Identifiable model;
		private bool canDelete;

		public Delete(Models.Identifiable model)
		{
			this.model=model;
			this.Build();
			this.Title="Delete "+model.Title;
			if(this.model.Parent==null||this.model.Parent.Group(model.GetModelInfo().Id)!=null&&this.model.Parent.Group(model.GetModelInfo().Id).Contains(model)){
				canDelete=true;
				this.label1.LabelProp="Are you sure you want to delete the "+model.GetModelInfo().Id+" '"+model.Title+"'?";
			} else{
				canDelete=false;
				this.label1.LabelProp="The node '"+model.Title+"' is not part of a group, and cannot be deleted without deleting its parent.";
				this.buttonOk.Hide();
			}
		}

		protected void handlerOkay(object sender,EventArgs e)
		{
			if(canDelete){
				model.Delete();
				MainWindow.Instance.ShowView(model.Parent);
			}
			this.Destroy();
		}

		protected void handlerCancel(object sender,EventArgs e)
		{
			this.Destroy();
		}
	}
}

