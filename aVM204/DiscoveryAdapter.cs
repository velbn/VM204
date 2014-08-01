using System;
using Android.Widget;
using Android.App;
using Android.Views;
using System.Collections.Generic;
using Java.Lang;
using VM204.Core;
using VM204;


namespace aVM204
{
    public class DiscoveryAdapter : BaseAdapter<RelayCardInfo>
    {
        Activity context;

        public List<RelayCardInfo> items { get; set; }

        public event EventHandler<ImageButtonClickedEventArgs> ImageButtonClicked;

        public DiscoveryAdapter(Activity context, List<RelayCardInfo> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }

        protected virtual void OnImageButtonClicked(ImageButtonClickedEventArgs e)
        {
            if (ImageButtonClicked != null)
                ImageButtonClicked(this, e);
        }

        #region implemented abstract members of BaseAdapter

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            View view = convertView; // re-use an existing view, if one is available
            if (view == null) // otherwise create a new one
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.DiscoveryRow, null);
                view.FindViewById<TextView>(Resource.Id.RelayName).Text = items[position].Name;
            }


            return view;
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        #endregion

        #region implemented abstract members of BaseAdapter

        public override RelayCardInfo this[int index]
        {
            get
            {
                return items[index];
            }
        }

        #endregion
    }
}

