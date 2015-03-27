using System;
using System.Collections.Generic;

namespace Revanche.Types
{
	public class BasicTimestampType:RevType
	{
		public BasicTimestampType():base("TIMESTAMP",0L,Content.NUMERIC,null)
		{
		}
	}
}

