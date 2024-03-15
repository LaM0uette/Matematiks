using System;
using Game.Modules.Board.Balls;

namespace Game.Modules.Board
{
    public static class BoardEvents
    {
        public static Action<Ball> CurrentBallSelectedEvent;
    }
}
