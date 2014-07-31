// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace iVM204
{
	[Register ("EditRelayViewController")]
	partial class EditRelayViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView Edit { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField txtIp { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField txtName { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField txtPassword { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField txtPort { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField txtUser { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (Edit != null) {
				Edit.Dispose ();
				Edit = null;
			}
			if (txtIp != null) {
				txtIp.Dispose ();
				txtIp = null;
			}
			if (txtName != null) {
				txtName.Dispose ();
				txtName = null;
			}
			if (txtPassword != null) {
				txtPassword.Dispose ();
				txtPassword = null;
			}
			if (txtPort != null) {
				txtPort.Dispose ();
				txtPort = null;
			}
			if (txtUser != null) {
				txtUser.Dispose ();
				txtUser = null;
			}
		}
	}
}
