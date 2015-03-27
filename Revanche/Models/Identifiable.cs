using System;
using System.Collections.Generic;
using Revanche.Types;

namespace Revanche.Models
{
	/// <summary>
	/// Main data object. Represents an instance of a RevModel blueprint.
	/// </summary>
	public class Identifiable : RegisteredObject
	{
		/// <remarks>
		/// I'd just like to point out that you were given every opportunity to succeed.
		/// </remarks>
		/// <permission cref="party">
		/// PartyProvider - There was even going to be a party for you.
		/// InvitationProvider - A big party that all your friends were invited to.
		/// ICompanionCube - I invited your best friend, the Companion Cube.
		/// </permission>
		/// <exception>
		/// MurderException - Of course, he couldn't come, because you murdered him.
		/// NoFriendsException - All your other friends couldn't come either, because you don't have any other friends.
		/// UnlikeableException - Because of how unlikeable you are.
		/// </exception>
		/// <see cref="personnel-chell">
		/// It says so here in your personnel file: Unlikeable. Liked by no one. A bitter, unlikeable loner whose passing shall not be mourned.
		/// </see>
		/// <summary>
		/// "Shall not be mourned." That's exactly what it says. Very formal. Very official. 
		/// </summary>
		/// <seealso cref="ibid">
		/// It also says you were adopted. So that's funny too.
		/// </seealso>
		private static List<Identifiable> noParents=new List<Identifiable>();
		private RevModel revModel;
		private Identifiable parent;
		// type information
		private Dictionary<string,Identifiable> children;
		private Dictionary<string,List<Identifiable>> collection;
		private Dictionary<string,object> properties;
		private string notes;
		private Dictionary<string,string> propertyNotes;

		/// <summary>
		/// Gets Identifiable instances with no parents.
		/// </summary>
		/// <value>The no parents.</value>
		[YamlDotNet.Serialization.YamlIgnore]
		public static Identifiable[] NoParents
		{
			get{
				return noParents.ToArray();
			}
		}

		/// <summary>
		/// Get an identifiable with a specific Id.
		/// </summary>
		/// <returns>The registered identifiable.</returns>
		/// <param name="id">Identifier.</param>
		public static Identifiable GetRegisteredIdentifiable(string id)
		{
			return Identifiable.GetRegisteredObject(id) as Identifiable;
		}

		/// <summary>
		/// Get all Identifiable objects which are instances of the given RevModel Id.
		/// </summary>
		/// <returns>The all of model type.</returns>
		/// <param name="model">Model.</param>
		public static Identifiable[] GetAllOfModelType(string model)
		{
			List<Identifiable> i=new List<Identifiable>();
			RegisteredObject.Iterate((RegisteredObject r) => {
				if(r.GetType()==typeof(Identifiable)&&(r as Identifiable).GetModelInfo().Id==model){
					i.Add(r as Identifiable);
				}
			});
			return i.ToArray();
		}

		/// <summary>
		/// Pretty title for UI display.
		/// </summary>
		/// <value>The title.</value>
		[YamlDotNet.Serialization.YamlIgnore]
		public string Title
		{
			get{
				if(revModel.TitleProp!=null&&properties[revModel.TitleProp]!=null&&properties[revModel.TitleProp].ToString()!=""){
					return properties[revModel.TitleProp].ToString();
				} else
					return (this.parent!=null&&this.parent.Group(this.revModel.Id)!=null?"Untitled ":"")+this.revModel.Id;
			}
		}

		/// <summary>
		/// Gets or sets the parent object.
		/// </summary>
		/// <value>The parent.</value>
		[YamlDotNet.Serialization.YamlIgnore]
		public Identifiable Parent
		{
			get{
				return this.parent;
			}
			set{
				if(value!=null&&noParents.Contains(this)){
					noParents.Remove(this);
				} else if(value==null&&!noParents.Contains(this)){
					noParents.Add(this);
				}
				parent=value;
			}
		}

