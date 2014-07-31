using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;

namespace VM204
{
	public class DiscoveryBuilder
	{
		public DiscoveryBuilder ()
		{
		}

		public Discovery tryParseDiscoveryMessage (string message, IPEndPoint endpoint)
		{
			Discovery discovery = new Discovery ();
			try {
				discovery.Address = endpoint.Address;
				StringReader strReader = new StringReader (message);
				discovery.Hostname = strReader.ReadLine ();
				discovery.MacAddress = PhysicalAddress.Parse (strReader.ReadLine ());
				discovery.Info = strReader.ReadLine ();
			} catch (Exception e) {
				Console.WriteLine (e.Message);
				return null;
			}
			if (discovery.Info != null)
				return discovery;
			else
				return null;
		}
	}
}

