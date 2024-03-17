using System.IO;
using UnityEngine;

namespace Game.Modules.Save.Savers
{
    public class BoolSaver : BaseSaver<bool>
    {
        #region Statements

        public BoolSaver(string key) : base(key)
        {
        }

        #endregion

        #region ISaver

        public override void Save(bool value)
        {
            var wrapper = new Wrappers.BoolWrapper { Value = value };
            var json = JsonUtility.ToJson(wrapper);
            File.WriteAllText(GetFilePath(), json);
        }

        public override bool Load()
        {
            var path = GetFilePath();
            
            if (!File.Exists(path)) 
                return false;
            
            var json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<Wrappers.BoolWrapper>(json);
            return wrapper.Value;
        }

        #endregion
    }
}
