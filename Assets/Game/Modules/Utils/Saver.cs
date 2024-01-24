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
        
        public static void ResetBestScore()
        {
            PlayerPrefs.SetInt(BestScore, 0);
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
        
        public static void ResetMaxBall()
        {
            PlayerPrefs.SetInt(MaxBall, 1);
        }

        #endregion
        
        #region CurrentScore
        
        private const string CurrentScore = "CurrentScore";

        public static void SaveCurrentScore(int score)
        {
            PlayerPrefs.SetInt(CurrentScore, score);
        }
        
        public static int GetCurrentScore()
        {
            return PlayerPrefs.GetInt(CurrentScore, 0);
        }
        
        public static void ResetCurrentScore()
        {
            PlayerPrefs.SetInt(CurrentScore, 0);
        }

        #endregion
    }
}