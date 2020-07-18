using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace Test
{
    public class UserProfile
    {
        public string Transport { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Attachment Picture { get; set; }

        public List<string> CompaniesToReview { get; set; } = new List<string>();

        //여기서부터

        public string UserName { get; set; }

        public string AvatarName { get; set; }


        public string Target { get; set; }

        public string KnowHow { get; set; }

        public int PreWeight { get; set; }

        public int PostWeight { get; set; }

        public string Date { get; set; }

        public string AlarmSetup { get; set; }

    }
}
