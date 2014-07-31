using System;
using System.Net;
using System.Net.NetworkInformation;
namespace VM204
{
	public class Discovery
	{
		public IPAddress Address { get; set;}
		public string Hostname { get; set;}
		public PhysicalAddress MacAddress { get; set;}
		public string Info { get; set;}
	}
}

