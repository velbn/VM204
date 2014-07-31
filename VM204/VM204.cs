using System;
using System.Runtime.InteropServices;
using System.Threading;


namespace VM204
{
    public class cVM204 : RelayCard
    {
        private Connection connection;
        Object _lock = new Object();
        public event EventHandler<InputsChangedEventArgs> InputsChanged;
        public event EventHandler<OutputsChangedEventArgs> OutputsChanged;
        public event EventHandler<AuthFailedEventArgs> AuthFailed;
        public event EventHandler<EventArgs> NotConnected;
        public cVM204(Connection connection)
            : base(4, 4)
        {
            this.connection = connection;
            this.connection.ConnectionClosed += connection_ConnectionClosed;
        }

        void connection_ConnectionClosed(object sender, EventArgs e)
        {
            OnNoConnection(e);
        }

        public Connection Connection
        {
            get
            {
                return this.connection;
            }
        }

        public void ToggleRelay(int relayIndex)
        {
            lock (_lock)
            {
                SendCommand("OUTPUT " + relayIndex.ToString() + " TOGGLE");
                ReadLineFromConnection();
            }
        }

        public void Login(string username, string password)
        {
            SendCommand("AUTH " + username + " " + password);
            string message;
            message = ReadLineFromConnection();

            if (message != "200 OK")
                OnAuthFailed(new AuthFailedEventArgs(message));
        }

        public void GetOutputsStatus()
        {
            lock (_lock)
            {
                SendCommand("STATUS outputs");
                string line;
                line = ReadLineFromConnection();

                line = ReadLineFromConnection();
                while (line != "END")
                {
                    string[] parameters = line.Split(' ');
                    if (parameters[1].Contains("ON"))
                        Relays[Convert.ToInt32(parameters[0]) - 1].SwitchOn();
                    else
                        Relays[Convert.ToInt32(parameters[0]) - 1].SwitchOff();

                    line = ReadLineFromConnection();
                }
                ReadEverythingFromConnection();
                OnOutputsChanged(new OutputsChangedEventArgs(Relays));
            }
        }

        public void GetInputsStatus()
        {
            try
            {
                lock (_lock)
                {


                    SendCommand("STATUS inputs");
                    Thread.Sleep(200);
                    string line;
                    line = ReadLineFromConnection();//Read "Inputs:"
                    if (!line.Contains("404"))
                    {
                        line = ReadLineFromConnection();
                        Console.WriteLine(line);
                        int number, tries = 0;

                        while (line != "END")
                        {
                            string[] parameters = line.Split(' ');
                            Inputs[Convert.ToInt32(parameters[0]) - 1].state = parameters[1].Contains("ON");
                            line = ReadLineFromConnection();
                        }
                        ReadEverythingFromConnection();
                        OnInputsChanged(new InputsChangedEventArgs(Inputs));
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public void GetAnalogStatus()
        {
            SendCommand("STATUS analog");
        }

        private void SendCommand(string data)
        {
            connection.WriteLine(data);
        }

        protected virtual void OnInputsChanged(InputsChangedEventArgs e)
        {
            if (InputsChanged != null)
                InputsChanged(this, e);
        }

        protected virtual void OnOutputsChanged(OutputsChangedEventArgs e)
        {
            if (OutputsChanged != null)
                OutputsChanged(this, e);
        }

        protected virtual void OnAuthFailed(AuthFailedEventArgs e)
        {
            if (AuthFailed != null)
                AuthFailed(this, e);
        }

        protected virtual void OnNoConnection(EventArgs e)
        {

            if (NotConnected != null)
                NotConnected(this, new EventArgs());
        }

        string ReadLineFromConnection()
        {
            string result;

            result = connection.ReadLine();

            return result;
        }

        void ReadEverythingFromConnection()
        {
            connection.ReadEverything();
        }

        public void Quit()
        {
            if (connection.Connected())
                SendCommand("QUIT");
        }

        public void ClearAllEvents()
        {
            this.connection.ConnectionClosed -= connection_ConnectionClosed;
        }

    }
}

