using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace VM204
{

    public class DiscoveryTool
    {
        public event EventHandler<DiscoveryReceivedEventArgs> DiscoveryReceived;

        UdpListener server;
        DiscoveryBuilder discoveryBuilder;
        UdpClient client;
        public DiscoveryTool()
        {
            server = new UdpListener();
            discoveryBuilder = new DiscoveryBuilder();
            IPEndPoint broadcastAddress = new IPEndPoint(IPAddress.Any, 30303);
            client = new UdpClient(broadcastAddress);

        }

        public void FindDevice()
        {
            try
            {

                client.EnableBroadcast = true;
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 30303);
                byte[] bytes = Encoding.ASCII.GetBytes("Discovery: Who is out there?\r\n");
                IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 30303);
                client.Send(bytes, bytes.Length, ip);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }

        public void StartListiningForDiscovery()
        {
            UdpState state = new UdpState(client, new IPEndPoint(IPAddress.Any, 30303));
            client.BeginReceive(new AsyncCallback(DataReceived), state);
        }

        private void DataReceived(IAsyncResult ar)
        {
            try
            {
                UdpClient c = (UdpClient)((UdpState)ar.AsyncState).c;
                IPEndPoint wantedIpEndPoint = (IPEndPoint)((UdpState)(ar.AsyncState)).e;
                IPEndPoint receivedIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                if (c != null)
                {
                    Byte[] receiveBytes = c.EndReceive(ar, ref receivedIpEndPoint);


                    string data = Encoding.ASCII.GetString(receiveBytes);
                    Discovery discovery = discoveryBuilder.tryParseDiscoveryMessage(data, receivedIpEndPoint);
                    if (discovery != null)
                        OnDiscoveryReceived(new DiscoveryReceivedEventArgs(discovery));
                    // Restart listening for udp data packages
                    c.BeginReceive(new AsyncCallback(DataReceived), ar.AsyncState);
                }
            }
            catch (Exception e)
            {

            }
        }

        public void Shutdown()
        {
            client.Close();
        }

        protected virtual void OnDiscoveryReceived(DiscoveryReceivedEventArgs e)
        {
            if (DiscoveryReceived != null)
                DiscoveryReceived(this, e);
        }
    }
}