using System;

namespace Game.Modules.Board.Balls
{
    public static class BallEvents
    {
        public static Action<Ball> CurrentBallSelectedEvent;
        public static Action ReleaseEvent;
    }
}
