using System;
using System.Collections.Generic;

namespace Revanche.Types
{
	/// <summary>
	/// Defines content types for UI handling. User-defined types from the structure folder
	/// will always be ENUMERATED, except for COLOR, which must be NULLABLE_ENUMERATED.
	/// </summary>
	public enum Content
	{
		ENUMERATED,
		// all user defined types as well as BOOLEAN
		NULLABLE_ENUMERATED,
		// enumerated but allow null values - VERY DANGEROUS
		FREEFORM,
		// TEXT type only
		NUMERIC
		// INTEGER, TIMESTAMP, and DECIMAL types only
	}

	/// <summary>
	/// Main class which stores type information for property fields.
	/// </summary>
	public class RevType : RegisteredObject
	{
		private object defaultValue;
		private Content contentType;
		private List<object> values;
		private Dictionary<string,object> namedValues;

		/// <summary>
		/// Initializes built-in types.
		/// </summary>
		public static void SetupBasicTypes()
		{
			new BasicBooleanType();
			new BasicDecimalType();
			new BasicIntegerType();
			new BasicTextType();
			new BasicTimestampType();
		}

		/// <summary>
		/// Gets a type with a specific Id
		/// </summary>
		/// <returns>The registered type.</returns>
		/// <param name="name">Name.</param>
		public static RevType GetRegisteredType(string name)
		{
			return RevType.GetRegisteredObject(name) as RevType;
		}

		/// <summary>
		/// Default value for this type. Required!
		/// </summary>
		/// <value>The default value.</value>
		public object DefaultValue
		{
			get{
				return this.defaultValue;
			}
			set{
				defaultValue=value;
			}
		}

		/// <summary>
		/// Content type of this type (probably Enumerated)
		/// </summary>
		/// <value>The type of the content.</value>
		[YamlDotNet.Serialization.YamlIgnore]
		public Content ContentType
		{
			get{
				return this.contentType;
			}
			set{
				contentType=value;
			}
		}

		/// <summary>
		/// Values this type may take. Normally what you want.
		/// </summary>
		/// <value>The values.</value>
		public object[] Values
		{
			get{
				return this.values.ToArray();
			}
			set{
				this.values=new List<object>(value);
			}
		}

		/// <summary>
		/// In the rare instance that values must have a UI value which corresponds to a separate backend value,
		/// values go in here. Currently only used for COLOR, probably would not be useful unless you plan to
		/// hack on the source code.
		/// </summary>
		/// <value>The named values.</value>
		public Dictionary<string,object> NamedValues
		{
			get{
				return this.namedValues;
			}
			set{
				this.namedValues=value;
			}
		}

		/// <summary>
		/// Persistence constructor
		/// </summary>
		public RevType()
		{
			this.values=new List<object>();
		}

		/// <summary>
		/// This is the constructor you want.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <param name="contentType">Content type.</param>
		/// <param name="values">Values.</param>
		public RevType(string name,object defaultValue,Content contentType,List<object> values):base(name)
		{
			this.defaultValue=defaultValue;
			this.contentType=contentType;
			this.values=values;
			if((defaultValue==null||defaultValue.ToString()=="")&&contentType==Content.ENUMERATED){
				throw new Exception("Enumerated content types require a default: "+name);
			}
		}
	}
}

