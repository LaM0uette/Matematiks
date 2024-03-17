using System;
using System.Collections.Generic;
using System.IO;
using Game.Modules.Board.Balls;
using UnityEngine;

namespace Game.Modules.Utils
{
    public static class Saver
    {
        #region Wrapper

        [Serializable]
        private class IntWrapper
        {
            public int value;
        }
        
        [Serializable]
        private class BoolWrapper
        {
            public bool value;
        }
        
        [Serializable]
        private class IntListWrapper
        {
            public List<int> list;
        }
        
        [Serializable]
        private class WeightedBallListWrapper
        {
            public List<WeightedBall> list;
        }

        #endregion

        #region Statements

        public const string Gem = "Gem";
        public const string HighScore = "HighScore";
        public const string HighBall = "HighBall";
        public const string LastScore = "LastScore";
        public const string CurrentScore = "CurrentScore";
        public const string CurrentBalls = "CurrentBalls";
        public const string CurrentWeightedBalls = "CurrentWeightedBalls";
        
        public const string VolumeIsMute = "VolumeIsMute";
        
        private static string FilePath(string key) => Path.Combine(Application.persistentDataPath, $"{key}.json");

        #endregion

        #region Save

        public static void Save(this string key, int value)
        {
            var wrapper = new IntWrapper { value = value };
            var json = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FilePath(key), json);
        }
        
        public static void Save(this string key, bool value)
        {
            var wrapper = new BoolWrapper { value = value };
            var json = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FilePath(key), json);
        }

        public static void Save(this string key, IEnumerable<int> values)
        {
            var wrapper = new IntListWrapper { list = new List<int>(values) };
            var json = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FilePath(key), json);
        }

        public static void Save(this string key, List<WeightedBall> weightedBalls)
        {
            var wrapper = new WeightedBallListWrapper { list = weightedBalls };
            var json = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FilePath(key), json);
        }

        #endregion

        #region Load

        public static int LoadInt(this string key)
        {
            var path = FilePath(key);
            
            if (!File.Exists(path)) 
                return 0;
            
            var json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<IntWrapper>(json);
            return wrapper.value;
        }
        
        public static bool LoadBool(this string key)
        {
            var path = FilePath(key);
            
            if (!File.Exists(path)) 
                return false;
            
            var json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<BoolWrapper>(json);
            return wrapper.value;
        }

        public static List<int> LoadListInt(this string key)
        {
            var path = FilePath(key);
            
            if (!File.Exists(path)) 
                return new List<int>();
            
            var json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<IntListWrapper>(json);
            return wrapper.list;
        }

        public static List<WeightedBall> LoadListWeightedBall(this string key)
        {
            var path = FilePath(key);
            
            if (!File.Exists(path)) 
                return new List<WeightedBall>();
            
            var json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<WeightedBallListWrapper>(json);
            return wrapper.list;
        }

        #endregion

        #region Delete

        public static void Delete(this string key)
        {
            var path = FilePath(key);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        #endregion

        #region Reset

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