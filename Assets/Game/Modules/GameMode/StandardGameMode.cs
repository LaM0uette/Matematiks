using System;
using System.Linq;
using Game.Modules.Board.Balls;
using Game.Modules.Manager;
using Game.Modules.Utils;
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

        public void Initialize()
        {
            InitializeWeightedBalls();
        }

        public void MergeBalls(Ball mergedBall, int countBallsSelected)
        {
            var mergedBallNumber = mergedBall.Number + 1;
            
            mergedBall.SetNum(mergedBallNumber);
            UpdateWeightedBalls(mergedBall, countBallsSelected);
            UpdateScore(mergedBallNumber);
        }

        public void End()
        {
            _levelManager.ShowLoosePanel();
        }

        #endregion

        #region Functions

        private void InitializeWeightedBalls()
        {
            _levelManager.WeightedBalls.Add(new WeightedBall(1, 80f));
            _levelManager.WeightedBalls.Add(new WeightedBall(2, 30f));
            _levelManager.WeightedBalls.Add(new WeightedBall(3, 10f));
        }

        private void UpdateWeightedBalls(Ball mergedBall, int countBallsSelected)
        {
            if (mergedBall.Number <= 2)
                return;
            
            var weightedBall = _levelManager.WeightedBalls.FirstOrDefault(wb => wb.Number == mergedBall.Number);

            if (!_levelManager.WeightedBalls.Contains(weightedBall) || weightedBall == null)
            {
                var newWeightedBall = new WeightedBall(mergedBall.Number, GameVar.DefaultNewBallWeight + countBallsSelected / 15f);
                _levelManager.WeightedBalls.Add(newWeightedBall);
            }
            else
            {
                weightedBall.Weight += mergedBall.Number /GameVar.DefaultBallWeightDiviser + countBallsSelected / GameVar.DefaultBallWeightDiviser;
            }
        }
        
        private void UpdateScore(int value)
        {
            _levelManager.ScoreValueVariable.Value += (int)Math.Pow(2, value) / 2;
        }

        #endregion
    }
}