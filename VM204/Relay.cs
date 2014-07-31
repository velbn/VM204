using System;

namespace VM204
{
	public class Relay
	{
		private bool state;

		public Relay(bool state = false)
		{
			this.state = state;
		}

		public bool IsOn()
		{
			return (state == true);
		}

		public bool IsOff()
		{
			return (state == false);
		}

		public void SwitchOn()
		{
			state = true;
		}

		public void SwitchOff()
		{
			state = false;
		}

		public void Toggle()
		{
			state = !state;
		}
	}
}

