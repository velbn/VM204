using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using VM204;
using System.Collections.Generic;

namespace iVM204
{
	public partial class ScanViewController : UITableViewController
	{
        DiscoveryTool discovery;
        List<RelayCardInfo> relayCards = new List<RelayCardInfo>();
		public ScanViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            this.NavigationItem.Title = "Scanning";
            discovery = new DiscoveryTool();
            discovery.DiscoveryReceived += (sender, dre) =>
            {
                RelayCardInfo relayCardInfo = new RelayCardInfo();
                relayCardInfo.Name = dre.Message.Hostname;
                relayCardInfo.Ip = dre.Message.Address.ToString();
                relayCardInfo.Port = 6000;
                relayCards.Add(relayCardInfo);
            };
            this.NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIBarButtonSystemItem.Search, (sender, args) =>
                {
                    discovery.FindDevice();
                    // button was clicked
                })
                , true);
            discovery.StartListiningForDiscovery();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView.Source = new ScanTableSource(relayCards.ToArray(), this);
        }
	}
}
