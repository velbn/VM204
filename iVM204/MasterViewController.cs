using System;
using System.Drawing;
using System.Collections.Generic;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using VM204;

namespace iVM204
{
    public partial class MasterViewController : UITableViewController
    {

        List<RelayCardInfo> list = new List<RelayCardInfo>();

        public MasterViewController(IntPtr handle)
            : base(handle)
        {
            Title = NSBundle.MainBundle.LocalizedString("Master", "Master");

            if (UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad)
            {
                ContentSizeForViewInPopover = new SizeF(320f, 600f);
                ClearsSelectionOnViewWillAppear = false;
            }

            // Custom initialization
        }

        void AddNewItem(object sender, EventArgs args)
        {
            //dataSource.Objects.Insert(0, DateTime.Now);

            using (var indexPath = NSIndexPath.FromRowSection(0, 0))
                TableView.InsertRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Automatic);
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            NavigationItem.LeftBarButtonItem = EditButtonItem;


            NavigationItem.RightBarButtonItem = addButton;
            var info = RelayCardInfoManager.GetAllRelayCardInfos();
            if(info == null)
            {
                list = new List<RelayCardInfo>();
            }
            else
            {
                list = info as List<RelayCardInfo>;
            }
            TableView.Source = new RootTableSource(list.ToArray(),this);
            //RelayCardInfo info = new RelayCardInfo();
            //info.Name = "VM204";
            //info.Password = "test";
            //info.Username = "admin";
            //info.Ip = "192.168.169.183";
            //RelayCardInfoManager.SaveRelayCardInfo(info);

            addButton.Clicked += (sender, e) =>
            {
                AddRelayCard();
            };
        }

        private void Populate()
        {

            list = RelayCardInfoManager.GetAllRelayCardInfos() as List<RelayCardInfo>;


        }

        private void AddRelayCard()
        {
            // first, add the task to the underlying data
            //var newId = 0;
            //if (list.Count != 0)
            //    newId = list[list.Count - 1].ID + 1;
            //else
            //    newId = list[0].ID + 1;
            //var newRelayCardInfo = new RelayCardInfo() { ID = newId };
            //list.Add(newRelayCardInfo);
            var newRelayCardInfo = new RelayCardInfo();
            // then open the detail view to edit it
            var detail = Storyboard.InstantiateViewController("Edit") as EditRelayViewController;
            detail.SetRelayCardInfo(newRelayCardInfo);
            NavigationController.PushViewController(detail, true);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            // bind every time, to reflect deletion in the Detail view

        }
   

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            //if (segue.Identifier == "TaskSegue")
            //{ // set in Storyboard
            //    var navctlr = segue.DestinationViewController as DetailViewController;
            //    if (navctlr != null)
            //    {
            //        var source = TableView.Source as RootTableSource;
            //        var rowPath = TableView.IndexPathForSelectedRow;
            //        var item = source.GetItem(rowPath.Row);
            //        navctlr.SetDetailItem(item); // to be defined on the TaskDetailViewController
            //    }
            //}
        }
    }
}

