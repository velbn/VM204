using System;

namespace VM204
{
	public class PacketEventArgs : EventArgs
	{
		public readonly Packet packet;

		public PacketEventArgs (Packet packet)
		{
			this.packet = packet;
		}
	}
}

