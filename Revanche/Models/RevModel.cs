using System;
using System.Collections.Generic;

namespace Revanche.Models
{
	/// <summary>
	/// Primary class which provides a blueprint for Revanche screens and data
	/// </summary>
	public class RevModel : RegisteredObject
	{
		private string parent;
		// type information
		private List<string> children;
		private List<string> collection;
		private Dictionary<string,string> properties;

		/// <summary>
		/// Gets a model with a specific Id
		/// </summary>
		/// <returns>The registered model.</returns>
		/// <param name="name">Name.</param>
		public static RevModel GetRegisteredModel(string name)
		{
			return RevModel.GetRegisteredObject(name) as RevModel;
		}

		/// <summary>
		/// The property the pretty title should use.
		/// </summary>
		/// <value>The title property.</value>
		[YamlDotNet.Serialization.YamlIgnore]
		public string TitleProp
		{
			get{
				if(properties.ContainsKey("Title")&&properties["Title"]!=null&&properties["Title"].ToString()!=""){
					return "Title";
				} else if(properties.ContainsKey("Name")&&properties["Name"]!=null&&properties["Name"].ToString()!=""){
					return "Name";
				} else
					return null;
			}
		}

		/// <summary>
		/// List of properties which can be used for pretty titles.
		/// </summary>
		/// <value>The title properties.</value>
		[YamlDotNet.Serialization.YamlIgnore]
		public string[] TitleProps
		{
			get{
				return new string[] { "Title", "Name" };
			}
		}

		/// <summary>
		/// Model which is the parent of this model, if any.
		/// </summary>
		/// <value>The parent.</value>
		public string Parent
		{
			get{
				return this.parent;
			}
			set{
				parent=value;
			}
		}

		/// <summary>
		/// List of models which are children of this model, if any.
		/// </summary>
		/// <value>The children.</value>
		public string[] Children
		{
			get{
				return children.ToArray();
			}
			set{
				this.children=new List<string>(value);
			}
		}

		/// <summary>
		/// List of model types which this model may collect many of, if any.
		/// </summary>
		/// <value>The collection.</value>
		public string[] Collection
		{
			get{
				return collection.ToArray();
			}
			set{
				this.collection=new List<string>(value);
			}
		}

		/// <summary>
		/// List of properties this model has. Key is the property name, value is the RevType Id.
		/// </summary>
		/// <value>The properties.</value>
		public Dictionary<string,string> Properties
		{
			get{
				return this.properties;
			}
			set{
				properties=value;
			}
		}

		/// <summary>
		/// Persistence constructor
		/// </summary>
		public RevModel()
		{
			this.children=new List<string>();
			this.collection=new List<string>();
			this.properties=new Dictionary<string, string>();
		}

		/// <summary>
		/// This is the constructor you want.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="parent">Parent.</param>
		/// <param name="children">Children.</param>
		/// <param name="collection">Collection.</param>
		/// <param name="properties">Properties.</param>
		public RevModel(string name,string parent,List<string> children,List<string> collection,Dictionary<string,string> properties):base(name)
		{
			this.parent=parent;
			this.children=children;
			this.collection=collection;
			this.properties=properties;
		}

		public RevModel GetChildModel(string name)
		{
			if(children.Contains(name)){
				return RevModel.GetRegisteredModel(name);
			} else
				throw new ArgumentException("Not a child model type");
		}

		public RevModel GetCollectionModel(string name)
		{
			if(collection.Contains(name)){
				return RevModel.GetRegisteredModel(name);
			} else
				throw new ArgumentException("Not a collection model type");
		}

		public Revanche.Types.RevType GetPropertyType(string name)
		{
			if(properties.ContainsKey(name)){
				return Types.RevType.GetRegisteredType(properties[name]);
			} else
				throw new ArgumentException("Not a valid property");
		}

		public string[] GetPropertyNames()
		{
			return (new List<string>(this.properties.Keys)).ToArray();
		}
	}
}
