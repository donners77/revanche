using System;
using Gtk;
using YamlDotNet.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Revanche
{
	/// <summary>
	/// Main object controlling the program.
	/// </summary>
	public class Controller
	{
		public static void Main(string[] args)
		{
			// Start Gtk#
			Application.Init();

			// Show exceptions nicely
			try{
				// load configuration from the dot file
				Config.Load();

				// create the persistence layer and load structure (stage 1)
				Lib.YamlDataManager ydm=new Lib.YamlDataManager();

				// check to make sure structure includes models
				bool valid=false;
				RegisteredObject.Iterate((RegisteredObject r) => {
					if(r.GetType()==typeof(Models.RevModel)){
						valid=true;
					}
				});
				if(!valid){
					// no models, probably first run
					throw new NoModelsException();
				}

				// load data (stage 2)
				ydm.Load();

				// Create the main window
				Views.MainWindow win=new Views.MainWindow(ydm.Collectables);
				win.Show();
				// show the home screen
				win.ShowView(null);
				// Start event loop
				Application.Run();
				// this is a clean exit - backup existing data files
				ydm.Backup();
				// serialize all registered objects to the appropriate locations.
				ydm.Save();
			} catch(Exception e){
				// show the error dialog with information about this exception
				Views.Dialogs.Error d=new Views.Dialogs.Error(e);
				d.Modal=true;
				d.Show();
				// run the dialog
				d.Run();
				System.Environment.Exit(1);
			}
		}
	}

	/// <summary>
	/// Exception thrown when the structure does not contain any RevModel objects.
	/// This means Revanche has no screens to display except the home screen, and the home screen has nothing that could be added to it.
	/// </summary>
	public class NoModelsException : Exception
	{
	}
}
