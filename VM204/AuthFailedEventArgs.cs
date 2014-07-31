using System;

namespace VM204
{
	public class AuthFailedEventArgs : EventArgs
	{
		public readonly string Message;
		public AuthFailedEventArgs (string message)
		{
			Message = message;
		}
	}
}

