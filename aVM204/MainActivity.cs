using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Content.PM;
using VM204;
using VM204.Core;
using System.Threading.Tasks;


namespace aVM204
{
    //[Activity (Label = "VM204", MainLauncher = true, Icon = "@drawable/icon",ScreenOrientation = ScreenOrientation.Portrait)]
    [Activity(Label = "VM204", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        ListView list;
        RelayCardInfoAdapter adapter;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
           

            list = FindViewById<ListView>(Resource.Id.RelayListView);

            adapter = new RelayCardInfoAdapter(this, RelayCardInfoManager.GetAllRelayCardInfos().ToList());
            adapter.ImageButtonClicked += (object sender, ImageButtonClickedEventArgs e) =>
            {
                Intent intent = new Intent(this, typeof(EditActivity));
                RelayCardInfo relayCardInfo = adapter[e.Position];
                intent.PutExtra("ID", relayCardInfo.ID);
                StartActivity(intent);
            };
            list.Adapter = adapter;
            list.DescendantFocusability = DescendantFocusability.BlockDescendants;
            list.ItemClick += OnListItemClick;
            Populate();           
        }

        protected override void OnResume()
        {
            base.OnResume();
            Populate();

        }

        protected override void OnStop()
        {
            base.OnStop();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }       

        private void Populate()
        {

            adapter.items = RelayCardInfoManager.GetAllRelayCardInfos().ToList();
            adapter.NotifyDataSetChanged();

        }

        protected void OnListItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            RelayCardInfo relayCardInfo = adapter[e.Position];

            Intent intent = new Intent(this, typeof(ControlActivity));

            intent.PutExtra("ID", relayCardInfo.ID);
            StartActivity(intent);

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu, menu);
            IMenuItem btnAddRelayCardInfo = menu.FindItem(Resource.Id.action_add);
            //btnAddRelayCardInfo.
            return base.OnCreateOptionsMenu(menu);
        }


        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_add:
                    StartAddRelayCardInfoActivity();
                    return true;
                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        private void StartAddRelayCardInfoActivity()
        {
            Intent intent = new Intent(this, typeof(EditActivity));

            intent.PutExtra("ID", 0);
            
            StartActivity(intent);
        }
    }
}



