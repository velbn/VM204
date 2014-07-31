using System;

namespace VM204
{
    public delegate void PacketEventHandler(object source, PacketEventArgs args);

    public abstract class Connection : IConnection
    {
        public event PacketEventHandler PacketReceived;
        public event PacketEventHandler PacketSent;

        public event EventHandler ConnectionOpened;
        public event EventHandler ConnectionClosed;

        public Connection()
        {

        }

        public void OnConnectionOpened()
        {
            if (ConnectionOpened != null) ConnectionOpened(this, null);
        }

        public void OnConnectionClosed()
        {
            if (ConnectionClosed != null) ConnectionClosed(this, null);
        }

        public void OnPacketReceive(PacketEventArgs args)
        {
            if (PacketReceived != null) PacketReceived(this, args);
        }

        public void OnPacketSent(PacketEventArgs args)
        {
            if (PacketSent != null) PacketSent(this, args);
        }

        public abstract void WriteLine(String data);

        public abstract string ReadLine();

        public abstract void Close();

        public abstract void Open();

        public abstract bool Connected();

        public abstract void ReadEverything();
    }
}