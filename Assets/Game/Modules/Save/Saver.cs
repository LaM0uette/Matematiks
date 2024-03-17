using System.Collections.Generic;
using Game.Modules.Save.Savers;

namespace Game.Modules.Save
{
    public static class Saver
    {
        #region Statements

        private static readonly Dictionary<string, ISaver<int>> _intSavers = new();
        private static readonly Dictionary<string, ISaver<bool>> _boolSavers = new();
        private static readonly Dictionary<string, ISaver<IEnumerable<int>>> _intListSavers = new();
        
        // IntSavers
        public static ISaver<int> Gem => _intSavers["Gem"];
        public static ISaver<int> HighScore => _intSavers["HighScore"];
        public static ISaver<int> HighBall => _intSavers["HighBall"];
        public static ISaver<int> LastScore => _intSavers["LastScore"];
        public static ISaver<int> CurrentScore => _intSavers["CurrentScore"];
        
        // BoolSavers
        public static ISaver<bool> VolumeMuted => _boolSavers["VolumeMuted"];

        // IntListSavers
        public static ISaver<IEnumerable<int>> CurrentBalls => _intListSavers["CurrentBalls"];
        public static ISaver<IEnumerable<int>> CurrentWeightedBalls => _intListSavers["CurrentWeightedBalls"];
        
        static Saver()
        {
            InitIntSaver();
            InitBoolSaver();
            InitIntListSaver();
        }

        #endregion

        #region Init Functions

        private static void InitIntSaver()
        {
            _intSavers.Add("Gem", new IntSaver("Gem"));
            _intSavers.Add("HighScore", new IntSaver("HighScore"));
            _intSavers.Add("HighBall", new IntSaver("HighBall"));
            _intSavers.Add("LastScore", new IntSaver("LastScore"));
            _intSavers.Add("CurrentScore", new IntSaver("CurrentScore"));
            _intSavers.Add("CurrentBalls", new IntSaver("CurrentBalls"));
            _intSavers.Add("CurrentWeightedBalls", new IntSaver("CurrentWeightedBalls"));
        }
        
        private static void InitBoolSaver()
        {
            _boolSavers.Add("VolumeMuted", new BoolSaver("VolumeMuted"));
        }
        
        private static void InitIntListSaver()
        {
            _intListSavers.Add("CurrentBalls", new IntListSaver("CurrentBalls"));
            _intListSavers.Add("CurrentWeightedBalls", new IntListSaver("CurrentWeightedBalls"));
        }

        #endregion

        #region Functions

        public static void ResetCurrentScores()
        {
            CurrentScore.Delete();
            CurrentBalls.Delete();
            CurrentWeightedBalls.Delete();
        }
        
        public static void ResetAllScores()
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

        #endregion
    }
}
