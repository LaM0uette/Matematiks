using System;
using System.Collections.Generic;
using Game.Modules.Board;
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
        
        private void Awake()
        {
            _levelManager = GetComponent<LevelManager>();
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            BonusManager.BonusEvent += OnBonusEvent;
        }

        private void OnDisable()
        {
            BonusManager.BonusEvent -= OnBonusEvent;
        }
        
        #endregion

        #region Functions
        
        public void Initialize()
        {
            BoardHandler.Initialize();

            _levelManager.InitializeBallsToMerge(3, 99);
            
            var balls = Saver.CurrentBalls.LoadListInt();
            if (balls.Count > 0)
                _levelManager.LoadCurrentGame();
            else
                _levelManager.InitializeWeightedBalls();
        }

        public void MergeBallsUpdate(int mergedBallNumber, int countBallsSelected)
        {
            UpdateGem(mergedBallNumber, countBallsSelected);
            UpdateHighBall(mergedBallNumber);
            UpdateScore(mergedBallNumber);
        }

        public void MergeBallsComplete()
        {
            CheckLoose();
        }
        
        private static void UpdateGem(int mergedBallNumber, int countBallsSelected)
        {
            var gem = Saver.Gem.LoadInt();
            gem += 1 + mergedBallNumber / 3 + countBallsSelected / 3;
            
            Saver.Gem.Save(gem);
            RaiseGemEvent(gem);
        }

        private static void UpdateHighBall(int newBallNumber)
        {
            if (!Application.isPlaying || newBallNumber <= Saver.HighBall.LoadInt())
                return;
            
            Saver.HighBall.Save(newBallNumber);
            GameEvents.HighBallEvent.Invoke(newBallNumber);
        }
        
        private static void UpdateScore(int value)
        {
            var currentScore = Saver.CurrentScore.LoadInt();
            currentScore += (int)Math.Pow(value, 3);
            
            Saver.CurrentScore.Save(currentScore);
            RaiseScoreEvent(currentScore);
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

        private static void LooseGame()
        {
            var currentScore = Saver.CurrentScore.LoadInt();
            Saver.LastScore.Save(currentScore);
            
            Saver.ResetAllCurrentScores();
            
            BoardHandler.IsLost = true;
            UiEvents.LooseEvent.Invoke();
        }
        
        private static void OnBonusEvent(BonusData bonusData)
        {
            var _gem = Saver.Gem.LoadInt();
            if (_gem < bonusData.Cost)
            {
                BonusManager.CurrentBonus = null;
                UiEvents.RefreshUiEvent.Invoke();
                return;
            }
            
            BonusManager.CurrentBonus = bonusData;
            
            _gem -= bonusData.Cost;
            Saver.Gem.Save(_gem);
            RaiseGemEvent(_gem);
        }
        
        private static void RaiseGemEvent(int value)
        {
            GameEvents.GemEvent.Invoke(value);
        }
        
        private static void RaiseScoreEvent(int value)
        {
            GameEvents.CurrentScoreEvent.Invoke(value);
            
            var highScore = Saver.HighScore.LoadInt();
            if (value <= highScore) 
                return;
            
            Saver.HighScore.Save(value);
            GameEvents.HighScoreEvent.Invoke(value);
        }
        
        #endregion
    }
}