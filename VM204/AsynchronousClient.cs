using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace VM204
{
	public class AsynchronousClient
	{
		private Socket client;
		private byte[] data = new byte[1024];
		private int size = 1024;
		bool isConnected = false;
		public AsynchronousClient ()
		{


		}

		public void Login ()
		{
			byte[] message = Encoding.ASCII.GetBytes ("AUTH admin test");
			client.BeginSend (message, 0, message.Length, SocketFlags.None,
				new AsyncCallback (SendData), client);
		}

		public void Send (String _message)
		{
			byte[] message = Encoding.ASCII.GetBytes (_message);
			client.BeginSend (message, 0, message.Length, SocketFlags.None,
				new AsyncCallback (SendData), client);
		}

		void Disconnect ()
		{
			client.Close ();
			isConnected = false;
		}

		void Connected (IAsyncResult iar)
		{
			client = (Socket)iar.AsyncState;
			isConnected = true;
			try {
				client.EndConnect (iar);
				//conStatus.Text = "Connected to: " + client.RemoteEndPoint.ToString ();
				client.BeginReceive (data, 0, size, SocketFlags.None,
					new AsyncCallback (ReceiveData), client);
			} catch (SocketException) {
				//conStatus.Text = "Error connecting";
			}
		}

		void ReceiveData (IAsyncResult iar)
		{
			Socket remote = (Socket)iar.AsyncState;
			int recv = remote.EndReceive (iar);
			string stringData = Encoding.ASCII.GetString (data, 0, recv);
			Console.WriteLine (stringData);
		}

		void SendData (IAsyncResult iar)
		{
			Socket remote = (Socket)iar.AsyncState;
			remote.EndSend (iar);
			remote.BeginReceive (data, 0, size, SocketFlags.None,
				new AsyncCallback (ReceiveData), remote);
		
		}


	}
}

