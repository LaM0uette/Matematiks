using System;

namespace Game.Modules.Board.Balls
{
    [Serializable]
    public struct BallNumber
    {
        public int Number;
        public int Weight;
        public bool IsLocked;
    }
}
