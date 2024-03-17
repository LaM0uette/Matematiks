using System.IO;
using UnityEngine;

namespace Game.Modules.Save.Savers
{
    public abstract class BaseSaver<T> : ISaver<T>
    {
        #region Statements

        private readonly string _key;

        protected BaseSaver(string key)
        {
            _key = key;
        }

        #endregion

        #region ISaver

        public abstract void Save(T value);

        public abstract T Load();

        public void Delete()
        {
            var path = GetFilePath();
            if (File.Exists(path))
                File.Delete(path);
        }

        #endregion

        #region Functions

        protected string GetFilePath() => Path.Combine(Application.persistentDataPath, $"{_key}.json");

        #endregion
    }
}
