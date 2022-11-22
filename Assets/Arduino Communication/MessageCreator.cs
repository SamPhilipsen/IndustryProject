using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoControl
{
    class MessageCreator
    {
        public char EndMarker { get; private set; }
        public char StartMarker { get; private set; }
        public string PartialMessage { get; set; }
        public char PayloadMarker { get; private set; }

        public MessageCreator(char end, char start)
        {
            EndMarker = end;
            StartMarker = start;
            PartialMessage = null;
            PayloadMarker = ':';
        }

        public string Add(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("Empty message sent to MessageCreator.Add");
            }

            foreach (char character in input)
            {
                if (character != EndMarker)
                {
                    PartialMessage += character;
                }
                else
                {
                    return PartialMessage;
                }
            }
            FinalizeMessage();
            return "";
        }

        public void FinalizeMessage()
        {
            PartialMessage += EndMarker;
        }

        public void Reset()
        {
            PartialMessage = StartMarker.ToString();
        }
    }
}
