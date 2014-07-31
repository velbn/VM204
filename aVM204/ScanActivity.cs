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
using Newtonsoft.Json;
using VM204;

namespace aVM204
{
    [Activity(Label = "ScanActivity")]
    public class ScanActivity : Activity
    {
        private static string RESULT_OK = "OK";
        private RelayCardInfoAdapter adapter;
        ListView list;
        DiscoveryTool discovery;
        Button btnScan;
        
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ScanActivity);
            // Create your application here
            list = FindViewById<ListView>(Resource.Id.scanListview);

            btnScan = FindViewById<Button>(Resource.Id.btnScan);
            btnScan.Click += (object sender, EventArgs e) => {
                adapter.items.Clear();
                adapter.NotifyDataSetChanged();
                discovery.FindDevice(); 
            };

            adapter = new RelayCardInfoAdapter(this, new List<RelayCardInfo>());
            list.Adapter = adapter;
            list.DescendantFocusability = DescendantFocusability.BlockDescendants;
            list.ItemClick += list_ItemClick;

            discovery = new DiscoveryTool();
            discovery.DiscoveryReceived += (object sender,DiscoveryReceivedEventArgs e) => {
                RelayCardInfo relayCardInfo = new RelayCardInfo();
                relayCardInfo.Name = e.Message.Hostname;
                relayCardInfo.Ip = e.Message.Address.ToString();
                relayCardInfo.Port = 6000;
                adapter.items.Add(relayCardInfo);
                RunOnUiThread(() =>
                {
                    adapter.NotifyDataSetChanged();
                });
            };
            discovery.StartListiningForDiscovery();
            discovery.FindDevice();


        }

        void list_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            RelayCardInfo relayCardInfo = adapter[e.Position];

           
            
            finishWithResult(relayCardInfo);
        }


        private void finishWithResult(Object o)
        {
            Intent data = new Intent();
            string serialized = JsonConvert.SerializeObject(o);
            data.PutExtra("RelayCardInfo", serialized);
            if (Parent == null)
            {
                this.SetResult(Result.Ok, data);
            }
            else
            {
                Parent.SetResult(Result.Ok, data);
            }
            Finish();
        }
    }
}