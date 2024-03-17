using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Modules.Utils
{
    public static class OldSaver
    {
        #region Statements

        public const string CurrentBalls = "CurrentBalls";
        public const string CurrentWeightedBalls = "CurrentWeightedBalls";
        
        private static string FilePath(string key) => Path.Combine(Application.persistentDataPath, $"{key}.json");

        #endregion

        #region Save

        public static void Save(this string key, List<Wrappers.WeightedBallWrapper> weightedBalls)
        {
            var json = JsonUtility.ToJson(weightedBalls);
            File.WriteAllText(FilePath(key), json);
        }

        #endregion

        #region Load

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
    }
}