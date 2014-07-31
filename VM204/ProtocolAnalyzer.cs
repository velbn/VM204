using System;
using System.Collections.Generic;
using System.IO;
using VM204.Core;

namespace VM204
{
	public class ProtocolAnalyzer
	{
		public ProtocolAnalyzer ()
		{
		}

		public List<Input> AnalyzeInputsData(string data)
		{
			List<Input> listOfInputs = new List<Input> ();
			string[]splittedData = data.Split (' ');
			listOfInputs.Add(new Input(splittedData[2].Contains("ON")));
			listOfInputs.Add(new Input(splittedData[4].Contains("ON")));
			listOfInputs.Add(new Input(splittedData[6].Contains("ON")));
			listOfInputs.Add(new Input(splittedData[8].Contains("ON")));

			return listOfInputs;
	
		}

		public List<Relay> AnalyzeOutputsData(string data)
		{
			List<Relay> listOfRelays = new List<Relay> ();
			string[]splittedData = data.Split (' ');
			listOfRelays.Add(new Relay(splittedData[2].Contains("ON")));
			listOfRelays.Add(new Relay(splittedData[4].Contains("ON")));
			listOfRelays.Add(new Relay(splittedData[6].Contains("ON")));
			listOfRelays.Add(new Relay(splittedData[8].Contains("ON")));

			return listOfRelays;

		}
	}
}

