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
       
        float scrollamount = 0.0f;
        float bottomPoint = 0.0f;
        bool moveViewUp = false;
        public float yOffset = 0.1f;
		public EditRelayViewController (IntPtr handle) : base (handle)
		{
          
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.txtName.ShouldReturn += TextFieldShouldReturn;
            this.txtUser.ShouldReturn += TextFieldShouldReturn;
            this.txtPassword.ShouldReturn += TextFieldShouldReturn;
            this.txtIp.ShouldReturn += TextFieldShouldReturn;
            this.txtPort.ShouldReturn += TextFieldShouldReturn;

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

        private void KeyboardUpNotification(NSNotification notification)
        {
            ResetTheView();

            RectangleF r = UIKeyboard.BoundsFromNotification(notification);

            if (this.txtIp.IsEditing)
            {
                //Calculate the bottom of the Texbox
                //plus a small margin...
                bottomPoint = (this.txtIp.Frame.Y + this.txtIp.Frame.Height + yOffset);

                //Calculate the amount to scroll the view
                //upwards so the Textbox becomes visible...
                //This is the height of the Keyboard -
                //(the height of the display - the bottom
                //of the Texbox)...	
                scrollamount = (r.Height - (View.Frame.Size.Height - bottomPoint));
            }

            //Check to see whether the view
            //should be moved up...
            if (scrollamount > 0)
            {
                moveViewUp = true;
                ScrollTheView(moveViewUp);
            }
            else moveViewUp = false;
        }

        private void ResetTheView()
        {
            UIView.BeginAnimations(string.Empty, System.IntPtr.Zero);
            UIView.SetAnimationDuration(0.3);

            RectangleF frame = View.Frame;
            frame.Y = 0;
            View.Frame = frame;
            UIView.CommitAnimations();
        }

        private void ScrollTheView(bool movedUp)
        {
            //To invoke a views built-in animation behaviour,
            //you create an animation block and
            //set the duration of the move...
            //Set the display scroll animation and duration...
            UIView.BeginAnimations(string.Empty, System.IntPtr.Zero);
            UIView.SetAnimationDuration(0.3);

            //Get Display size...
            RectangleF frame = View.Frame;

            if (movedUp)
            {
                //If the view should be moved up,
                //subtract the keyboard height from the display...
                frame.Y -= scrollamount;
            }
            else
            {
                //If the view shouldn't be moved up, restore it
                //by adding the keyboard height back to the original...
                frame.Y += scrollamount;
            }

            //Assign the new frame to the view...
            View.Frame = frame;

            //Tell the view that your all done with setting
            //the animation parameters, and it should
            //start the animation...
            UIView.CommitAnimations();

        }

       
    }
}
