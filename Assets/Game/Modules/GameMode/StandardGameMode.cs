using System;
using System.Collections.Generic;
using System.Linq;
using Game.Modules.Board.Balls;
using Game.Modules.Manager;
using Game.Modules.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Modules.GameMode
{
    public class StandardGameMode : MonoBehaviour, IGameMode
    {
        #region Statements

        private LevelManager _levelManager;
        private int _currentScore;
        
        private void Awake()
        {
            _levelManager = GetComponent<LevelManager>();
        }

        private void Start()
        {
            _currentScore = Saver.CurrentScore.LoadInt();
        }

        #endregion
        
        #region Events

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
                var newWeightedBall = new WeightedBall(mergedBall.Number, GameVar.DefaultNewBallWeight + countBallsSelected / GameVar.DefaultBallWeightDiviser);
                _levelManager.WeightedBalls.Add(newWeightedBall);
            }
            else
            {
                weightedBall.Weight += mergedBall.Number /GameVar.DefaultBallWeightDiviser + countBallsSelected / GameVar.DefaultBallWeightDiviser;
            }
        }
        
        private void UpdateScore(int value)
        {
            _currentScore += (int)Math.Pow(2, value) / 2;
            
            Saver.CurrentScore.Save(_currentScore);
            _levelManager.RaiseScoreEvent(_currentScore);
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
            
            Saver.CurrentScore.Delete();
            Saver.CurrentBalls.Delete();
            Saver.CurrentWeightedBalls.Delete();
            
            _levelManager.LooseGame();
        }
        
        #endregion
    }
}