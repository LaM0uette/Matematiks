using System;
using Game.Modules.Manager;
using UnityEngine;

namespace Game.Modules.GameMode
{
    public class StandardGameMode : MonoBehaviour, IGameMode
    {
        #region Statements

        private LevelManager _levelManager;
        
        private void Awake()
        {
            _levelManager = GetComponent<LevelManager>();
        }

        #endregion
        
        #region Events

        public void Start()
        {
        }

        public void Update()
        {
        }

        public void End()
        {
        }

        #endregion
        
        
    }
}