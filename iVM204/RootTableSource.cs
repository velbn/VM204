using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using VM204;


namespace iVM204
{
	public class RootTableSource : UITableViewSource {

		// there is NO database or storage of Tasks in this example, just an in-memory List<>
		RelayCardInfo[] tableItems;
		string cellIdentifier = "taskcell"; // set in the Storyboard
        private MasterViewController mController;
        public RootTableSource(RelayCardInfo[] items,MasterViewController parentController)
		{
			tableItems = items;
            mController = parentController;
		}
		public override int RowsInSection (UITableView tableview, int section)
		{
			return tableItems.Length;
		}
		public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			// in a Storyboard, Dequeue will ALWAYS return a cell, 
			UITableViewCell cell = tableView.DequeueReusableCell (cellIdentifier);
			// now set the properties as normal
			cell.TextLabel.Text = tableItems[indexPath.Row].Name;
            //if (tableItems[indexPath.Row].Done) 
            //    cell.Accessory = UITableViewCellAccessory.Checkmark;
            //else
            //    cell.Accessory = UITableViewCellAccessory.None;
			return cell;
		}
        public RelayCardInfo GetItem(int id)
        {
			return tableItems[id];
		}

        /// <summary>
        /// Called when the DetailDisclosureButton is touched.
        /// Does nothing if DetailDisclosureButton isn't in the cell
        /// </summary>
        public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
        {
            new UIAlertView("Touchy TOUCH"
                , tableItems[indexPath.Row].Name, null, "OK", null).Show();
            var newRelayCardInfo = tableItems[indexPath.Row];
            // then open the detail view to edit it
            var detail = mController.Storyboard.InstantiateViewController("Edit") as EditRelayViewController;
            detail.SetRelayCardInfo(newRelayCardInfo);
            mController.NavigationController.PushViewController(detail, true);
        }
	}
}

