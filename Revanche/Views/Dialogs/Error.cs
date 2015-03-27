using System;

namespace Revanche.Views.Dialogs
{
	/// <summary>
	/// Dialog which displays any exceptions that may occur, as well as the first-time welcome message (NoModelsException)
	/// </summary>
	public partial class Error : Gtk.Dialog
	{
		public Error(Exception ex)
		{
			this.Build();
			if(ex.GetType()!=typeof(NoModelsException)){
				this.image5.Hide();
				this.label2.LabelProp="An error was encountered, and Revanche must close.\n\n"+ex.Message;
				this.buttonOk.Hide();
			} else{
				this.Title="Welcome";
				this.image4.Hide();
				this.label2.LabelProp="Welcome to Revanche!\n\nIt looks like this is your first time running this program.\nNo models were found in '"+Config.ROOT+"struct'.\n\nRevanche cannot run until models are defined.\nI have created a README file in this folder to help you set up Revanche for the first time.\n\nIf this is not your first time running Revanche, please make sure your configuration in '"+Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)+System.IO.Path.DirectorySeparatorChar+".revanche"+"' is pointing to the correct directory.";
				this.buttonCancel.Hide();
			}
		}

		protected void handlerButton(object sender,EventArgs e)
		{
			this.Destroy();
		}
	}
}

