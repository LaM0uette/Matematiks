using System;
using System.Collections.Generic;

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
        public class WeightedBallWrapper
        {
            public int Number;
            public float Weight;

            public WeightedBallWrapper(int number, float weight)
            {
                Number = number;
                Weight = weight;
            }
        }
    }
}