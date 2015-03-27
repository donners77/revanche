using System;
using System.Collections.Generic;

namespace Revanche.Lib
{
	/// <summary>
	/// Interface for an object that provides persistence
	/// </summary>
	public interface Loader<T>
	{
		List<T> Load(string uri);

		void Save(List<T> types,string uri);
	}
}

