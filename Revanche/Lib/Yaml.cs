using System;
using YamlDotNet.Serialization;
using System.IO;
using System.Collections.Generic;

namespace Revanche.Lib
{
	/// <summary>
	/// Yaml persistence layer
	/// </summary>
	public class Yaml<T>:Loader<T>
	{
		private Serializer serializer;
		private Deserializer deserializer;

		public Yaml(Serializer serializer,Deserializer deserializer)
		{
			this.serializer=serializer;
			this.deserializer=deserializer;
		}

		public List<T> Load(string uri)
		{
			return this.deserializer.Deserialize<List<T>>(new StreamReader(Config.ROOT+uri+".yml"));
		}

		public void Save(List<T> types,string uri)
		{
			if(File.Exists(Config.ROOT+uri+".yml")){
				File.Delete(Config.ROOT+uri+".yml");
			}
			var streamWriter=new StreamWriter(Config.ROOT+uri+".yml",false);
			this.serializer.Serialize(streamWriter,types);
			streamWriter.Flush();
		}
	}
}

