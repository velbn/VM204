using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;

namespace VM204
{
    public class TcpConnection : Connection
    {

        public IPAddress Address { get; set; }
        private readonly Object _lock = new Object();
        public int Port { get; set; }

        public const int DefaultPort = 6000;
        private TcpClient client;
        private NetworkStream stream; //Creats a NetworkStream (used for sending and receiving data)
        StreamReader streamReader;
        TextReader textReader;
        Object _ = new Object();
        ReaderWriterLock rwl = new ReaderWriterLock();
        public TcpConnection(String address = "127.0.0.1", int port = DefaultPort)
        {
            client = new TcpClient();
            client.ReceiveTimeout = 5000;
            this.Address = IPAddress.Parse(address);
            this.Port = port;
        }

        public override string ReadLine()
        {

            try
            {
                rwl.AcquireReaderLock(500);
                if (client.Connected)
                {
                    var line = streamReader.ReadLine();
                    return line;
                }
                else
                {
                    OnConnectionClosed();
                    return null;
                }

            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                if (client.Connected)
                {
                    return null;
                }
                else
                {

                    streamReader.Close();
                    OnConnectionClosed();
                }
                return "";
            }
            catch (OutOfMemoryException ex)
            {
                return "";
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }            
            finally
            {
                rwl.ReleaseReaderLock();
            }
        }

        public override void ReadEverything()
        {
            try
            {
                if (client.GetStream().DataAvailable)
                    streamReader.ReadToEnd();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public override void WriteLine(string msg)
        {
            try
            {
                stream = client.GetStream();
                byte[] data; // creates a new byte without mentioning the size of it cuz its a byte used for sending

                data = Encoding.Default.GetBytes(msg); // put the msg in the byte ( it automaticly uses the size of the msg )

                stream.Write(data, 0, data.Length); //Sends the real data
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "Function:WriteLine");
                if (client.Client != null)
                {
                    if (client.Connected)
                    {
                        stream.Dispose();
                        //stream.Close();
                        Close();
                    }
                    else
                    {
                        OnConnectionClosed();
                    }
                }
            }
        }

        #region implemented abstract members of Connection

        public override void Close()
        {
            if (streamReader != null && client != null)
            {
                //streamReader.Close();
                client.Close();
                OnConnectionClosed();
            }
        }
        public override void Open()
        {
            // Connect using a timeout (5 seconds)

            IAsyncResult result = client.BeginConnect(Address, Port, null, null);

            bool success = result.AsyncWaitHandle.WaitOne(5000, true);

            if (client.Connected)
            {
                streamReader = new StreamReader(client.GetStream());
                //streamReader.BaseStream.ReadTimeout = 5000;
                OnConnectionOpened();
            }
            else
            {
                // NOTE, MUST CLOSE THE SOCKET

                client.Close();
                throw new ApplicationException("Failed to connect server.");
            }
        }
        public override bool Connected()
        {
            return client.Connected;
        }
        #endregion
    }

}

