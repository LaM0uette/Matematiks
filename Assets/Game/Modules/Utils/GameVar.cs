namespace Game.Modules.Utils
{
    public static class GameVar
    {
        public static float BallDropDuration { get; private set; } = 0.2f;
        public static float BallMoveDuration { get; private set; } = 0.1f;
        
        public static float DefaultNewBallWeight { get; private set; } = 1f;
        public static float DefaultBallWeightDiviser { get; private set; } = 15f;
    }
}
