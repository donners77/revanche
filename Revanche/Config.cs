using System;
using Revanche.Lib;
using System.IO;

namespace Revanche
{
	/// <summary>
	/// Class which maintains user settings. Not stored in the revanche directory, stored in the user's home directory in a dot file.
	/// </summary>
	public static class Config
	{
		private static bool buttonIcons=true;
		private static bool buttonText=false;
		private static string userHome=
			( Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)!=""
				? Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
				: ( (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
					? Environment.GetEnvironmentVariable("HOME")
					: Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%") ) != ""
				? (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
					? Environment.GetEnvironmentVariable("HOME")
					: Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%")
				: Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().CodeBase) )
			+Path.DirectorySeparatorChar;
		private static string root=userHome+"Revanche"+Path.DirectorySeparatorChar;
		private static string noteStyle="i";
		private static uint propertyPadding=0u;
		private static string dateFormat="M/d/yyyy";
		private static string timeFormat="h:mm tt";

		/// <summary>
		/// Whether to show button icons in the main view.
		/// </summary>
		/// <value><c>true</c> if BUTTO n_ ICON; otherwise, <c>false</c>.</value>
		public static bool BUTTON_ICONS
		{
			get{
				return Config.buttonIcons;
			}
			set{
				buttonIcons=value;
				if(value==false){
					buttonText=true;
				}
			}
		}

		/// <summary>
		/// Whether to show button text in the main view.
		/// </summary>
		/// <value><c>true</c> if BUTTO n_ TEX; otherwise, <c>false</c>.</value>
		public static bool BUTTON_TEXT
		{
			get{
				return Config.buttonText;
			}
			set{
				buttonText=value;
				if(value==false){
					buttonIcons=true;
				}
			}
		}

		/// <summary>
		/// Root folder for structure, data, and backups.
		/// </summary>
		/// <value>The ROO.</value>
		public static string ROOT
		{
			get{
				return Config.root;
			}
			set{
				if(root!=value){
					// create parents and new directory, then delete new directory to prevent IOException, leaving parents
					Directory.CreateDirectory(value);
					Directory.Delete(value);
					Directory.Move(root,value);
				}
				root=value;
				if(!root.EndsWith(Path.DirectorySeparatorChar+"")){
					root+=Path.DirectorySeparatorChar;
				}
			}
		}

		/// <summary>
		/// What UI style to use to indicate a property has notes. Valid values are 'u' for underline or 'i' for italics.
		/// </summary>
		/// <value>The NOT e_ STYL.</value>
		public static string NOTE_STYLE
		{
			get{
				return Config.noteStyle;
			}
			set{
				noteStyle=value;
			}
		}

		/// <summary>
		/// How much padding to put between properties in the UI.
		/// </summary>
		/// <value>The PROPERT y_ PADDIN.</value>
		public static uint PROPERTY_PADDING
		{
			get{
				return Config.propertyPadding;
			}
			set{
				propertyPadding=value;
			}
		}

		/// <summary>
		/// Date format string.
		/// </summary>
		/// <value>The DAT e_ FORMA.</value>
		public static string DATE_FORMAT
		{
			get{
				return dateFormat;
			}
			set{
				dateFormat=value;
			}
		}

		/// <summary>
		/// Time format string.
		/// </summary>
		/// <value>The TIM e_ FORMA.</value>
		public static string TIME_FORMAT
		{
			get{
				return timeFormat;
			}
			set{
				timeFormat=value;
			}
		}

		/// <summary>
		/// Load configuration from the dot file.
		/// </summary>
		public static void Load()
		{
			string configfile=Config.userHome+".revanche";
			if(File.Exists(configfile)){
				using(StreamReader sr=new StreamReader(configfile)){
					string line;
					while((line=sr.ReadLine())!=null){
						if(line!=""){
							string[] config=line.Split('=');
							if(config.Length!=2){
								System.Console.Error.WriteLine("Malformed config option: "+line);
							} else{
								switch(config[0]){
									case "buttonIcons":
										buttonIcons=Boolean.Parse(config[1]);
										break;
										case "buttonText":
										buttonText=Boolean.Parse(config[1]);
										break;
										case "root":
										Directory.CreateDirectory(config[1]);
										root=config[1];
										break;
										case "noteStyle":
										if(config[1]!="i"&&config[1]!="u"){
											System.Console.Error.WriteLine("Invalid config option: "+line);
											System.Console.Error.WriteLine("Note style must be 'i' or 'u'.");
										} else{	
											noteStyle=config[1];
										}
										break;
										case "propertyPadding":
										propertyPadding=UInt32.Parse(config[1]);
										break;
										case "dateFormat":
										dateFormat=config[1];
										break;
										case "timeFormat":
										timeFormat=config[1];
										break;
								}
							}
						}
					}
				}
			} else{
				Config.Save();
			}
		}

		/// <summary>
		/// Save configuration to the dot file.
		/// </summary>
		public static void Save()
		{
			string configfile=userHome+".revanche";
			using(StreamWriter sw=new StreamWriter(configfile,false)){
				sw.WriteLine("buttonIcons="+buttonIcons.ToString());
				sw.WriteLine("buttonText="+buttonText.ToString());
				sw.WriteLine("root="+root.ToString());
				sw.WriteLine("noteStyle="+noteStyle.ToString());
				sw.WriteLine("propertyPadding="+propertyPadding.ToString());
				sw.WriteLine("dateFormat="+dateFormat.ToString());
				sw.WriteLine("timeFormat="+timeFormat.ToString());
			}
		}
	}
}

