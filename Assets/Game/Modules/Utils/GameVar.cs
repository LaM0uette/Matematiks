namespace Game.Modules.Utils
{
    public static class GameVar
    {
        #region SceneName

        public static string GameScene { get; private set; } = "Game";
        public static string MenuScene { get; private set; } = "Menu";

        #endregion

        #region BallProperties

        public static float BallDropDuration { get; private set; } = 0.2f;
        public static float BallMoveDuration { get; private set; } = 0.1f;

        #endregion

        #region BallWeightProperties

        public static float DefaultNewBallWeight { get; private set; } = 1f;
        public static float DefaultBallWeightDiviser { get; private set; } = 15f;

        #endregion
    }
}
