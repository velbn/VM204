using System;
using System.Collections.Generic;

namespace VM204
{
	public class InputsChangedEventArgs : EventArgs
	{
		public readonly List<Input> Inputs;
		public InputsChangedEventArgs (List<Input> inputs)
		{
			Inputs = inputs;
		}
	}
}

