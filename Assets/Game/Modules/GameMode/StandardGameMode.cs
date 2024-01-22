using System;
using System.Collections.Generic;
using Game.Modules.Board.Balls;
using Game.Modules.Manager;
using Obvious.Soap;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.GameMode
{
    public class StandardGameMode : MonoBehaviour, IGameMode
    {
        #region Statements

        // Components
        private LevelManager _levelManager;
        
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
            _levelManager.WeightedBalls.Add(new WeightedBall(1, 100f));
            _levelManager.WeightedBalls.Add(new WeightedBall(2, 25f));
            _levelManager.WeightedBalls.Add(new WeightedBall(3, 10f));
        }
        
        private void UpdateScore(int value)
        {
            _levelManager.ScoreValueVariable.Value += (int)Math.Pow(2, value) / 2;
        }

        #endregion
    }
}