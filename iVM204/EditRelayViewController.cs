using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using VM204;
using System.Drawing;

namespace iVM204
{
    partial class EditRelayViewController : UIViewController
    {
        RelayCardInfo currentInfo;
        private UIView activeview;             // Controller that activated the keyboard
        private float scroll_amount = 0.0f;    // amount to scroll 
        private float bottom = 0.0f;           // bottom point
        private float offset = 10.0f;          // extra offset
        private bool moveViewUp = false;           // which direction are we moving

        public EditRelayViewController(IntPtr handle)
            : base(handle)
        {

        }



        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (currentInfo != null)
            {
                Populate();
            }

            this.NavigationItem.SetRightBarButtonItem(
                new UIBarButtonItem(UIBarButtonSystemItem.Refresh, (sender, args) =>
                {
                    Scan();
                    // button was clicked
                })
                , true);

        }

        private void Populate()
        {
            this.txtName.Text = currentInfo.Name;
            this.txtUser.Text = currentInfo.Username;
            this.txtPass.Text = currentInfo.Password;
            this.txtIp.Text = currentInfo.Ip;
            this.txtPort.Text = Convert.ToString(currentInfo.Port);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Keyboard popup
            NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.DidShowNotification, KeyBoardUpNotification);

            // Keyboard Down
            NSNotificationCenter.DefaultCenter.AddObserver
            (UIKeyboard.WillHideNotification, KeyBoardDownNotification);

            this.txtName.ShouldReturn += TextFieldShouldReturn;
            this.txtUser.ShouldReturn += TextFieldShouldReturn;
            this.txtPass.ShouldReturn += TextFieldShouldReturn;
            this.txtIp.ShouldReturn += TextFieldShouldReturn;
            this.txtPort.ShouldReturn += TextFieldShouldReturn;

            btnSave.TouchUpInside += (sender, ea) =>
            {
                UpdateCurrentInfo();
                RelayCardInfoManager.SaveRelayCardInfo(currentInfo);
            };
        }



        private void KeyBoardUpNotification(NSNotification notification)
        {
            // get the keyboard size
            RectangleF r = UIKeyboard.BoundsFromNotification(notification);

            // Find what opened the keyboard
            foreach (UIView view in this.View.Subviews)
            {
                if (view.IsFirstResponder)
                    activeview = view;
            }

            // Bottom of the controller = initial position + height + offset      
            bottom = (activeview.Frame.Y + activeview.Frame.Height + offset);

            // Calculate how far we need to scroll
            scroll_amount = (r.Height - (View.Frame.Size.Height - bottom));

            // Perform the scrolling
            if (scroll_amount > 0)
            {
                moveViewUp = true;
                ScrollTheView(moveViewUp);
            }
            else
            {
                moveViewUp = false;
            }

        }

        private void UpdateCurrentInfo()
        {
            currentInfo.Name = this.txtName.Text;
            currentInfo.Username = this.txtUser.Text;
            currentInfo.Password = this.txtPass.Text;
            currentInfo.Ip = this.txtIp.Text;
            currentInfo.Port = Convert.ToInt32(this.txtPort.Text);
        }

        private void KeyBoardDownNotification(NSNotification notification)
        {
            if (moveViewUp) { ScrollTheView(false); }
        }


        private void ScrollTheView(bool move)
        {

            // scroll the view up or down
            UIView.BeginAnimations(string.Empty, System.IntPtr.Zero);
            UIView.SetAnimationDuration(0.3);

            RectangleF frame = View.Frame;

            if (move)
            {
                frame.Y -= scroll_amount;
            }
            else
            {
                frame.Y += scroll_amount;
                scroll_amount = 0;
            }

            View.Frame = frame;
            UIView.CommitAnimations();
        }

        private bool TextFieldShouldReturn(UITextField tf)
        {
            tf.ResignFirstResponder();
            if (moveViewUp) { ScrollTheView(false); }
            return true;
        }

        public void SetRelayCardInfo(RelayCardInfo info)
        {
            currentInfo = info;
        }

        public void Scan()
        {
            
            var detail = this.Storyboard.InstantiateViewController("Scan") as ScanViewController;
            //detail.SetRelayCardInfo(newRelayCardInfo);
            this.NavigationController.PushViewController(detail, true);
        }



    }
}
