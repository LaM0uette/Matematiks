﻿using System.Collections;
using System.Collections.Generic;
using Game.Modules.Board.Balls;
using Game.Modules.Board.Cells;
using Game.Modules.GameMode;
using Game.Modules.Utils;
using Obvious.Soap;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.Manager
{
    public class LevelManager : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Board")]
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Material _lineRendererMaterial;
        [SerializeField] private float _maxDistanceBetweenBalls = 1.3f;
        
        [Space, Title("Soap")]
        public IntVariable ScoreValueVariable;
        public ScriptableListWeightedBall WeightedBalls;
        [SerializeField] private ScriptableEventNoParam _releaseEvent;
        [SerializeField] private ScriptableEventBall _ballSelectedEvent;
        [SerializeField] private ScriptableListBall _ballsSelected;
        [SerializeField] private BoolVariable _mouseDownVariable;
        [SerializeField] private BoolVariable _ongoingAction;
        [SerializeField] private BoolVariable _isLoose;

        [Space, Title("Score")]
        [ShowInInspector, ReadOnly] private int _maxBallNumber = 1;
        
        [Space, Title("Ui")]
        [SerializeField] private GameObject _loosePanel;
        [SerializeField] private Ball _ballScore;
        
        private IGameMode _gameMode;
        
        public readonly GameObject[,] BoardGrid = new GameObject[7, 5];
        private int _minBallsToMerge;
        private int _maxBallsToMerge;

        private void Awake()
        {
            _gameMode = gameObject.AddComponent<StandardGameMode>();
            _gameMode.Initialize();
            
            FillBoardGrid();
        }

        #endregion

        #region Events
        
        private void OnEnable()
        {
            _releaseEvent.OnRaised += OnRelease;
            _ballSelectedEvent.OnRaised += OnBallSelected;
        }

        private void OnDisable()
        {
            _releaseEvent.OnRaised -= OnRelease;
            _ballSelectedEvent.OnRaised -= OnBallSelected;
        }

        #endregion

        #region SoapEvents
        
        private void OnRelease()
        {
            StartCoroutine(MergeBalls());
        }

        private void OnBallSelected(Ball ball)
        {
            if (_mouseDownVariable.Value == false || _isLoose.Value) 
                return;
            
            if (_ballsSelected.Count == 0)
            {
                AddFirstBall(ball);
                return;
            }

            if (_ballsSelected.Contains(ball) && _ballsSelected.Count > 1)
            {
                RemovePreviousBall(ball);
                return;
            }

            if (_ballsSelected.Count >= _maxBallsToMerge) 
                return;
            
            var firstNumBall = _ballsSelected[0];
            if (firstNumBall.Number != ball.Number) 
                return;
            
            _lineRendererMaterial.color = ball.Color;
            _ballsSelected.Add(ball);
            
            if (_ballsSelected.Count > 1)
            {
                var distance = Vector3.Distance(_ballsSelected[^2].transform.position, _ballsSelected[^1].transform.position);

                if (distance > _maxDistanceBetweenBalls)
                {
                    _ballsSelected.Remove(_ballsSelected[^1]);
                    _lineRenderer.positionCount = _ballsSelected.Count;
                    return;
                }
            }
            
            _lineRenderer.positionCount = _ballsSelected.Count;
            _lineRenderer.SetPosition(_ballsSelected.Count - 1, ball.transform.position);
        }
        
        #endregion

        #region Functions

        public void InitializeBallsToMerge(int min, int max)
        {
            _minBallsToMerge = min;
            _maxBallsToMerge = max;
        }
        
        private IEnumerator MergeBalls() 
        {
            if (_ballsSelected.Count < _minBallsToMerge)
            {
                _ballsSelected.Clear();
                _lineRenderer.positionCount = 0;
                yield break;
            }
            
            _ongoingAction.Value = true;
            
            for (var i = 0; i < _ballsSelected.Count - 1; i++) 
            {
                var currentBall = _ballsSelected[i];
                var nextBall = _ballsSelected[i + 1];
                
                var duration = GameVar.BallMoveDuration;
                var elapsedTime = 0f;
                var startPosition = currentBall.transform.position;
                var endPosition = nextBall.transform.position;
                
                RemoveLineRendererFirstPosition();
                
                while (elapsedTime < duration) 
                {
                    currentBall.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                Destroy(currentBall.gameObject);
            }
            
            var mergedBall = _ballsSelected[^1];
            _gameMode.MergeBalls(mergedBall, _ballsSelected.Count);
            
            // TODO: à supprimer/deplacer
            var newBallNumber = mergedBall.Number;
            if (Application.isPlaying && newBallNumber >= _maxBallNumber)
            {
                if (!_ballScore.isActiveAndEnabled)
                    _ballScore.gameObject.SetActive(true);

                _maxBallNumber = newBallNumber;
                _ballScore.SetNum(newBallNumber);
                
                if (newBallNumber > Saver.GetMaxBall())
                {
                    Saver.SaveMaxBall(newBallNumber);
                }
            }
            
            _ballsSelected.Clear();
            _ongoingAction.Value = false;
            
            Invoke(nameof(CheckLoose), 1f);
        }
        
        private void RemoveLineRendererFirstPosition() 
        {
            var positionsCount = _lineRenderer.positionCount;
            
            if (positionsCount <= 1) 
            {
                _lineRenderer.positionCount = 0;
                return;
            }

            var newPositions = new Vector3[positionsCount - 1];

            for (var i = 1; i < positionsCount; i++) 
            {
                newPositions[i - 1] = _lineRenderer.GetPosition(i);
            }

            _lineRenderer.positionCount = positionsCount - 1;
            _lineRenderer.SetPositions(newPositions);
        }
        
        private void AddFirstBall(Ball ball)
        {
            _ballsSelected.Add(ball);
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, ball.transform.position);
        }

        private void RemovePreviousBall(Object ball)
        {
            if (_ballsSelected[^2] != ball) 
                return;
            
            _ballsSelected.Remove(_ballsSelected[^1]);
            _lineRenderer.positionCount = _ballsSelected.Count;
        }

        #endregion

        #region Loose
        
        public void LooseGame()
        {
            _loosePanel.SetActive(true);
            _isLoose.Value = true;
        }

        private void FillBoardGrid()
        {
            var cells = FindObjectsOfType<Cell>();
            
            foreach (var cell in cells)
            {
                if (cell.transform.CompareTag("CellCache")) 
                    continue;

                var position = ExtractCellPositionFromName(cell.name);
                BoardGrid[position.x, position.y] = cell.gameObject;
            }
        }
        
        private static Vector2Int ExtractCellPositionFromName(string name)
        {
            var parts = name.Split('_');
            var x = int.Parse(parts[1]);
            var y = int.Parse(parts[2]);
            return new Vector2Int(x, y);
        }

        private void CheckLoose()
        {
            _gameMode.CheckLoose();
        }
        
        #endregion
        
        #region Odin

        [Button]
        public void SetAllNumForTestLineRenderer()
        {
            var board = new GameObject[7, 5];
            var cells = FindObjectsOfType<Cell>();
            
            foreach (var cell in cells)
            {
                if (cell.transform.CompareTag("CellCache")) 
                    continue;

                var position = ExtractCellPositionFromName(cell.name);
                board[position.x, position.y] = cell.gameObject;
            }
            
            var width = board.GetLength(0);
            var height = board.GetLength(1);
            
            List<Ball> balls = new();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var ball = board[x, y].transform.GetComponentInChildren<Ball>();
                    balls.Add(ball);
                }
            }
            
            
            int[] lstNum = {1,1,1,1,1, 2,2,2,2,2, 3,3,3,3,3, 4,4,4,4,4, 5,5,5,8,9, 6,6,6,8,9, 7,7,7,8,9};

            for (var i = 0; i < balls.Count; i++)
            {
                var ball = balls[i];
                ball.SetNum(lstNum[i]);
            }
        }
        
        [Button]
        public void SetAllNumForLoose()
        {
            var board = new GameObject[7, 5];
            var cells = FindObjectsOfType<Cell>();
            
            foreach (var cell in cells)
            {
                if (cell.transform.CompareTag("CellCache")) 
                    continue;

                var position = ExtractCellPositionFromName(cell.name);
                board[position.x, position.y] = cell.gameObject;
            }
            
            var width = board.GetLength(0);
            var height = board.GetLength(1);
            
            List<Ball> balls = new();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var ball = board[x, y].transform.GetComponentInChildren<Ball>();
                    balls.Add(ball);
                }
            }
            
            
            int[] lstNum = {1,3,1,3,1, 2,5,2,5,2, 1,3,1,3,1, 2,5,2,5,2, 6,7,6,7,6, 6,7,6,7,6, 2,5,9,9,9};

            for (var i = 0; i < balls.Count; i++)
            {
                var ball = balls[i];
                ball.SetNum(lstNum[i]);
            }
        }
        
        [Button]
        public void ResetAllNumForLoose()
        {
            var board = new GameObject[7, 5];
            var cells = FindObjectsOfType<Cell>();
            
            foreach (var cell in cells)
            {
                if (cell.transform.CompareTag("CellCache")) 
                    continue;

                var position = ExtractCellPositionFromName(cell.name);
                board[position.x, position.y] = cell.gameObject;
            }
            
            var width = board.GetLength(0);
            var height = board.GetLength(1);
            
            List<Ball> balls = new();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var ball = board[x, y].transform.GetComponentInChildren<Ball>();
                    balls.Add(ball);
                }
            }

            foreach (var ball in balls)
            {
                ball.SetNum(1);
            }
        }

        #endregion
    }
}