		/// <summary>
		/// Gets or sets the RevModel, used to interperet the data stored in this object.
		/// Please prefer GetModelInfo() to using this property, it's intended for use by the persistence layer.
		/// </summary>
		/// <value>The model.</value>
		public string Model
		{
			get{
				return this.revModel.Id;
			}
			set{
				this.revModel=RevModel.GetRegisteredModel(value);
			}
		}

		/// <summary>
		/// Gets or sets the children of this object.
		/// Please prefer Child() to using this property, it's intended for use by the persistence layer.
		/// </summary>
		/// <value>The children.</value>
		public Dictionary<string, Identifiable> Children
		{
			get{
				return this.children;
			}
			set{
				children=value;
				foreach(KeyValuePair<string,Identifiable> i in children){
					i.Value.Parent=this;
				}
			}
		}

		/// <summary>
		/// Gets or sets the collection objects this object holds.
		/// Please prefer Group() to using this property, it's intended for use by the persistence layer.
		/// </summary>
		/// <value>The collection.</value>
		public Dictionary<string, List<Identifiable>> Collection
		{
			get{
				return this.collection;
			}
			set{
				collection=value;
				foreach(KeyValuePair<string,List<Identifiable>> l in collection){
					foreach(Identifiable i in l.Value){
						i.Parent=this;
					}
				}
			}
		}

		/// <summary>
		/// Gets or sets the properties of this object.
		/// Please prefer Property() and SetProperty() to using this property, it's intended for use by the persistence layer.
		/// </summary>
		/// <value>The properties.</value>
		public Dictionary<string, object> Properties
		{
			get{
				return this.properties;
			}
			set{
				properties=new Dictionary<string, object>();
				foreach(KeyValuePair<string,object> kvp in value){
					object castedValue=kvp.Value;
					RevType type=this.revModel.GetPropertyType(kvp.Key);
					if(type==RevType.GetRegisteredType("DECIMAL")&&castedValue.GetType()!=typeof(double)){
						castedValue=Double.Parse(kvp.Value.ToString());
					}
					if(type==RevType.GetRegisteredType("INTEGER")&&castedValue.GetType()!=typeof(int)){
						castedValue=Int32.Parse(kvp.Value.ToString());
					}
					if(type==RevType.GetRegisteredType("BOOLEAN")&&castedValue.GetType()!=typeof(bool)){
						castedValue=Boolean.Parse(kvp.Value.ToString());
					}
					if(type==RevType.GetRegisteredType("TIMESTAMP")&&castedValue.GetType()!=typeof(long)){
						castedValue=Int64.Parse(kvp.Value.ToString());
					}
					properties.Add(kvp.Key,castedValue);
				}
			}
		}

		/// <summary>
		/// Gets or sets the global notes for this object.
		/// </summary>
		/// <value>The notes.</value>
		public string Notes
		{
			get{
				return this.notes;
			}
			set{
				notes=value??"";
			}
		}

		/// <summary>
		/// Gets or sets the property notes.
		/// Please prefer PropertyNote() and SetPropertyNote() to using this property, it's intended for use by the persistence layer.
		/// </summary>
		/// <value>The property notes.</value>
		public Dictionary<string, string> PropertyNotes
		{
			get{
				return this.propertyNotes;
			}
			set{
				Dictionary<string,string> reinitialized=new Dictionary<string, string>();
				foreach(KeyValuePair<string,string> kvp in value){
					reinitialized[kvp.Key]=kvp.Value??"";
				}
				propertyNotes=reinitialized;
			}
		}

		/// <summary>
		/// Persistence layer constructor
		/// </summary>
		public Identifiable()
		{
			noParents.Add(this);
		}

