using System.Collections.Generic;
using Game.Modules.Board.Balls;
using Game.Modules.Board.Cells;
using Game.Modules.Utils;
using Obvious.Soap;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Modules.Manager
{
    public class GameManager : MonoBehaviour
    {
        #region Statements

        public static GameManager Instance { get; private set; }
        
        [Space, Title("Balls")]
        [SerializeField] private List<BallNumber> _ballNumbers = new();
        [ShowInInspector, ReadOnly] public BallNumber[] BallNumbers;

        [Space, Title("Soap")]
        [SerializeField] private BoolVariable _isInAnimationVariable;
        [SerializeField] private ScriptableEventNoParam _looseEvent;
        
        public static Ball BallScore { get; set; }
        [Space, Title("Score")]
        [ShowInInspector, ReadOnly] public int MaxBallNumber = 1;
        
        private readonly GameObject[,] _boardGrid = new GameObject[6, 5];
        
        private void Awake()
        {
            Instance ??= this;

            BallNumbers = _ballNumbers.ToArray();
        }

        private void Start()
        {
            MaxBallNumber = 1;
            BallScore = GameObject.FindGameObjectWithTag("BallMaxScore").transform.GetComponent<Ball>();
            BallScore.gameObject.SetActive(false);
            
            SetQualitySettings();
            FillBoardGrid();
            InvokeRepeating(nameof(CheckLoose), 1f, 2f);
            
            //Saver.ResetBestScore();
            //Saver.ResetMaxBall();
        }

        #endregion

        #region Functions

        public void UpdateBallNumbers(int ballNumber)
        {
            if (ballNumber > BallNumbers.Length)
                return;
            
            if (BallNumbers[ballNumber - 1].IsLocked)
            {
                BallNumbers[ballNumber - 1].IsLocked = false;
            }

            if (ballNumber > 2)
            {
                BallNumbers[ballNumber - 1].Weight += ballNumber / 30f;
            }
        }

        private static void SetQualitySettings()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 240;
        }

        private void FillBoardGrid()
        {
            var cells = FindObjectsOfType<Cell>();
            
            foreach (var cell in cells)
            {
                if (cell.transform.CompareTag("CellCache")) 
                    continue;

                var position = ExtractPositionFromName(cell.name);
                _boardGrid[position.x, position.y] = cell.gameObject;
            }
        }
        
        private static Vector2Int ExtractPositionFromName(string name)
        {
            var parts = name.Split('_');
            var x = int.Parse(parts[1]);
            var y = int.Parse(parts[2]);
            return new Vector2Int(x, y);
        }
        
        private void CheckLoose()
        {
            if (_isInAnimationVariable.Value)
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
            
            Debug.Log("Loose");
            _looseEvent.Raise();
        }
        
        private int DFS(int x, int y, int number)
        {
            var width = _boardGrid.GetLength(0);
            var height = _boardGrid.GetLength(1);
            
            if (x < 0 || y < 0 || x >= width || y >= height)
                return 0;
            
            var ball = _boardGrid[x, y].transform.GetComponentInChildren<Ball>();
            
            if (ball.IsDestroyed() || ball.IsVisited || ball.Number != number)
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

        private static void ResetVisited(IEnumerable<Ball> balls)
        {
            foreach (var ball in balls)
            {
                if (ball.IsDestroyed() || ball == null)
                    continue;
                
                ball.IsVisited = false;
            }
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

                var position = ExtractPositionFromName(cell.name);
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

                var position = ExtractPositionFromName(cell.name);
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
