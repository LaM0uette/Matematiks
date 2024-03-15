using System;

namespace Game.Modules.Events
{
    public static class GameEvents
    {
        public static Action<int> GemEvent;
        public static Action<int> CurrentScoreEvent;
        public static Action<int> HighScoreEvent;
        public static Action<int> HighBallEvent;
    }
}
