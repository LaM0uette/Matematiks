using Game.Modules.GameMode;
using UnityEngine;

namespace Game.Modules.Manager
{
    public class LevelManager : MonoBehaviour
    {
        #region Statements

        private IGameMode _gameMode;

        private void Start()
        {
            _gameMode = gameObject.AddComponent<StandardGameMode>();
            _gameMode.Start();
        }

        #endregion

        #region Events

        private void Update()
        {
            _gameMode.Update();
        }

        #endregion
    }
}