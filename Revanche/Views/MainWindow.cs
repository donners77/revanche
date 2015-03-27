using System;
using Gtk;
using System.Reflection;

namespace Revanche.Views
{
	/// <summary>
	/// The main Revanche window. Toolbar with navigation, actions for the current main view item, about dialog, and settings dialog.
	/// </summary>
	public partial class MainWindow: Gtk.Window
	{
		/// <summary>
		/// Get the single instance of MainWindow from elsewhere in the program.
		/// </summary>
		public static MainWindow Instance;
		private string baseTitle;
		private string[] collectables;

		/// <summary>
		/// MainWindow should only be constructed once.
		/// </summary>
		/// <param name="collectables">Collectables.</param>
		public MainWindow(string[] collectables): base (Gtk.WindowType.Toplevel)
		{
			if(Instance!=null){
				throw new InvalidOperationException();
			}
			Instance=this;
			Build();
			AssemblyName name=Assembly.GetExecutingAssembly().GetName();
			this.Title="Revanche "+name.Version.Major+"."+name.Version.Minor;
			#if DEBUG
			this.Title+="."+name.Version.Build+"."+name.Version.Revision;
			#endif
			baseTitle=this.Title;
			this.collectables=collectables;
		}

		/// <summary>
		/// Show an Identifiable as the root object. If root is null, show all objects that have no parents (Home screen).
		/// </summary>
		/// <param name="root">Root.</param>
		public void ShowView(Models.Identifiable root)
		{
			this.RefreshTitle(root);
			if(this.mainView.Child!=null){
				this.mainView.Remove(this.mainView.Child);
			}
			Gtk.Widget view;
			if(root==null){
				Objects.Collection c=new Objects.Collection();
				c.Initialize(Models.Identifiable.NoParents,this.collectables);
				view=c;
			} else{
				view=new Views.Objects.Identifiable(root,null);
			}
			this.mainView.Child=view;
			this.mainView.Child.Show();
		}

		/// <summary>
		/// Refresh the window title with information about the current root object.
		/// </summary>
		/// <param name="root">Root.</param>
		public void RefreshTitle(Models.Identifiable root)
		{
			string titleString="";
			if(root!=null){
				titleString=root.Title;
				Models.Identifiable iter=root.Parent;
				while(iter!=null){
					titleString=iter.Title+"/"+titleString;
					iter=iter.Parent;
				}
				titleString=" "+titleString;
			}

			this.Title=this.baseTitle+titleString;
		}

		/// <summary>
		/// Refresh all the UI components in the current view.
		/// </summary>
		public void RefreshView()
		{
			if(this.mainView.Child.GetType()==typeof(Views.Objects.Identifiable)){
				(this.mainView.Child as Views.Objects.Identifiable).Refresh();
			} else if(this.mainView.Child.GetType()==typeof(Views.Objects.Collection)){
				(this.mainView.Child as Views.Objects.Collection).Refresh();
			}
		}

		protected void OnDeleteEvent(object sender,DeleteEventArgs a)
		{
			Application.Quit();
			a.RetVal=true;
		}

		protected void handlerHome(object sender,EventArgs e)
		{
			this.ShowView(null);
		}

		protected void handlerUp(object sender,EventArgs e)
		{
			if(this.mainView.Child.GetType()==typeof(Views.Objects.Identifiable)){
				(this.mainView.Child as Views.Objects.Identifiable).Up(sender,e);
			}
		}

		protected void handlerSettings(object sender,EventArgs e)
		{
			Dialogs.Settings s=new Dialogs.Settings();
			s.Modal=true;
			s.Run();
			this.RefreshView();
		}

		protected void handleNotes(object sender,EventArgs e)
		{
			if(this.mainView.Child.GetType()==typeof(Views.Objects.Identifiable)){
				(this.mainView.Child as Objects.Identifiable).Notes(sender,e);
			}
		}

		public void Edit(){
			this.handleEdit(this,EventArgs.Empty);
		}

		protected void handleEdit(object sender,EventArgs e)
		{
			if(this.mainView.Child.GetType()==typeof(Views.Objects.Identifiable)){
				(this.mainView.Child as Objects.Identifiable).Edit(sender,e);
			}
		}

		protected void handlerDelete(object sender,EventArgs e)
		{
			if(this.mainView.Child.GetType()==typeof(Views.Objects.Identifiable)){
				(this.mainView.Child as Objects.Identifiable).DeleteModel(sender,e);
			}
		}

		protected void handlerAbout(object sender,EventArgs e)
		{
			Dialogs.About a=new Dialogs.About();
			a.Show();
		}
	}
}