using System.IO;
using UnityEngine;

namespace Game.Modules.Save.Savers
{
    public abstract class BaseSaver<T> : ISaver<T>
    {
        protected string Key;

        protected BaseSaver(string key)
        {
            Key = key;
        }

        public abstract void Save(T value);

        public abstract T Load();

        public void Delete()
        {
            var path = GetFilePath();
            if (File.Exists(path))
                File.Delete(path);
        }

        protected string GetFilePath() => Path.Combine(Application.persistentDataPath, $"{Key}.json");
    }
}
