using System;
using System.Collections.Generic;
using System.Text;

namespace VM204
{
    public class RelayCard
    {
        private List<Relay> relays;
        private List<Input> inputs;
        public RelayCard(int amountOfRelays, int amountOfInputs)
        {
            relays = new List<Relay>();
            for (int i = 0; i < amountOfRelays; i++)
            {
                Relay relay = new Relay();
                // prop
                Relays.Add(relay);
            }

            inputs = new List<Input>();
            for (int i = 0; i < amountOfInputs; i++)
            {
                Input input = new Input();
                inputs.Add(input);
            }

        }

        public List<Relay> Relays
        {
            get
            {
                return this.relays;
            }
        }

        public List<Input> Inputs
        {
            get
            {
                return this.inputs;
            }
        }
    }
}
