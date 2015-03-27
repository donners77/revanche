using System;
using System.Collections.Generic;

namespace Revanche.Views.Objects
{
	/// <summary>
	/// UI component which renders an identifiable's collections. Creates a TreeView for each one, listing the contents of the collection in the tree with a column for each of the RevModel's properties.
	/// </summary>
	[System.ComponentModel.ToolboxItem(true)]
	public partial class Collection : Gtk.Bin
	{
		private Dictionary<string,Gtk.TreeView> collectionLists;
		private Models.Identifiable model;
		private List<Models.Identifiable> bag;

		public Collection()
		{
			this.Build();
		}

		/// <summary>
		/// For display of normal collections
		/// </summary>
		/// <param name="model">Model.</param>
		public void Initialize(Models.Identifiable model)
		{
			string[] collections=model.GetModelInfo().Collection;
			this.collectionLists=new Dictionary<string,Gtk.TreeView>();
			this.model=model;
			foreach(string key in collections){
				Gtk.TreeView collectionList=new Gtk.TreeView();
				collectionLists[key]=collectionList;
				collectionList.CanFocus=true;
				collectionList.RowActivated+=(object sender, Gtk.RowActivatedArgs e) => {
					handlerRowActivated(key); 
				};

				Gtk.TreeViewColumn tvc=new Gtk.TreeViewColumn();
				tvc.Title="Name";
				Gtk.CellRendererText crt=new Gtk.CellRendererText();
				tvc.PackStart(crt,true);
				collectionList.AppendColumn(tvc);
				tvc.SetCellDataFunc(crt,renderField);

				Models.RevModel modelType=Models.RevModel.GetRegisteredModel(key);

				foreach(string property in modelType.GetPropertyNames()){
					if(Array.IndexOf(modelType.TitleProps,property)<0){
						tvc=new Gtk.TreeViewColumn();
						tvc.Title=property;
						crt=new Gtk.CellRendererText();
						tvc.PackStart(crt,true);
						collectionList.AppendColumn(tvc);
						tvc.SetCellDataFunc(crt,renderField);
					}
				}

				Gtk.Label label=new Gtk.Label();
				label.UseMarkup=true;
				label.LabelProp="<b>"+key+"s</b>";
				label.SetAlignment(0,0.5f);

				Gtk.VBox containerBox=new Gtk.VBox();
				containerBox.Spacing=0;

				containerBox.PackStart(label,false,false,0);

				containerBox.PackStart(collectionList,true,true,0);

				Gtk.Button addButton=new Gtk.Button(Gtk.Stock.Add);
				addButton.Clicked+=(object sender, EventArgs e) => {
					handlerAdd(key);
				};

				containerBox.PackStart(addButton,false,false,0);

				this.vbox.PackStart(containerBox,true,true,0);
			}
			this.Refresh();
			this.ShowAll();
		}

		/// <summary>
		/// For display of null parents, called by the home screen.
		/// </summary>
		/// <param name="models">Models.</param>
		public void Initialize(Models.Identifiable[] models,string[] collectables)
		{
			this.bag=new List<Models.Identifiable>(models);
			this.collectionLists=new Dictionary<string,Gtk.TreeView>();
			Gtk.TreeView collectionList=new Gtk.TreeView();
			collectionList.WidthRequest=400;

			collectionLists["__BAG_DISPLAY__"]=collectionList;
			collectionList.CanFocus=true;
			collectionList.RowActivated+=(object sender, Gtk.RowActivatedArgs e) => {
				handlerRowActivated("__BAG_DISPLAY__"); 
			};

			foreach(string property in new string[]{"Name","Type"}){
				Gtk.TreeViewColumn tvc=new Gtk.TreeViewColumn();
				tvc.Title=property;
				Gtk.CellRendererText crt=new Gtk.CellRendererText();
				tvc.PackStart(crt,true);
				collectionList.AppendColumn(tvc);
				tvc.SetCellDataFunc(crt,renderField);
			}

			Gtk.VBox containerBox=new Gtk.VBox();
			containerBox.Spacing=0;

			containerBox.PackStart(collectionList,true,true,0);

			Gtk.Button addButton=new Gtk.Button(Gtk.Stock.Add);
			addButton.Clicked+=(object sender, EventArgs e) => {
				handlerAddCollectable(collectables);
			};

			containerBox.PackStart(addButton,false,false,0);

			this.vbox.PackStart(containerBox,true,true,0);

			this.Refresh();
			this.ShowAll();
		}

		private List<Models.Identifiable> getContents(string key)
		{
			if(model!=null){
				return model.Group(key);
			} else if(key=="__BAG_DISPLAY__"){
				return bag;
			}
			throw new ArgumentException(key);
		}

		public void Refresh()
		{
			foreach(KeyValuePair<string,Gtk.TreeView> kvp in collectionLists){
				Gtk.ListStore store=new Gtk.ListStore(typeof(Models.Identifiable));
				foreach(Models.Identifiable i in this.getContents(kvp.Key)){
					store.AppendValues(i);
				}
				kvp.Value.Model=store;
			}
		}

		protected void handlerRowActivated(string collection)
		{
			Gtk.TreeIter iter=new Gtk.TreeIter();
			collectionLists[collection].Selection.GetSelected(out iter);
			MainWindow.Instance.ShowView(collectionLists[collection].Model.GetValue(iter,0) as Models.Identifiable);
		}

		private void renderField(Gtk.TreeViewColumn column,Gtk.CellRenderer cell,Gtk.TreeModel treemodel,Gtk.TreeIter iter)
		{
			Models.Identifiable i=treemodel.GetValue(iter,0) as Models.Identifiable;

			object s="";
			if(this.model!=null){
				if(column.Title=="Name"){
					s=i.Title;
				} else{
					s=i.Property(column.Title);
					if(i.GetTypeInfo(column.Title).Id=="BOOLEAN"){
						s=((s as bool?)??false)?"Yes":"No";
					} else if(i.GetTypeInfo(column.Title).Id=="TIMESTAMP"){
						DateTime dt=new DateTime((s as long?)??0L);
						s=dt.ToString(Config.DATE_FORMAT+" "+Config.TIME_FORMAT);
					}
				}
			} else{
				s=(column.Title=="Name")?i.Title:(column.Title=="Type")?i.GetModelInfo().Id:"";
			}

			(cell as Gtk.CellRendererText).Text=(s??"").ToString();
		}

		protected void handlerAdd(string modelType)
		{
			Models.Identifiable newChild=new Models.Identifiable(this.model,Models.RevModel.GetRegisteredModel(modelType));
			MainWindow.Instance.ShowView(newChild);
		}

		protected void handlerAddCollectable(string[] collectables)
		{
			Dialogs.Collectable c=new Dialogs.Collectable(collectables);
			c.Modal=true;
			c.Show();
			c.Run();
		}
	}
}

