using System.Collections.Generic;
using Game.Modules.Board.Balls;
using UnityEngine;

namespace Game.Modules.Utils
{
    public static class Saver
    {
        #region Keys

        public const string Gem = "Gem";
        public const string HighScore = "HighScore";
        public const string HighBall = "HighBall";
        public const string LastScore = "LastScore";
        public const string CurrentScore = "CurrentScore";
        public const string CurrentBalls = "CurrentBalls";
        public const string CurrentWeightedBalls = "CurrentWeightedBalls";
        
        #endregion

        #region Save

        public static void Save(this string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
        
        public static void Save(this string key, IEnumerable<int> values)
        {
            var parsedValue = "";
            
            foreach (var value in values)
            {
                parsedValue += value + ";";
            }
            
            PlayerPrefs.SetString(key, parsedValue);
        }
        
        public static void Save(this string key, List<WeightedBall> weightedBalls)
        {
            var json = JsonUtility.ToJson(new Serialization<List<WeightedBall>>(weightedBalls));
            PlayerPrefs.SetString(key, json);
        }

        #endregion

        #region Load

        public static int LoadInt(this string key)
        {
            return PlayerPrefs.GetInt(key, 0);
        }
        
        public static List<int> LoadListInt(this string key)
        {
            var ballsParsed = PlayerPrefs.GetString(key, "");
            var balls = new List<int>();
            
            foreach (var ballNumber in ballsParsed.Split(';'))
            {
                if (ballNumber == "") 
                    continue;
                
                balls.Add(int.Parse(ballNumber));
            }

            return balls;
        }
        
        public static List<WeightedBall> LoadListWeightedBall(this string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                var json = PlayerPrefs.GetString(key);
                var serialization = JsonUtility.FromJson<Serialization<List<WeightedBall>>>(json);
                return serialization.data;
            }

            return new List<WeightedBall>();
        }

        #endregion
        
        #region Delete

        public static void Delete(this string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        #endregion

        #region Functions
        
        public static void ResetAll()
        {
            Gem.Delete();
            HighScore.Delete();
            HighBall.Delete();
            LastScore.Delete();
            CurrentScore.Delete();
            CurrentBalls.Delete();
            CurrentWeightedBalls.Delete();
            
            HighBall.Save(1);
        }

        public static void ResetAllCurrentScores()
        {
            CurrentScore.Delete();
            CurrentBalls.Delete();
            CurrentWeightedBalls.Delete();
        }

        #endregion
    }
}