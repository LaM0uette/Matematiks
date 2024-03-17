using System.Collections.Generic;
using System.IO;
using Game.Modules.Board.WeightedBall;
using UnityEngine;

namespace Game.Modules.Save.Savers
{
    public class WeightedBallSaver : BaseSaver<IEnumerable<WeightedBall>>
    {
        #region Statements

        public WeightedBallSaver(string key) : base(key)
        {
        }

        #endregion

        #region ISaver

        public override void Save(IEnumerable<WeightedBall> values)
        {
            var wrapper = new Wrappers.WeightedBallListWrapper { list = new List<WeightedBall>(values) };
            var json = JsonUtility.ToJson(wrapper);
            File.WriteAllText(GetFilePath(), json);
        }

        public override IEnumerable<WeightedBall> Load()
        {
            var path = GetFilePath();
            
            if (!File.Exists(path)) 
                return new List<WeightedBall>();
            
            var json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<Wrappers.WeightedBallListWrapper>(json);
            return wrapper.list;
        }

        #endregion
    }
}
