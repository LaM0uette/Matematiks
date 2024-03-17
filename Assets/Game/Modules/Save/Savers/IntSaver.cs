using System.IO;
using UnityEngine;

namespace Game.Modules.Save.Savers
{
    public class IntSaver : BaseSaver<int>
    {
        #region Statements

        public IntSaver(string key) : base(key)
        {
        }

        #endregion

        #region ISaver

        public override void Save(int value)
        {
            var wrapper = new Wrappers.IntWrapper { value = value };
            var json = JsonUtility.ToJson(wrapper);
            File.WriteAllText(GetFilePath(), json);
        }

        public override int Load()
        {
            var path = GetFilePath();
            
            if (!File.Exists(path)) 
                return 0;
            
            var json = File.ReadAllText(path);
            var wrapper = JsonUtility.FromJson<Wrappers.IntWrapper>(json);
            return wrapper.value;
        }

        #endregion
    }
}
