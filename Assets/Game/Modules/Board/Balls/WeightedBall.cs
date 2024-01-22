namespace Game.Modules.Board.Balls
{
    public struct WeightedBall
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