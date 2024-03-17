using System;

namespace Game.Modules.Board.WeightedBall
{
    [Serializable]
    public class WeightedBall
    {
        public int Number;
        public float Weight;

        public WeightedBall(int number, float weight)
        {
            Number = number;
            Weight = weight;
        }
    }
}