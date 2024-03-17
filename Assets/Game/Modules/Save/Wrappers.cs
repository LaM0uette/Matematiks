using System;
using System.Collections.Generic;
using Game.Modules.Board.WeightedBall;

namespace Game.Modules.Save
{
    public abstract class Wrappers
    {
        [Serializable]
        public class IntWrapper
        {
            public int Value;
        }
        
        [Serializable]
        public class BoolWrapper
        {
            public bool Value;
        }
        
        [Serializable]
        public class IntListWrapper
        {
            public List<int> Values;
        }
        
        [Serializable]
        public class WeightedBallListWrapper
        {
            public List<WeightedBall> Values;
        }
    }
}