using System.Collections.Generic;
using System.Linq;
using Game.Modules.Board.Balls;
using Game.Modules.Board.Cells;
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

        private GameObject[,] _boardGrid = new GameObject[6, 5];
        
        private void Awake()
        {
            Instance ??= this;

            BallNumbers = _ballNumbers.ToArray();
        }

        private void Start()
        {
            SetQualitySettings();
            FillBoardGrid();
            InvokeRepeating(nameof(CheckLoose), 1f, 2f);
        }

        #endregion

        #region Functions

        public void UpdateBallNumbers(int ballNumber)
        {
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
                            Debug.Log("Good");
                            return;
                        }
                    }
                }
            }
            
            Debug.Log("Loose");
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
                if (ball.IsDestroyed())
                    continue;
                
                ball.IsVisited = false;
            }
        }

        #endregion
    }
}
