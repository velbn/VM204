using System;

namespace VM204
{
	public class DiscoveryReceivedEventArgs : EventArgs
	{
		public readonly Discovery Message;

		public DiscoveryReceivedEventArgs (Discovery message)
		{
			Message = message;
		}
	}
}

