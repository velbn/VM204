using System;

namespace VM204
{
	public class Packet
	{
		public string Data{ get; set; }

		public Packet (string data)
		{
			this.Data = data;
		}
	}
}

