
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Net.Sockets;
using System.Threading.Tasks;

namespace VM204
{
	abstract class UdpBase
	{
		protected UdpClient Client;

		protected UdpBase()
		{
		}

		public async Task<Received> Receive()
		{
			var result = await Client.ReceiveAsync();

			return new Received()
			{
				Message = Encoding.ASCII.GetString(result.Buffer, 0, result.Buffer.Length),
				Sender = result.RemoteEndPoint
			};
		}
	}
}

