using System.Collections;
using System.Collections.Generic;
using Game.Modules.Board.Balls;
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
        [SerializeField] private float _maxDistanceBetweenBalls = 1.3f;
        
        [Space, Title("Soap")]
        public IntVariable ScoreValueVariable;
        [SerializeField] private ScriptableEventNoParam _releaseEvent;
        [SerializeField] private ScriptableEventBall _ballSelectedEvent;
        [SerializeField] private BoolVariable _mouseDownVariable;
        [SerializeField] private BoolVariable _ongoingAction;

        private readonly List<Ball> _ballsSelected = new();
        
        private IGameMode _gameMode;

        private void Start()
        {
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
            _gameMode.MergedBall(mergedBall);
            
            // TODO: à supprimer/deplacer
            var newBallNumber = mergedBall.Number + 1;
            GameManager.Instance.UpdateBallNumbers(newBallNumber);
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
    }
}