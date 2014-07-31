
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Security.Cryptography;
using VM204.Core;
using Android.Content.PM;
using VM204;
using Newtonsoft.Json;

namespace aVM204
{
    [Activity(Label = "EditActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class EditActivity : Activity
    {
        RelayCardInfo relayCardInfo;
        EditText txtName, txtUserName, txtPassword, txtIp, txtPort;
        Button btnDone;
        DiscoveryTool discovery;
        private static readonly int SCAN_CARDS = 1;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.EditActivity);
            try
            {
                //				Bundle b = getIntent().getExtras();

                discovery = new DiscoveryTool();
                discovery.DiscoveryReceived += (object sender, DiscoveryReceivedEventArgs e) =>
                {

                };
                int position = Intent.GetIntExtra("ID", 0);
                if (position == 0)
                {
                    GetAllTheViews();
                    UpdateUI(null);
                    btnDone.Click += SaveRelayCardInfo;
                }
                else //Add a new One
                {
                    GetAllTheViews();
                    relayCardInfo = RelayCardInfoManager.GetRelayCardInfo(position);
                    UpdateUI(relayCardInfo);
                    btnDone.Click += SaveRelayCardInfo;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.scanmenu, menu);
            IMenuItem btnAddRelayCardInfo = menu.FindItem(Resource.Id.action_scan);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_scan:
                    startScanActivity();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            GetAllTheViews();
            UpdateUI(relayCardInfo);
        }

        private void GetAllTheViews()
        {
            txtName = FindViewById<EditText>(Resource.Id.txtName);

            txtUserName = FindViewById<EditText>(Resource.Id.txtUserName);

            txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);

            txtIp = FindViewById<EditText>(Resource.Id.txtIp);

            txtPort = FindViewById<EditText>(Resource.Id.txtPort);

            btnDone = FindViewById<Button>(Resource.Id.btnSave);

            btnDone = FindViewById<Button>(Resource.Id.btnSave);
        }

        public void SaveRelayCardInfo(object sender, EventArgs e)
        {
            relayCardInfo.Name = txtName.Text;
            relayCardInfo.Ip = txtIp.Text;
            relayCardInfo.Port = Convert.ToInt32(txtPort.Text);
            relayCardInfo.Username = txtUserName.Text;
            relayCardInfo.Password = txtPassword.Text;
            RelayCardInfoManager.SaveRelayCardInfo(relayCardInfo);
            Toast.MakeText(this, "Data saved", ToastLength.Short).Show();
            Finish();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                if (requestCode == SCAN_CARDS)
                {
                    relayCardInfo = JsonConvert.DeserializeObject<RelayCardInfo>(data.GetStringExtra("RelayCardInfo"));

                    //UpdateUI(relayCardInfo);
                }
            }
        }

        private void startScanActivity()
        {
            Intent intent = new Intent(this, typeof(ScanActivity));

            StartActivityForResult(intent, SCAN_CARDS);
        }

        private void UpdateUI(RelayCardInfo _relaycardInfo)
        {
            try
            {
                txtName.Text = _relaycardInfo.Name;
                txtUserName.Text = _relaycardInfo.Username;
                txtPassword.Text = _relaycardInfo.Password;
                txtIp.Text = _relaycardInfo.Ip;
                txtPort.Text = _relaycardInfo.Port.ToString();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Finish();
        }

    }
}


