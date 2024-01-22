using System;
using System.Collections.Generic;
using Game.Modules.Board.Balls;
using Game.Modules.Manager;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.GameMode
{
    public class StandardGameMode : MonoBehaviour, IGameMode
    {
        #region Statements

        // Components
        private LevelManager _levelManager;
        
        [ShowInInspector, ReadOnly]
        private List<WeightedBall> _weightedBalls = new();
        
        private void Awake()
        {
            _levelManager = GetComponent<LevelManager>();
        }

        #endregion
        
        #region Events

        public void StartGame()
        {
            InitializeWeightedBalls();
        }

        public void MergeBalls(Ball mergedBall)
        {
            var mergedBallNumber = mergedBall.Number + 1;
            
            mergedBall.SetNum(mergedBallNumber);
            UpdateScore(mergedBallNumber);
        }

        public void EndGame()
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
        
        private void UpdateScore(int value)
        {
            _levelManager.ScoreValueVariable.Value += (int)Math.Pow(2, value) / 2;
        }

        #endregion
    }
}