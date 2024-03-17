using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Modules.Save.Savers
{
    public class IntListSaver : BaseSaver<IEnumerable<int>>
    {
        #region Statements

        public IntListSaver(string key) : base(key)
        {
        }

        #endregion

        #region ISaver

        public override void Save(IEnumerable<int> values)
        {
            var wrapper = new Wrappers.IntListWrapper { Values = new List<int>(values) };
            var json = JsonUtility.ToJson(wrapper);
            File.WriteAllText(GetFilePath(), json);
        }

        public override IEnumerable<int> Load()
        {
            var path = GetFilePath();
            
            if (!File.Exists(path)) 
                return new List<int>();
            
            var json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<Wrappers.IntListWrapper>(json);
            return wrapper.Values;
        }

        #endregion
    }
}
