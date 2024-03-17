using System.IO;
using Game.Modules.Utils;
using UnityEngine;

namespace Game.Modules.Save.Savers
{
    public class BoolSaver : BaseSaver<bool>
    {
        public BoolSaver(string key) : base(key)
        {
        }

        public override void Save(bool value)
        {
            var wrapper = new Wrappers.BoolWrapper { value = value };
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
            return wrapper.value;
        }
    }
}
