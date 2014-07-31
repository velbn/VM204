using System;
using System.Collections.Generic;

namespace VM204
{
	public class OutputsChangedEventArgs:EventArgs
	{
		public readonly List<Relay> Relays;
		public OutputsChangedEventArgs (List<Relay> relays)
		{
			Relays = relays;
		}
	}
}

