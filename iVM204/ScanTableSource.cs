using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using VM204;


namespace iVM204
{
    public class ScanTableSource : UITableViewSource
    {

        // there is NO database or storage of Tasks in this example, just an in-memory List<>
        RelayCardInfo[] tableItems;
        string cellIdentifier = "taskcell"; // set in the Storyboard
        private ScanViewController mController;
        public ScanTableSource(RelayCardInfo[] items, ScanViewController parentController)
        {
            tableItems = items;
            mController = parentController;
        }
        public override int RowsInSection(UITableView tableview, int section)
        {
            return tableItems.Length;
        }
        public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
        {
            // in a Storyboard, Dequeue will ALWAYS return a cell, 
            UITableViewCell cell = tableView.DequeueReusableCell(cellIdentifier);
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
    }
}

