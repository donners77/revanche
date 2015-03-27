using System;
using System.Reflection;

namespace Revanche.Views.Dialogs
{
	/// <summary>
	/// Dialog which displays information about Revanche.
	/// </summary>
	public partial class About : Gtk.Dialog
	{
		public About()
		{
			this.Build();
			AssemblyName name=Assembly.GetExecutingAssembly().GetName();
			this.label2.LabelProp="Revanche "+name.Version.Major+"."+name.Version.Minor+"."+name.Version.Build+"."+name.Version.Revision+"\n\nReleased under the terms of the GNU General Public License, v3.0\n\nhttps://github.com/deinonychuscowboy/revanche";
		}

		protected void handlerOkay(object sender,EventArgs e)
		{
			this.Destroy();
		}
	}
}

