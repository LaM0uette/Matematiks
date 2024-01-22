using System;
using System.Collections;
using System.Collections.Generic;
using Game.Modules.Board.Balls;
using Game.Modules.Board.Cells;
using Game.Modules.GameMode;
using Game.Modules.Utils;
using Obvious.Soap;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Modules.Manager
{
    public class LevelManager : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Board")]
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _maxDistanceBetweenBalls = 1.3f;
        
        [Space, Title("Soap")]
        public IntVariable ScoreValueVariable;
        public ScriptableListWeightedBall WeightedBalls;
        [SerializeField] private ScriptableEventNoParam _releaseEvent;
        [SerializeField] private ScriptableEventBall _ballSelectedEvent;
        [SerializeField] private BoolVariable _mouseDownVariable;
        [SerializeField] private BoolVariable _ongoingAction;

        [Space, Title("Ui")]
        [SerializeField] private GameObject _loosePanel;
        
        private IGameMode _gameMode;
        
        private readonly List<Ball> _ballsSelected = new();
        private readonly GameObject[,] _boardGrid = new GameObject[6, 5];

        private void Awake()
        {
            FillBoardGrid();
        }

        private void Start()
        {
            
            //InvokeRepeating(nameof(CheckLoose), 1f, 2f);
            
            _gameMode = gameObject.AddComponent<StandardGameMode>();
            
            _gameMode.StartGame();
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

        private void LateUpdate()
        {
            CheckLoose();
        }

        #endregion

        #region SoapEvents
        
        private void OnRelease()
        {
            StartCoroutine(MergeBalls());
        }

        private void OnBallSelected(Ball ball)
        {
            if (_mouseDownVariable.Value == false) 
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

            var firstNumBall = _ballsSelected[0];
            if (firstNumBall.Number != ball.Number) 
                return;
            
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
        
        private void OnLoose()
        {
            _loosePanel.SetActive(true);
        }
        
        #endregion

        #region Functions
        
        private IEnumerator MergeBalls() 
        {
            if (_ballsSelected.Count < 3) // TODO: ajouter aussi un maximum sui sera initialisé dans les modes de jeux
            {
                _ballsSelected.Clear();
                _lineRenderer.positionCount = 0;
                yield break;
            }
            
            _ongoingAction.Value = true;
            SetBlockAllBalls(true);
            
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
            _gameMode.MergeBalls(mergedBall);
            
            // TODO: à supprimer/deplacer
            var newBallNumber = mergedBall.Number + 1;
            if (Application.isPlaying && newBallNumber >= GameManager.Instance.MaxBallNumber)
            {
                GameManager.BallScore.gameObject.SetActive(true);
                GameManager.Instance.MaxBallNumber = newBallNumber;
                GameManager.BallScore.SetNum(newBallNumber);
                
                if (newBallNumber > Saver.GetMaxBall())
                {
                    Saver.SaveMaxBall(newBallNumber);
                }
            }
            
            _ongoingAction.Value = false;
            SetBlockAllBalls(false);
            
            _ballsSelected.Clear();
        }
        
        private static void SetBlockAllBalls(bool value) 
        {
            foreach (var ball in FindObjectsOfType<Ball>()) 
            {
                ball.IsBlocked = value;
            }
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
        
        public void ShowLoosePanel()
        {
            _loosePanel.SetActive(true);
        }

        private void FillBoardGrid()
        {
            var cells = FindObjectsOfType<Cell>();
            
            foreach (var cell in cells)
            {
                if (cell.transform.CompareTag("CellCache")) 
                    continue;

                var position = ExtractCellPositionFromName(cell.name);
                _boardGrid[position.x, position.y] = cell.gameObject;
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
            if (_ongoingAction.Value)
                return;
            
            var width = _boardGrid.GetLength(0);
            var height = _boardGrid.GetLength(1);

            List<Ball> balls = new();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var ball = _boardGrid[x, y].transform.GetComponentInChildren<Ball>();
                    balls.Add(ball);
                }
            }
            
            ResetVisited(balls);

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var ball = _boardGrid[x, y].transform.GetComponentInChildren<Ball>();
                    
                    if (!ball.IsVisited)
                    {
                        if (DFS(x, y, ball.Number) >= 3)
                        {
                            return;
                        }
                    }
                }
            }
            
            _gameMode.EndGame();
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
            var width = _boardGrid.GetLength(0);
            var height = _boardGrid.GetLength(1);
            
            if (x < 0 || y < 0 || x >= width || y >= height)
                return 0;
            
            var ball = _boardGrid[x, y].transform.GetComponentInChildren<Ball>();
            
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

        #endregion
        
        #region Odin

        [Button]
        public void SetAllNumForLoose()
        {
            var board = new GameObject[6, 5];
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
            
            int[] lstNum = {1,3,1,3,1,2,5,2,5,2,1,3,1,3,1,2,5,2,5,2,6,7,6,7,6,2,5,9,9,9};

            for (var i = 0; i < balls.Count; i++)
            {
                var ball = balls[i];
                ball.SetNum(lstNum[i]);
            }
        }
        
        [Button]
        public void ResetAllNumForLoose()
        {
            var board = new GameObject[6, 5];
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