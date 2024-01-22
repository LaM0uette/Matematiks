using System.Collections.Generic;
using Game.Modules.Board.Balls;
using Game.Modules.Manager;
using UnityEngine;

namespace Game.Modules.GameMode
{
    public class StandardGameMode : MonoBehaviour, IGameMode
    {
        #region Statements

        // Components
        private LevelManager _levelManager;
        
        private List<WeightedBall> _weightedBalls = new();
        
        private void Awake()
        {
            _levelManager = GetComponent<LevelManager>();
        }

        #endregion
        
        #region Events

        public void Start()
        {
            InitializeWeightedBalls();
        }

        public void Update()
        {
        }

        public void End()
        {
        }

        #endregion

        #region Functions

        private void InitializeWeightedBalls()
        {
            _weightedBalls.Add(new WeightedBall(1, 100f));
            _weightedBalls.Add(new WeightedBall(2, 25f));
            _weightedBalls.Add(new WeightedBall(3, 10f));
        }

        #endregion
    }
}