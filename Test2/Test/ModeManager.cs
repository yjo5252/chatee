using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    public class ModeManager
    {
        public static int mode = 0;

        public enum Modes
        {
            None = 0,
            Tutorial = 1,
            RecommendExercise = 2,
            Record = 3,
            RecommendFood = 4,
            RecommendEquipment = 5,
            CheckCharacterState = 6,
            SeeMyRecord = 7
        }
    }
}
