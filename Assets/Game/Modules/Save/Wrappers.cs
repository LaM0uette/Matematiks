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
            public int value;
        }
        
        [Serializable]
        public class BoolWrapper
        {
            public bool value;
        }
        
        [Serializable]
        public class IntListWrapper
        {
            public List<int> list;
        }
        
        [Serializable]
        public class WeightedBallListWrapper
        {
            public List<WeightedBall> list;
        }
    }
}