using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Modules.Utils
{
    public static class OldSaver
    {
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
            var wrapper = new Wrappers.IntWrapper { value = value };
            var json = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FilePath(key), json);
        }
        
        public static void Save(this string key, bool value)
        {
            var wrapper = new Wrappers.BoolWrapper { value = value };
            var json = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FilePath(key), json);
        }

        public static void Save(this string key, IEnumerable<int> values)
        {
            var wrapper = new Wrappers.IntListWrapper { list = new List<int>(values) };
            var json = JsonUtility.ToJson(wrapper);
            File.WriteAllText(FilePath(key), json);
        }

        public static void Save(this string key, List<Wrappers.WeightedBallWrapper> weightedBalls)
        {
            var json = JsonUtility.ToJson(weightedBalls);
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
            var wrapper = JsonUtility.FromJson<Wrappers.IntWrapper>(json);
            return wrapper.value;
        }
        
        public static bool LoadBool(this string key)
        {
            var path = FilePath(key);
            
            if (!File.Exists(path)) 
                return false;
            
            var json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<Wrappers.BoolWrapper>(json);
            return wrapper.value;
        }

        public static List<int> LoadListInt(this string key)
        {
            var path = FilePath(key);
            
            if (!File.Exists(path)) 
                return new List<int>();
            
            var json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<Wrappers.IntListWrapper>(json);
            return wrapper.list;
        }

        public static IEnumerable<Wrappers.WeightedBallWrapper> LoadWeightedBallWrappers(this string key)
        {
            var path = FilePath(key);
            
            if (!File.Exists(path)) 
                return new List<Wrappers.WeightedBallWrapper>();
            
            var json = File.ReadAllText(path);
            var weightedBalls = JsonUtility.FromJson<List<Wrappers.WeightedBallWrapper>>(json);
            return weightedBalls;
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