using System;
using System.Collections.Generic;
using System.Linq;
using Game.Modules.Board.Balls;
using Game.Modules.Bonus;
using Game.Modules.Events;
using Game.Modules.Manager;
using Game.Modules.Utils;
using Game.Ui;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Modules.GameMode
{
    public class StandardGameMode : MonoBehaviour, IGameMode
    {
        #region Statements

        private LevelManager _levelManager;
        private int _gem;
        private int _highBallNumber;
        private int _currentScore;
        
        private void Awake()
        {
            _levelManager = GetComponent<LevelManager>();
        }

        private void Start()
        {
            _currentScore = Saver.CurrentScore.LoadInt();
#if UNITY_EDITOR
            Saver.Gem.Save(5000);
#endif
            _gem = Saver.Gem.LoadInt();
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            BonusManager.Instance.BonusEvent += OnBonusEvent;
        }

        private void OnDisable()
        {
            BonusManager.Instance.BonusEvent -= OnBonusEvent;
        }
        
        public void Initialize()
        {
            InitializeWeightedBalls();
            _levelManager.InitializeBallsToMerge(3, 99);
        }

        public void MergeBalls(Ball mergedBall, int countBallsSelected)
        {
            var mergedBallNumber = mergedBall.Number + 1;
            
            mergedBall.SetNum(mergedBallNumber);
            UpdateWeightedBalls(mergedBall, countBallsSelected);
            
            GainGem(mergedBallNumber, countBallsSelected);
            UpdateHighBall(mergedBallNumber);
            UpdateScore(mergedBallNumber);
        }

        public void AfterMergeBalls()
        {
            CheckLoose();
        }
        
        #endregion

        #region Functions

        private void InitializeWeightedBalls()
        {
            _levelManager.WeightedBalls.Add(new WeightedBall(1, 80f));
            _levelManager.WeightedBalls.Add(new WeightedBall(2, 40f));
            _levelManager.WeightedBalls.Add(new WeightedBall(3, 20f));
        }

        private void UpdateWeightedBalls(Ball mergedBall, int countBallsSelected)
        {
            if (mergedBall.Number <= 1)
                return;
            
            var weightedBall = _levelManager.WeightedBalls.FirstOrDefault(wb => wb.Number == mergedBall.Number);

            if (!_levelManager.WeightedBalls.Contains(weightedBall) || weightedBall == null)
            {
                var newWeightedBall = new WeightedBall(mergedBall.Number, GameVar.DefaultNewBallWeight + countBallsSelected);
                _levelManager.WeightedBalls.Add(newWeightedBall);
            }
            else
            {
                weightedBall.Weight += mergedBall.Number / GameVar.DefaultBallWeightDiviser + countBallsSelected;
            }
        }
        
        private void GainGem(int mergedBallNumber, int countBallsSelected)
        {
            _gem += 1 + mergedBallNumber / 3 + countBallsSelected / 3;
            
            Saver.Gem.Save(_gem);
            LevelManager.RaiseGemEvent(_gem);
        }

        private void UpdateHighBall(int newBallNumber)
        {
            if (!Application.isPlaying || newBallNumber <= _highBallNumber)
                return;
            
            _highBallNumber = newBallNumber;
            if (newBallNumber > Saver.HighBall.LoadInt())
            {
                Saver.HighBall.Save(newBallNumber);
                DataEvents.HighBallEvent.Invoke(newBallNumber);
            }
        }
        
        private void UpdateScore(int value)
        {
            _currentScore += (int)Math.Pow(value, 3);
            
            Saver.CurrentScore.Save(_currentScore);
            LevelManager.RaiseScoreEvent(_currentScore);
        }

        private void CheckLoose()
        {
            var width = _levelManager.BoardGrid.GetLength(0);
            var height = _levelManager.BoardGrid.GetLength(1);

            List<Ball> balls = new();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var ball = _levelManager.BoardGrid[x, y].transform.GetComponentInChildren<Ball>();
                    balls.Add(ball);
                }
            }
            
            ResetVisited(balls);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var ball = _levelManager.BoardGrid[x, y].transform.GetComponentInChildren<Ball>();

                    if (ball == null || ball.IsVisited) 
                        continue;
                    
                    if (DFS(x, y, ball.Number) >= 3)
                    {
                        return;
                    }
                }
            }
            
            LooseGame();
        }
        
        private static void ResetVisited(IEnumerable<Ball> balls)
        {
            foreach (var ball in balls)
            {
                if (ball.IsDestroyed() || ball == null)
                    continue;
                
                ball.IsVisited = false;
            }
        }
        
        private int DFS(int x, int y, int number)
        {
            var width = _levelManager.BoardGrid.GetLength(0);
            var height = _levelManager.BoardGrid.GetLength(1);
            
            if (x < 0 || y < 0 || x >= width || y >= height)
                return 0;
            
            var ball = _levelManager.BoardGrid[x, y].transform.GetComponentInChildren<Ball>();
            
            if (ball == null || ball.IsDestroyed() || ball.IsVisited || ball.Number != number)
                return 0;

            ball.IsVisited = true;
            var count = 1;

            // Vérifier les huit directions (verticales, horizontales et diagonales)
            count += DFS(x + 1, y, number);
            count += DFS(x - 1, y, number);
            count += DFS(x, y + 1, number);
            count += DFS(x, y - 1, number);
            count += DFS(x + 1, y + 1, number);
            count += DFS(x - 1, y - 1, number);
            count += DFS(x + 1, y - 1, number);
            count += DFS(x - 1, y + 1, number);

            return count;
        }

        private void LooseGame()
        {
            Saver.LastScore.Save(_currentScore);
            _currentScore = 0;
            
            Saver.ResetAllCurrentScores();
            
            _levelManager.LooseGame();
        }
        
        private void OnBonusEvent(BonusData bonusData)
        {
            if (_gem < bonusData.Cost)
            {
                BonusManager.Instance.CurrentBonus = null;
                UiEvents.RefreshUiEvent.Invoke();
                return;
            }
            
            BonusManager.Instance.CurrentBonus = bonusData;
            
            _gem -= bonusData.Cost;
            Saver.Gem.Save(_gem);
            LevelManager.RaiseGemEvent(_gem);
        }
        
        #endregion
    }
}