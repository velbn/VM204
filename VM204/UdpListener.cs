using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Net;
using System.Net.Sockets;

namespace VM204
{
    //Server
    class UdpListener : UdpBase
    {
        public UdpListener()
        {
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 30303);
            Client = new UdpClient(ipep);
        }

        public void Reply(string message, IPEndPoint endpoint)
        {
            var datagram = Encoding.ASCII.GetBytes(message);
            Client.Send(datagram, datagram.Length, endpoint);
        }

    }
}

