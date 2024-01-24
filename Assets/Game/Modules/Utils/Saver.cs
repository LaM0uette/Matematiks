using System.Collections.Generic;
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
        
        // CurrentScore
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
        
        // Balls
        private const string CurrentBalls = "CurrentBalls";

        public static void SaveCurrentBalls(IEnumerable<int> ballNumbers)
        {
            var ballsParsed = "";
            
            foreach (var ballNumber in ballNumbers)
            {
                ballsParsed += ballNumber + ";";
            }
            
            PlayerPrefs.SetString(CurrentBalls, ballsParsed);
        }
        
        public static List<int> GetCurrentBalls()
        {
            var ballsParsed = PlayerPrefs.GetString(CurrentBalls, "");
            var balls = new List<int>();
            
            foreach (var ballNumber in ballsParsed.Split(';'))
            {
                if (ballNumber == "") 
                    continue;
                
                balls.Add(int.Parse(ballNumber));
            }

            return balls;
        }
        
        public static void ResetCurrentBalls()
        {
            PlayerPrefs.SetString(CurrentBalls, "");
        }

        #endregion
        
    }
}