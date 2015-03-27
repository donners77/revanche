using System;
using Gtk;

namespace Revanche.Views.Base
{
	[System.ComponentModel.ToolboxItem(true)]
	/// <summary>
	/// The default gtk combo box doesn't let you easily set the currently selected item. Stupid!
	/// If given strings, shows the strings, if given objects, calls ToString(), if given null, shows <NONE>.
	/// </summary>
	public partial class NonStupidComboBox : Gtk.ComboBox
	{

		public NonStupidComboBox():base()
		{
			global::Stetic.Gui.Initialize(this);
			global::Stetic.BinContainer.Attach(this);
			this.Name="Revanche.Views.Base.NonStupidComboBox";
			if((this.Child!=null)){
				this.Child.ShowAll();
			}
			CellRendererText crt=new CellRendererText();
			this.PackStart(crt,false);
			this.AddAttribute(crt,"text",0); 
			this.SetCellDataFunc(crt,renderObject);
			this.Hide();
		}

		public void SetContents(object[] c,Type t)
		{
			ListStore store=new ListStore(t);
			foreach(object o in c){
				store.AppendValues(o);
			}
			this.Model=store;
		}

		public void SetActive(object o)
		{
			int i=0;
			foreach(object[] r in (ListStore)this.Model){
				if(r[0]==null&&o==null||r[0]!=null&&r[0].Equals(o)){
					this.Active=i;
					break;
				}
				i++;
			}
		}

		public object GetActive()
		{
			int i=0;
			foreach(object[] r in (ListStore)this.Model){
				if(i==this.Active){
					return r[0];
				}
				i++;
			}
			return null;
		}

		private void renderObject(CellLayout celllayout,CellRenderer cell,TreeModel treemodel,TreeIter iter)
		{
			object i=treemodel.GetValue(iter,0);

			(cell as Gtk.CellRendererText).Text=(i??"<NONE>").ToString();
		}
	}
}

