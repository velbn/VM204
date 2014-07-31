using System;

namespace aVM204
{
	public class ImageButtonClickedEventArgs : EventArgs
	{
		public readonly int Position;

		public ImageButtonClickedEventArgs (int position)
		{
			Position = position;
		}
	}
}

