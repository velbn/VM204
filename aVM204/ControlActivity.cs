
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Net;
using System.Text;
using Android.App;
using Android.Content;
using Android.Nfc;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using VM204.Core;
using VM204;
using Android.Util;
using System.Timers;




namespace aVM204
{
    [Activity(Label = "ControlActivity")]
    public class ControlActivity : Activity
    {
        protected TcpConnection connection;
        private System.Object lockThis = new System.Object();
        protected cVM204 card;
        RelayCardInfo relayCardInfo;
        List<CheckBox> chkInputs;
        List<Switch> swOutputs;
        Button btnInputScan;
        private ProgressDialog progressDialog = null;
        Timer timer;
        ConnectTask task;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            timer = new Timer();
            timer.Elapsed += timer_Elapsed;

            SetContentView(Resource.Layout.ControlLayout);
            chkInputs = new List<CheckBox>();
            swOutputs = new List<Switch>();

            int position = Intent.GetIntExtra("ID", 0);
            relayCardInfo = RelayCardInfoManager.GetRelayCardInfo(position);

            var switch1 = FindViewById<Switch>(Resource.Id.switch1);
            switch1.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
            {
                if (switch1.Pressed)
                {

                    card.ToggleRelay(1);

                }

            };

            var switch2 = FindViewById<Switch>(Resource.Id.switch2);
            switch2.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
            {
                if (switch2.Pressed)
                {

                    card.ToggleRelay(2);

                }
            };

            var switch3 = FindViewById<Switch>(Resource.Id.switch3);
            switch3.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
            {
                if (switch3.Pressed)
                {

                    card.ToggleRelay(3);

                }
            };

            var switch4 = FindViewById<Switch>(Resource.Id.switch4);
            switch4.CheckedChange += (object sender, CompoundButton.CheckedChangeEventArgs e) =>
            {
                if (switch4.Pressed)
                {

                    card.ToggleRelay(4);

                }
            };

            swOutputs.Add(switch1);
            swOutputs.Add(switch2);
            swOutputs.Add(switch3);
            swOutputs.Add(switch4);

            chkInputs.Add(FindViewById<CheckBox>(Resource.Id.checkBox1));
            chkInputs.Add(FindViewById<CheckBox>(Resource.Id.checkBox2));
            chkInputs.Add(FindViewById<CheckBox>(Resource.Id.checkBox3));
            chkInputs.Add(FindViewById<CheckBox>(Resource.Id.checkBox4));

            connection = new TcpConnection();
            card = new cVM204(connection);
            card.InputsChanged += card_InputsChanged;
            card.OutputsChanged += card_OutputsChanged;

            card.AuthFailed += card_AuthFailed;
            card.NotConnected += card_NotConnected;

            task = new ConnectTask(this);
            task.Execute();
        }

        void card_InputsChanged(object sender, InputsChangedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                SetInputsCheckBoxes(e.Inputs);
            });
        }

        void card_OutputsChanged(object sender, OutputsChangedEventArgs e)
        {
            RunOnUiThread(() =>
            {
                SetOutputSwitches(e.Relays);
            });
        }

        void card_AuthFailed(object sender, AuthFailedEventArgs e)
        {
            Console.WriteLine(e.Message);
            RunOnUiThread(() =>
            {
                Toast.MakeText(this, "Failed to login on the card", ToastLength.Short).Show();
                ClearEvenetsAndFinish();
            });
        }

        void card_NotConnected(object sender, EventArgs e)
        {
            StopTimer();

            task.Cancel(true);
            task.Dispose();

            Finish();

            RunOnUiThread(() =>
            {
                Toast.MakeText(this, "Connection Lost", ToastLength.Short).Show();

            });
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            StopTimer();

            card.GetInputsStatus();

            StartTimer();
        }

        private class ConnectTask : AsyncTask
        {
            private ProgressDialog _progressDialog;

            private ControlActivity _context;

            public ConnectTask(ControlActivity activity)
            {
                _context = activity;
                _progressDialog = ProgressDialog.Show(activity, "Connecting", "Please Wait", true, true);
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);

                try
                {
                    _progressDialog.Dismiss();
                }
                catch (Exception e)
                {
                    throw;
                }

            }

            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
            {
                _context.connect();

                _context.Login();

                _context.RefreshIOInfo();

                _context.StartTimer();

                return null;
            }
        }

        private void connect()
        {
            if (relayCardInfo != null)
            {
                connection.Address = IPAddress.Parse(relayCardInfo.Ip);
                connection.Port = relayCardInfo.Port;
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    ClearEvenetsAndFinish();//End this activity
                }
            }
        }

        private void Login()
        {
            card.Login(relayCardInfo.Username, relayCardInfo.Password);

        }

        protected override void OnPause()
        {
            base.OnPause();
            card.NotConnected -= card_NotConnected;
            card.InputsChanged -= card_InputsChanged;
            card.OutputsChanged -= card_OutputsChanged;
            timer.Elapsed -= timer_Elapsed;
        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        void SetInputsCheckBoxes(List<Input> list)
        {
            for (int i = 0; i < 4; i++)
            {
                chkInputs[i].Checked = list[i].state;
            }
        }

        void SetOutputSwitches(List<Relay> list)
        {
            for (int i = 0; i < 4; i++)
            {
                swOutputs[i].Checked = list[i].IsOn();
            }
        }

        void Disconnect()
        {
            card.Quit();
        }

        void RefreshIOInfo()
        {
            try
            {
                card.GetInputsStatus();
                card.GetOutputsStatus();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (e is ArgumentOutOfRangeException)
                {
                    Console.WriteLine(e.Message + "RefreshIOInfo");
                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        Toast.MakeText(this, "Could not retrieve the IO status", ToastLength.Short).Show();
                        ClearEvenetsAndFinish();
                    });
                }

            }
        }

        void StartTimer()
        {
            timer.Start();
        }

        void StopTimer()
        {
            timer.Stop();
        }

        void ClearEvenetsAndFinish()
        {
            card.NotConnected -= card_NotConnected;
            card.InputsChanged -= card_InputsChanged;
            card.OutputsChanged -= card_OutputsChanged;
            timer.Elapsed -= timer_Elapsed;
            Finish();
        }
    }
}

