using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Modules.Board.Balls;
using Game.Modules.Board.Cells;
using Game.Modules.Events;
using Game.Modules.GameMode;
using Game.Modules.Utils;
using Game.Ui;
using Obvious.Soap;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Modules.Manager
{
    public class LevelManager : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Board")]
        [SerializeField] private GameObject _ballPrefab;
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private Material _lineRendererMaterial;
        
        [Space, Title("Board settings")]
        [SerializeField] private float _maxDistanceBetweenBalls = 1.3f;
        [SerializeField] private int _removeOldBallsThreshold = 8;
        
        [Space, Title("Soap")]
        public ScriptableListWeightedBall WeightedBalls;
        [SerializeField] private ScriptableEventNoParam _releaseEvent;
        [SerializeField] private ScriptableEventBall _ballSelectedEvent;
        [SerializeField] private ScriptableListBall _ballsSelected;
        [SerializeField] private BoolVariable _mouseDownVariable;
        [SerializeField] private BoolVariable _ongoingAction;
        [SerializeField] private BoolVariable _isLoose;
        
        private IGameMode _gameMode;
        
        public readonly GameObject[,] BoardGrid = new GameObject[7, 5];
        private int _minBallsToMerge;
        private int _maxBallsToMerge;

        private void Awake()
        {
            _gameMode = gameObject.AddComponent<StandardGameMode>();
            
            FillBoardGrid();
        }

        private void Start()
        {
            _gameMode.Initialize();
            Initialize();
        }

        private void Initialize()
        {
            // CurrentBalls
            var ballNumbers = Saver.CurrentBalls.LoadListInt();
            if (ballNumbers.Count <= 0)
                return;

            var width = BoardGrid.GetLength(0);
            var height = BoardGrid.GetLength(1);
            
            var i = 0;
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var ballGo = Instantiate(_ballPrefab, BoardGrid[x, y].transform);
                    ballGo.transform.localPosition = Vector3.zero;
                    
                    var ball = ballGo.GetComponent<Ball>();
                    ball.SetNum(ballNumbers[i]);
                    i++;
                }
            }
            
            // CurrentWeightedBalls
            var weightedBalls = Saver.CurrentWeightedBalls.LoadListWeightedBall();
            
            if (weightedBalls.Count <= 0)
                return;

            WeightedBalls.Reset();
            foreach (var weightedBall in weightedBalls)
            {
                WeightedBalls.Add(weightedBall);
            }
        }

        #endregion

        #region Events
        
        private void OnEnable()
        {
            _releaseEvent.OnRaised += OnRelease;
            UiEvents.RefreshUiEvent += OnUpdateBoard;
            _ballSelectedEvent.OnRaised += OnBallSelected;
        }

        private void OnDisable()
        {
            _releaseEvent.OnRaised -= OnRelease;
            UiEvents.RefreshUiEvent -= OnUpdateBoard;
            _ballSelectedEvent.OnRaised -= OnBallSelected;
        }

        #endregion

        #region SoapEvents
        
        public static void RaiseGemEvent(int value)
        {
            DataEvents.GemEvent.Invoke(value);
        }
        
        public static void RaiseScoreEvent(int value)
        {
            DataEvents.CurrentScoreEvent.Invoke(value);
            
            var highScore = Saver.HighScore.LoadInt();
            if (value <= highScore) 
                return;
            
            Saver.HighScore.Save(value);
            DataEvents.HighScoreEvent.Invoke(value);
        }
        
        private void OnRelease()
        {
            StartCoroutine(MergeBalls());
        }

        private void OnUpdateBoard()
        {
            Invoke(nameof(AfterMergeBalls), 1f);
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
            if (_ballsSelected.Count < _minBallsToMerge || _lineRenderer == null)
            {
                _ballsSelected.Clear();
                _lineRenderer.positionCount = 0;
                yield break;
            }
            
            CancelInvoke();
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
            
            _ballsSelected.Clear();
            _ongoingAction.Value = false;
            
            Invoke(nameof(AfterMergeBalls), 1f);
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
            _isLoose.Value = true;
            UiEvents.LooseEvent.Invoke();
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

        private void AfterMergeBalls()
        {
            if (_ongoingAction.Value)
                return;
            
            RemoveOldWeightedBalls();
            SaveCurrentBalls();
            _gameMode.AfterMergeBalls();
        }
        
        private void RemoveOldWeightedBalls()
        {
            var width = BoardGrid.GetLength(0);
            var height = BoardGrid.GetLength(1);

            var minBallNumber = int.MaxValue;
            var maxBallNumber = int.MinValue;
                
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var cellGrid = BoardGrid[x, y].transform;
                    
                    if (cellGrid.childCount <= 0)
                        continue;
                    
                    var ball = cellGrid.GetComponentInChildren<Ball>();
                    var ballNumber = ball.Number;
                    
                    if (ballNumber < minBallNumber)
                        minBallNumber = ballNumber;
                    
                    if (ballNumber > maxBallNumber)
                        maxBallNumber = ballNumber;
                }
            }
            
            var balls = FindObjectsOfType<Ball>();
            for (var i = WeightedBalls.Count - 1; i >= 0; i--)
            {
                if (WeightedBalls[i].Number <= maxBallNumber - _removeOldBallsThreshold && WeightedBalls[i].Number < minBallNumber)
                {
                    foreach (var ball in balls)
                    {
                        if (ball.Number == WeightedBalls[i].Number)
                        {
                            Destroy(ball.gameObject);
                        }
                    }
                    
                    WeightedBalls.Remove(WeightedBalls[i]);
                }
            }
        }

        private void SaveCurrentBalls()
        {
            var width = BoardGrid.GetLength(0);
            var height = BoardGrid.GetLength(1);
            
            List<int> ballNumbers = new();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (_ongoingAction.Value)
                        return;

                    var cellGrid = BoardGrid[x, y].transform;
                    
                    if (cellGrid.childCount <= 0)
                        return;
                    
                    var ball = cellGrid.GetComponentInChildren<Ball>();
                    ballNumbers.Add(ball.Number);
                }
            }
            
            Saver.CurrentBalls.Save(ballNumbers);
            Saver.CurrentWeightedBalls.Save(WeightedBalls.ToList());
        }
        
        #endregion
        
        #region Odin
        
        [Button]
        public void SetAllNumForRemoveOldBalls()
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
            
            int[] lstNum = {9,9,9,4,4, 4,4,4,4,4, 4,4,4,4,4, 4,4,4,4,4, 4,4,4,4,4, 4,4,4,4,4, 4,4,4,4,4};

            for (var i = 0; i < balls.Count; i++)
            {
                var ball = balls[i];
                ball.SetNum(lstNum[i]);
            }
        }

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

            WeightedBalls.Reset();
            WeightedBalls.Add(new WeightedBall(1, 80f));
            WeightedBalls.Add(new WeightedBall(2, 30f));
            WeightedBalls.Add(new WeightedBall(3, 1f));
            WeightedBalls.Add(new WeightedBall(4, 1f));
            WeightedBalls.Add(new WeightedBall(5, 1f));
            WeightedBalls.Add(new WeightedBall(6, 1f));
            WeightedBalls.Add(new WeightedBall(7, 1f));
            WeightedBalls.Add(new WeightedBall(8, 1f));
            WeightedBalls.Add(new WeightedBall(9, 1f));
            
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