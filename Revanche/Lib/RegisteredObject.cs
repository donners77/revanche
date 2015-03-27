using System;
using System.Collections.Generic;

namespace Revanche
{
	/// <summary>
	/// Object which manages its own instance references - going out of scope will not cause the object to be destroyed
	/// </summary>
	public class RegisteredObject
	{
		private static Dictionary<string,RegisteredObject> directory=new Dictionary<string, RegisteredObject>();
		private string id;

		/// <summary>
		/// Unique identifier for this object.
		/// </summary>
		/// <value>The identifier.</value>
		public string Id
		{
			get{
				return this.id;
			}
			set{
				if(value!=null&&directory.ContainsKey(value)){
					throw new InvalidOperationException("Object name collision: '"+value+"' is already defined.");
				}
				if(this.id!=null&&directory.ContainsKey(this.id)){
					directory.Remove(this.id);
				}
				this.id=value;
				directory.Add(this.id,this);
			}
		}

		/// <summary>
		/// Remove this instance from the instance registry.
		/// </summary>
		protected void Deregister()
		{
			directory.Remove(this.id);
			this.id=null;
		}

		/// <summary>
		/// Fetch a particular object using its Id attribute.
		/// </summary>
		/// <returns>The registered object.</returns>
		/// <param name="name">Name.</param>
		public static RegisteredObject GetRegisteredObject(string name)
		{
			if(directory.ContainsKey(name)){
				return directory[name];
			} else
				throw new ArgumentException("Not a registered object");
		}

		public RegisteredObject()
		{
		}

		public RegisteredObject(string name)
		{
			this.Id=name; // Use property so directory is updated
		}

		/// <summary>
		/// Execute a delegate over the objects in the registry. Safer than providing access to the registry.
		/// </summary>
		/// <param name="function">Function.</param>
		public static void Iterate(Action<RegisteredObject> function)
		{
			foreach(KeyValuePair<string,RegisteredObject> kvp in directory){
				function(kvp.Value);
			}
		}
	}
}