		/// <summary>
		/// This is the constructor you want to use
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="model">Model.</param>
		public Identifiable(Identifiable parent,RevModel model)
		{
			this.revModel=model;
			this.Id=Guid.NewGuid().ToString(); // use property so directory is updated
			this.Parent=parent; // use property so noParents is updated
			this.notes="";

			this.children=new Dictionary<string, Identifiable>();
			this.collection=new Dictionary<string, List<Identifiable>>();
			this.properties=new Dictionary<string, object>();
			this.propertyNotes=new Dictionary<string, string>();

			foreach(string child in revModel.Children){
				this.children.Add(child,new Identifiable(this,revModel.GetChildModel(child)));
			}
			foreach(string collection in revModel.Collection){
				this.collection.Add(collection,new List<Identifiable>());
			}
			foreach(KeyValuePair<string,string> kvp in revModel.Properties){
				this.properties.Add(kvp.Key,revModel.GetPropertyType(kvp.Key).DefaultValue);
				this.propertyNotes.Add(kvp.Key,"");
			}

			if(parent!=null&&Array.IndexOf(parent.GetModelInfo().Collection,model.Id)>-1){
				parent.AddGroupElement(model.Id,this);
			}
		}

		public RevModel GetModelInfo()
		{
			return this.revModel;
		}

		public RevType GetTypeInfo(string propertyName)
		{
			return this.revModel.GetPropertyType(propertyName);
		}

		public Identifiable Child(string name)
		{
			return this.children[name];
		}

		public List<Identifiable> Group(string name)
		{
			return this.collection.ContainsKey(name)?this.collection[name]:null;
		}

		public void AddGroupElement(string groupName,Identifiable element)
		{
			this.collection[groupName].Add(element);
		}

		public void RemoveGroupElement(string groupName,Identifiable element)
		{
			if(this.collection.ContainsKey(groupName)&&this.collection[groupName].Contains(element)){
				this.collection[groupName].Remove(element);
			}
		}

		public object Property(string name)
		{
			return this.properties[name];
		}

		public void SetProperty(string name,object value)
		{
			this.properties[name]=value;
		}

		public string PropertyNote(string name)
		{
			return this.propertyNotes[name];
		}

		public void SetPropertyNote(string name,string value)
		{
			this.propertyNotes[name]=value??"";
		}

		/// <summary>
		/// Flatten this instance's children for serialization
		/// </summary>
		public List<Identifiable> Flatten()
		{
			List<Identifiable> flattened=new List<Identifiable>();
			flattened.Add(this);
			foreach(KeyValuePair<string,Identifiable> kvp in this.children){
				foreach(Identifiable child in kvp.Value.Flatten()){
					flattened.Add(child);
				}
			}
			foreach(KeyValuePair<string,List<Identifiable>> kvp in this.collection){
				foreach(Identifiable collectionObject in kvp.Value){
					foreach(Identifiable child in collectionObject.Flatten()){
						flattened.Add(child);
					}
				}
			}
			return flattened;
		}

		/// <summary>
		/// Delete this instance and all children.
		/// </summary>
		public void Delete()
		{
			foreach(KeyValuePair<string,Identifiable> kvp in this.children){
				kvp.Value.Delete();
			}
			foreach(KeyValuePair<string,List<Identifiable>> kvp in this.collection){
				foreach(Identifiable collectionObject in kvp.Value){
					collectionObject.Delete();
				}
			}
			if(this.parent!=null){
				this.parent.RemoveGroupElement(this.revModel.Id,this);
			}
			this.Deregister();
		}

		/// <summary>
		/// Assign this instance to a new parent.
		/// </summary>
		/// <param name="newParent">New parent.</param>
		public void Reparent(Identifiable newParent)
		{
			if(this.parent!=null){
				this.parent.collection[this.GetModelInfo().Id].Remove(this);
			}
			this.Parent=newParent;
			if(this.parent!=null){
				this.parent.collection[this.GetModelInfo().Id].Add(this);
			}
		}

		public override string ToString()
		{
			return this.Title;
		}
	}
}

