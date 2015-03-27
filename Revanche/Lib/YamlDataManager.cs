using System;
using YamlDotNet.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace Revanche.Lib
{
	/// <summary>
	/// Data management class which uses the YAML persistence layer to set up Revanche's type system
	/// </summary>
	public class YamlDataManager
	{
		private List<string> collectables;

		/// <summary>
		/// Fetches models which can possibly be stored in a collection, or which have no defined parent model.
		/// </summary>
		/// <value>The collectables.</value>
		public string[] Collectables
		{
			get{
				return this.collectables.ToArray();
			}
		}

		/// <summary>
		/// Constructs a YamlDataManager and loads the Revanche structure in the process (stage 1)
		/// </summary>
		public YamlDataManager()
		{
			Directory.CreateDirectory(Config.ROOT+"struct");
			Directory.CreateDirectory(Config.ROOT+"data");
			Directory.CreateDirectory(Config.ROOT+"backup");

			if(!File.Exists(Config.ROOT+"struct"+Path.DirectorySeparatorChar+"README")){
				this.WriteReadmeFile(Config.ROOT+"struct"+Path.DirectorySeparatorChar+"README");
			}

			Types.RevType.SetupBasicTypes();

			Lib.Yaml<Types.RevType> tl=new Lib.Yaml<Types.RevType>(new Serializer(),new Deserializer());
			Lib.Yaml<Models.RevModel> ml=new Lib.Yaml<Models.RevModel>(new Serializer(),new Deserializer());

			List<string> files=new List<string>();
			files.Add("types");
			files.Add("models");

			foreach(string file in files){
				if(File.Exists(Config.ROOT+"struct"+Path.DirectorySeparatorChar+"index_"+file)){
					using(System.IO.StreamReader sr=new System.IO.StreamReader(Config.ROOT+"struct"+Path.DirectorySeparatorChar+"index_"+file)){
						string name;
						while((name=sr.ReadLine())!=null){
							try{
								switch(file){
									case "types":
										List<Types.RevType> toCheck=tl.Load("struct"+Path.DirectorySeparatorChar+file+"_"+name);
										foreach(Types.RevType type in toCheck){
											if(type.ContentType==Revanche.Types.Content.ENUMERATED&&(type.DefaultValue==null||type.DefaultValue.ToString()=="")){
												if(type.Id=="COLOR"){
													type.ContentType=Revanche.Types.Content.NULLABLE_ENUMERATED;
												} else{
													throw new Exception("Enumerated content types require a default: "+type.Id);
												}
											}
										}
										break;
									case "models":
										ml.Load("struct"+Path.DirectorySeparatorChar+file+"_"+name);
										break;
								}
							} catch(YamlDotNet.Core.YamlException e){
								string specialMessage;
								if(e.InnerException!=null&&(new Regex(@"Property '")).IsMatch(e.InnerException.Message)){
									specialMessage="'"+e.InnerException.Message.Split('\'')[1]+"' is not a known element.";
								} else{
									specialMessage=(new Regex(@"\([^\)]*\)")).Replace((e.InnerException??e).Message,"");
								}
								throw new FormatException("Malformed YAML in file "+Config.ROOT+"struct"+Path.DirectorySeparatorChar+file+"_"+name+".yml\nLine "+e.End.Line+": "+specialMessage);
							}
						}
					}
				}
			}

			this.collectables=new List<string>();

			RegisteredObject.Iterate((RegisteredObject r) => {
				if(r.GetType()==typeof(Revanche.Models.RevModel)){
					foreach(string s in (r as Revanche.Models.RevModel).Collection){
						if(!this.collectables.Contains(s)){
							this.collectables.Add(s);
						}
					}
					if((r as Revanche.Models.RevModel).Parent==null){
						string s=(r as Revanche.Models.RevModel).Id;
						if(!this.collectables.Contains(s)){
							this.collectables.Add(s);
						}
					}
				}
			});
		}

		/// <summary>
		/// Loads the Revanche data (stage 2).
		/// </summary>
		public void Load()
		{
			if(File.Exists(Config.ROOT+"data"+Path.DirectorySeparatorChar+"index")){
				Lib.Yaml<Models.Identifiable> il=new Lib.Yaml<Revanche.Models.Identifiable>(new Serializer(),new Deserializer());
				using(System.IO.StreamReader sr=new System.IO.StreamReader(Config.ROOT+"data"+Path.DirectorySeparatorChar+"index")){
					string id;
					while((id=sr.ReadLine())!=null){
						il.Load("data"+Path.DirectorySeparatorChar+id);
					}
				}
			}
		}

		/// <summary>
		/// Cycles backup folders. Maintains up to 10 backups.
		/// </summary>
		public void Backup()
		{
			if(Directory.Exists(Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~9")){
				Directory.Delete(Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~9",true);
			}
			if(Directory.Exists(Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~8")){
				Directory.Move(
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~8",
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~9"
				);
			}
			if(Directory.Exists(Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~7")){
				Directory.Move(
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~7",
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~8"
				);
			}
			if(Directory.Exists(Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~6")){
				Directory.Move(
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~6",
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~7"
				);
			}
			if(Directory.Exists(Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~5")){
				Directory.Move(
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~5",
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~6"
				);
			}
			if(Directory.Exists(Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~4")){
				Directory.Move(
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~4",
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~5"
				);
			}
			if(Directory.Exists(Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~3")){
				Directory.Move(
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~3",
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~4"
				);
			}
			if(Directory.Exists(Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~2")){
				Directory.Move(
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~2",
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~3"
				);
			}
			if(Directory.Exists(Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~1")){
				Directory.Move(
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~1",
					Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~2"
				);
			}
			if(Directory.Exists(Config.ROOT+"data")){
				Directory.Move(Config.ROOT+"data",Config.ROOT+"backup"+Path.DirectorySeparatorChar+"data~1");
			}
			Directory.CreateDirectory(Config.ROOT+"data");
		}

		/// <summary>
		/// Saves the in-ram data structures through the persistence layer.
		/// </summary>
		public void Save()
		{
			Lib.Yaml<Models.Identifiable> il=new Lib.Yaml<Revanche.Models.Identifiable>(new Serializer(),new Deserializer());
			using(System.IO.StreamWriter sw=new System.IO.StreamWriter(Config.ROOT+"data"+Path.DirectorySeparatorChar+"index",false)){
				foreach(Models.Identifiable i in Models.Identifiable.NoParents){
					List<Models.Identifiable> flatList=i.Flatten();
					// reverse so that children are serialized first to make a nice clean flat list instead of a descending heirarchy of indents
					flatList.Reverse();
					il.Save(flatList,"data"+Path.DirectorySeparatorChar+i.Id);
					sw.WriteLine(i.Id);
				}
			}
		}

		/// <summary>
		/// Creates the readme file to help people with writing structures.
		/// </summary>
		/// <param name="path">Path.</param>
		private void WriteReadmeFile(string path)
		{
			using(StreamWriter sw=new StreamWriter(path)){
				sw.Write(@"REVANCHE STRUCTURES FOLDER
==========================

This folder contains the specification for your Revanche data! Creating
a specification is the first step to storing your data in Revanche.

Revanche uses [YAML](http://yaml.org/) to store data. YAML is a very
simple 'markup language', a kind of programming language that provides
both data and additional information about that data. Writing a YAML
document should be easy, even if you've never programmed before, as it's
meant to be very similar to a bulleted list you might jot down on a
notepad.

Brief YAML Tutorial
-------------------

There is no definitive YAML tutorial, although searching for
'YAML Tutorial' in your favorite search engine is pretty helpful. A
quick-and-dirty guide follows.

Everything in YAML is a list. There are two kinds of lists. The first is
a sequence.

- Tacos
- Enchiladas
- Burritos

This is a sequence. Sequences are indicated by a hyphen followed by a
space, and then the item, much like a bulleted list on a piece of paper.

The other kind of list is a mapping. In a mapping, each item has both a
name and a value.

Guacamole: Delicious
Salsa: Overrated but pleasant
SourCream: Mandatory

A mapping is defined without hyphens, and has a colon and a space
separating the name from the value. As you can see, names must be one
word (no spaces), while values can be multiple words. Sequences can also
contain multiple words per item.

Sequences and mappings can be nested inside each other without limit. To
nest a sequence or mapping, simply indent it. Make sure to be consistent
with your indents - use either space characters or tab characters, not
both.

Here are some nested mappings and sequences:

- Bananas: Potassium
  Oranges: Vitamin C
  Spinach:
    - Vitamin A
    - Vitamin C
    - Vitamin K
    - Magnesium
    - Manganese
    - Folate
    - Iron
- Burrito:
    - Sour Cream
    - Beef
    - Hot Sauce
    - Cheese: Queso
    - Rice
    - Guac
    - Black Beans
  Taco:
    - Chicken
    - Lettuce
    - Sour Cream
    - Cheese: Queso
    - Hot Sauce
    - Salsa

This is essentially what Revanche's structures look like. For your own
purposes, you can also make notes using the # character. Anything on a
line beginning with a # will be ignored; this is called a comment.

- Burrito:
# I prefer burritos to tacos
    - Sour Cream

All Revanche YAML documents contain a sequence at the base level, with
additional sequences or mappings nested inside.

Introduction to Revanche Structures
-----------------------------------

Revanche has two structure components: Types and Models. Both are
required to set up data in Revanche. You are free to define these in as
many files as you like, with these expectations:

1. Files containing types begin with the prefix 'types_' and end with
'.yml'
2. Files containing models begin with the prefix 'models_' and end with
'.yml'
3. All files containing types are listed, one per line, in 'index_types'
(not 'index_types.yml', 'index_types.txt', or otherwise), without the
'types_' prefix or '.yml'
4. All files containing models are listed, one per line, in
'index_models', without the 'models_' prefix or '.yml'

If you prefer not to use multiple files, simply create an index_types
with the contents 'all' and an index_models with the contents 'all', and
define your types and models in the files 'types_all.yml' and
'models_all.yml'.

Types
-----

Some types are included by default and do not need to be specified in
the structures folder, these are as follows:
BOOLEAN   - true or false
DECIMAL   - decimal number
INTEGER   - number with no decimal
TEXT      - raw text data with no fancy handling
TIMESTAMP - date and time information

Each of these built-in types has special UI rendering that make editing
it easier and more comfortable. These types will probably not be
sufficient for your purposes, but if you do not need any other types in
your models, you can run Revanche with just the built-in types, and skip
this section.

Types are a specification of what values should be options in the UI.
User-defined types are always rendered as a dropdown - if you want a
different UI element, such as a text box, use one of the
specially-handled built-in types above. Most types have only three
attributes, in a mapping. Types correspond to individual edit fields on
a Revanche screen.

Id - This defines the name of the type. The convention for types is to
make this name all caps; please follow it.
Values - This defines what values are available to be selected.
DefaultValue - This matches one of the items in the Values attribute,
and is required - if you want to be able to set a type to nothing, you
must put 'Nothing' in the Values list, and list it as the default value.

Here is an example type list:

- Id: VOICE
  Values:
    - Tenor
    - High Baritone
    - Low Baritone
    - High Bass
    - Low Bass
    - Raspy
    - Husky
  DefaultValue: High Baritone

You may also define types with both names and values, using the
NamedValues property.

Special handling is included for a type named COLOR. This type uses
NamedValues to provide both color names and hexadeximal color values to
display on the screen, and also uses Values to break this color list
into rows in a UI dialog. DefaultValue is not required for the COLOR
type. The COLOR type is in some ways built in, like the BOOLEAN and
similar types, but you must specify it yourself; no default definition
is included.

Here is the COLOR definition I use, as an example:

- Id: COLOR
  Values:
    - 10
    - 7
    - 8
    - 10
    - 10
  NamedValues:
    Black: 000000
    Smoke: 333333
    Gray: 777777
    Silver: bbbbbb
    White: ffffff
    Russet: 771100
    Vermillion: ff3300
    Dusky: bbaa88
    Cream: ffeeaa
    Tawny: cccc55
    Copper: cc6600
    Brown: 773300
    Umber: 331100
    Hazel: 667700
    Green: 007700
    Cadet: 003377
    Steel: 7799bb
    Orchid: dd77ff
    Pink: ff99cc
    Rose: ff8888
    Salmon: ff9977
    Citrine: ddff77
    Spring: bbff77
    Sky: 77ffff
    Periwinkle: bb99ff
    Red: ff0000
    ConstructionYellow: ffcc00
    ElectricYellow: ddff00
    Chartreuse: 77ff00
    NeonGreen: 00ff00
    Somtaaw: 426CCA
    Cobalt: 0000ff
    Indigo: aa00ff
    Fuchsia: ff00ff
    Claret: cc0066
    Gold: 777700
    Viridian: 007733
    Teal: 007777
    Navy: 000077
    Ultramarine: 330077
    Plum: 550077
    Purple: 770077
    Bordeaux: 770055
    Burgundy: 770033
    Maroon: 770011

Models
------

Models specify the structure of your objects, and correspond to an
entire screen in Revanche's UI. Models are a blueprint for objects.
Considering the original use of Revanche as a management tool for
characters in fiction stories, the Character model is the central model,
and objects saved in the data folder may be instances of the Character
model. You would not define a model for each character, just one for all
of them.

Revanche models have several attributes, all but one of which are
optional. The single mandatory attribute is:

Id - as with types, this defines the name of the model. The convention
for models is to make this CamelCase; please follow it.

The optional attributes are as follows:

Parent - This specifies which model 'owns' this one. Most models should
have this attribute.
Children - This is a sequence of models whose parent is this one. An
instance may have exactly one instance of each model in its model's
Children list. One parent, one child.
Collection - This is also a sequence of models whose parent is this one.
An instance may have many instances of each model in its model's
Collection list.
Properties - This is the meat and potatoes of a model definition. The
properties list is a mapping, with the name being shown in the UI and
the value being a type (see the discussion of types above). The
properties list defines the editable fields you will see in the UI, and
refers to the defined types to provide information about those fields.

Here is an example definition of three basic models for the
fiction-writing use case:

- Id: World
  Collection:
    - Project
  Properties:
    Name: TEXT
- Id: Project
  Parent: World
  Collection:
    - Character
  Properties:
    Name: TEXT
    Location: LOCATION
    DateTime: TIMESTAMP
- Id: Character
  Parent: Project
  Collection:
    - TropeReference
  Children:
    - Coloration
    - Attire
    - Body
    - Behavior
  Properties:
    Name: TEXT
    Gender: GENDER
    Age: DECIMAL
    Occupation: TEXT
    Voice: VOICE

In this example, you can see three models: World, Project, and
Character. An instance of World can contain many Projects, an instance
of Project can contain many Characters. The Character model also refers
to a TropeReference model, which it can contain many of, and four other
models, which it contains exactly one of. Child models are a good way to
break up properties so you don't get a huge list that's difficult to
read through.

Each model also contains a list of properties. If you use CamelCase in
defining your property names, Revanche will split the names and insert a
space before each capital letter in the UI. The values refer to types.
As mentioned above, the TEXT, TIMESTAMP, and DECIMAL types are built-in;
the others would need to be specified.

Revanche also handles properties named 'Name' or 'Title' specially. In
this example, since Character has a Name property, objects which are
instances of the Character model will be identified by this property
in the UI. In other words, you will see Characters listed by the names
you give them. If a model does not have a Name or Title property, 
Revanche falls back to using the Id for UI display.

Closing
-------

For more examples of types and models, please refer to the 
[Revanche source code](https://github.com/deinonychuscowboy/revanche).
");
			}
		}
	}
}

