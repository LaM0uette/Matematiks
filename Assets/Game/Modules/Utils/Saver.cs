using UnityEngine;

namespace Game.Modules.Utils
{
    public static class Saver
    {
        
        #region BestScore
        
        private const string BestScore = "BestScore";

        public static void SaveBestScore(int score)
        {
            PlayerPrefs.SetInt(BestScore, score);
        }
        
        public static int GetBestScore()
        {
            return PlayerPrefs.GetInt(BestScore, 0);
        }

        #endregion
        
        #region BestScore
        
        private const string MaxBall = "MaxBall";

        public static void SaveMaxBall(int ballNumber)
        {
            PlayerPrefs.SetInt(MaxBall, ballNumber);
        }
        
        public static int GetMaxBall()
        {
            return PlayerPrefs.GetInt(MaxBall, 1);
        }

        #endregion
    }
}