using System;

namespace Revanche.Views.Objects
{
	/// <summary>
	/// UI component which renders an Identifiable. Creates a collapsible frame with title and button bar, then renders the properties, children, and collections of the Identifiable.
	/// Button options allow the user to refocus the main view onto this identifiable, view and edit notes, or edit the properties.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Identifiable : Gtk.Bin
	{
		private Models.Identifiable model;
		private Views.Objects.Identifiable parent;

		public string Title
		{
			get{
				return (parent!=null?parent.Title+"/":"")+this.model.Title;
			}
		}

		public Identifiable(Models.Identifiable model,Views.Objects.Identifiable parent)
		{
			this.parent=parent;
			this.model=model;
			this.Build();
			this.properties.Initialize(model);
			this.children.Initialize(model,this);
			this.collection.Initialize(model);
			if(parent==null){
				toolbar.Hide();
				separator.Hide();
				rootAlignment.TopPadding=0;
				containerFrame.ShadowType=Gtk.ShadowType.None;
			}
			this.Refresh();
		}

		protected void handlerArrowToggle(object o,Gtk.ButtonReleaseEventArgs args)
		{
			if(this.arrow.ArrowType==Gtk.ArrowType.Down){
				this.arrow.ArrowType=Gtk.ArrowType.Right;
				this.dataContainer.Hide();
				this.separator.Hide();
				this.actionButtons.Hide();
			} else{
				this.arrow.ArrowType=Gtk.ArrowType.Down;
				this.dataContainer.Show();
				this.separator.Show();
				this.actionButtons.Show();
			}
		}

		public void Refresh()
		{
			this.title.LabelProp=this.model.Title;
			if(parent==null){
				MainWindow.Instance.RefreshTitle(this.model);
			}
			if(!Config.BUTTON_ICONS){
				this.notesIcon.Hide();
				this.editIcon.Hide();
				this.viewIcon.Hide();
			}
			if(!Config.BUTTON_TEXT){
				this.notesLabel.Hide();
				this.editLabel.Hide();
				this.viewLabel.Hide();
			}
			this.notes.LabelProp=model.Notes;
			this.properties.Refresh();
			this.children.Refresh();
			this.collection.Refresh();
		}

		public void Up(object sender,EventArgs e)
		{
			MainWindow.Instance.ShowView(this.model.Parent);
		}

		public void Edit(object sender,EventArgs e)
		{
			this.handlerEdit(sender,e);
		}

		public void Notes(object sender,EventArgs e)
		{
			this.handlerNotes(sender,e);
		}

		public void DeleteModel(object sender,EventArgs e)
		{
			Dialogs.Delete d=new Dialogs.Delete(this.model);
			d.Modal=true;
			d.Run();
		}

		protected void handlerView(object sender,EventArgs e)
		{
			MainWindow.Instance.ShowView(this.model);
		}

		protected void handlerEdit(object sender,EventArgs e)
		{
			Dialogs.Edit d=new Dialogs.Edit(this.model,this);
			d.Show();
		}

		protected void handlerNotes(object sender,EventArgs e)
		{
			Dialogs.Note d=new Dialogs.Note(this.model,this);
			d.Show();
		}
	}
}

