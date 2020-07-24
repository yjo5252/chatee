using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    public class UserInfoManager
    {

        public static int keyNum { get; set; } //고유번호

        public static string UserName { get; set; } //유저 이름

        public static int PreWeight { get; set; } //현재 체중

        public static int PostWeight { get; set; } //목표 체중

        public static string SkillLevel { get; set; } //숙련도

        public static string Area { get; set; } //부위

        public static string Category { get; set; } //종류

        public static int ConversationCount { get; set; }

        public static string AvatarName { get; set; } //캐릭터 이름

        public static int AvatarState { get; set; } //아바타 상태

    }
}
