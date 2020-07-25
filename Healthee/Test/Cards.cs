using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace Test
{
    public class Cards
    {
        public static Attachment CreateAdaptiveCardAttachment(string cardname)
        {
            // combine path for cross platform support
            var paths = new[] { ".", "Resources", cardname };

            var adaptiveCardJson = File.ReadAllText(Path.Combine(paths));

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };

            return adaptiveCardAttachment;
        }
    }
}
