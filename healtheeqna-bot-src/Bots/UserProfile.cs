// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Schema;
using System.Collections.Generic;

namespace Microsoft.BotBuilderSamples
{
    /// <summary>
    /// This is our application state. Just a regular serializable .NET class.
    /// </summary>
    public class UserProfile
    {
      
        public string UserName { get; set; }

        public string AvatarName { get; set; }

        
        public string Target { get; set; }
        
        public string KnowHow { get; set; }
        
        public string PreWeight { get; set; }
        
        public string PostWeight { get; set; }
        
        public string Date { get; set; }


        // ======================================
        public string Transport { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public Attachment Picture { get; set; }

        // The list of companies the user wants to review.
        public List<string> CompaniesToReview { get; set; } = new List<string>();
    }
}