using System;
using System.Collections.Generic;

namespace Revanche.Types
{
	public class BasicBooleanType:RevType
	{
		public BasicBooleanType():base("BOOLEAN",false,Content.ENUMERATED,new List<object>(){false,true})
		{
		}
	}
}

