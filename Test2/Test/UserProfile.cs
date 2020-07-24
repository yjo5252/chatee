using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace Test
{
    public class UserProfile
    {
        //public List<string> CompaniesToReview { get; set; } = new List<string>();
        
        public string UserName { get; set; } //유저 이름

        public int PreWeight { get; set; } //현재 체중

        public int PostWeight { get; set; } //목표 체중

        public string SkillLevel { get; set; } //숙련도

        public string Area { get; set; } //부위

        public string Category { get; set; } //종류

        public int ConversationCount { get; set; }

        public string AvatarName { get; set; } //캐릭터 이름

        public int AvatarState { get; set; } //아바타 상태

    }
}
