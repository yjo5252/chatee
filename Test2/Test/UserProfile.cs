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

        
        public string UserName { get; set; } //유저 이름

        public string AvatarName { get; set; } //캐릭터 이름

        public string Target { get; set; } //운동 종류

        public string KnowHow { get; set; } //숙련도

        public int PreWeight { get; set; } //현재 체중

        public int PostWeight { get; set; } //목표 체중

        public string Date { get; set; } //목표 기간

        public string AlarmSetup { get; set; } //알람 설정 여부

        public int DidTutorial { get; set; } //튜토리얼 했는지



    }
}
