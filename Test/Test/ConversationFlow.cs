using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    public class ConversationFlow
    {

        public enum Question { 
        
            Name,
            Age,
            Date, 
            None
        }

        public Question LastQuestionAsked { get; set; } = Question.None;

    }
}
