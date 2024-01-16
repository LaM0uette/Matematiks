using System.Collections;
using Game.Modules._utils;
using Game.Modules.Board.Balls;
using Game.Modules.Player.Inputs;
using Obvious.Soap;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Board")]
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _maxDistanceBetweenTwoBalls = 1.3f;
        
        [Space, Title("Soap")]
        [SerializeField] private BoolVariable _mouseIsDownVariable;
        [SerializeField] private ScriptableEventBall _ballSelectedEvent;
        [SerializeField] private ScriptableListBall _ballsSelected;

        private PlayerInputsReader _inputsReader;
        
        private void Awake()
        {
            _inputsReader = GetComponent<PlayerInputsReader>();
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            _inputsReader.PressAction += OnPress;
            _inputsReader.ReleaseAction += OnRelease;
            _ballSelectedEvent.OnRaised += OnBallSelected;
        }

        private void OnDisable()
        {
            _inputsReader.PressAction -= OnPress;
            _inputsReader.ReleaseAction -= OnRelease;
            _ballSelectedEvent.OnRaised -= OnBallSelected;
        }

        private void OnPress()
        {
            _mouseIsDownVariable.Value = true;
        }
        
        private void OnRelease()
        {
            if (_mouseIsDownVariable.Value == false) 
                return;
            
            StartCoroutine(AnimateAndMergeBalls());
            _mouseIsDownVariable.Value = false;
        }
        
        private void OnBallSelected(Ball ball)
        {
            // if (_mouseIsDownVariable.Value == false) 
            //     return;
            //
            // if (AddFirstBall(ball)) return;
            // if (CheckIsSameBall(ball)) return;
            //
            // var firstBall = _ballsSelected[0];
            // if (firstBall.Number != ball.Number) 
            //     return;
            //
            // _ballsSelected.Add(ball);
            //
            // if (CheckDistanceBetweenTwoLastBalls()) return;
            //
            // _lineRenderer.positionCount = _ballsSelected.Count;
            // _lineRenderer.SetPosition(_ballsSelected.Count - 1, ball.transform.position);
            
            if (_mouseIsDownVariable.Value == false) 
                return;
            
            if (_ballsSelected.Count == 0)
            {
                _ballsSelected.Add(ball);
                _lineRenderer.positionCount = 1;
                _lineRenderer.SetPosition(0, ball.transform.position);
                return;
            }
            
            if (_ballsSelected.Contains(ball) && _ballsSelected.Count > 1)
            {
                if (_ballsSelected[^2] == ball)
                {
                    _ballsSelected.Remove(_ballsSelected[^1]);
                    _lineRenderer.positionCount = _ballsSelected.Count;
                }
                
                return;
            }

            var firstNumBall = _ballsSelected[0];
            if (firstNumBall.Number != ball.Number) 
                return;
            
            _ballsSelected.Add(ball);
            
            if (_ballsSelected.Count >= 2)
            {
                var distance = Vector3.Distance(_ballsSelected[^2].transform.position, _ballsSelected[^1].transform.position);

                if (distance > _maxDistanceBetweenTwoBalls)
                {
                    _ballsSelected.Remove(_ballsSelected[^1]);
                    _lineRenderer.positionCount = _ballsSelected.Count;
                    return;
                }
            }
            
            _lineRenderer.positionCount = _ballsSelected.Count;
            
            var newYPosition = ball.transform.position;
            
            foreach (var selectedBall in _ballsSelected)
            {
                if (!(Mathf.Abs(selectedBall.transform.position.y - ball.transform.position.y) <= 0.2f)) 
                    continue;
                
                newYPosition.y = selectedBall.transform.position.y;
                break;
            }
            
            _lineRenderer.SetPosition(_ballsSelected.Count - 1, newYPosition);
        }
        
        #endregion

        #region Functions
        
        private bool AddFirstBall(Ball ball)
        {
            if (_ballsSelected.Count != 0) 
                return false;
            
            _ballsSelected.Add(ball);
            
            _lineRenderer.positionCount = 1;
            _lineRenderer.SetPosition(0, ball.transform.position);
            
            return true;
        }

        private bool CheckIsSameBall(Ball ball)
        {
            if (!_ballsSelected.Contains(ball) || _ballsSelected.Count <= 1) 
                return false;

            var lastBall = _ballsSelected[^1];
            var secondLastBall = _ballsSelected[^2];
            
            if (secondLastBall != ball) 
                return false;
            
            _ballsSelected.Remove(lastBall);
            _lineRenderer.positionCount = _ballsSelected.Count;
            
            return true;
        }
        
        private bool CheckDistanceBetweenTwoLastBalls()
        {
            if (_ballsSelected.Count <= 1) 
                return false;
            
            var lastBall = _ballsSelected[^1];
            var secondLastBall = _ballsSelected[^2];
            
            if (lastBall == null || secondLastBall == null)
                return false;
            
            var distanceBetweenBalls = Vector3.Distance(secondLastBall.transform.position, lastBall.transform.position);

            if (distanceBetweenBalls > _maxDistanceBetweenTwoBalls)
            {
                _ballsSelected.Remove(lastBall);
                _lineRenderer.positionCount = _ballsSelected.Count;
                return false;
            }

            return true;
        }
        
        private IEnumerator AnimateAndMergeBalls() 
        {
            if (_ballsSelected.Count < 3)
            {
                _ballsSelected.Clear();
                _lineRenderer.positionCount = 0;
                yield break;
            }
            
            SetBlockAllBalls(true);
            
            for (var i = 0; i < _ballsSelected.Count - 1; i++) 
            {
                var currentBall = _ballsSelected[i];
                var nextBall = _ballsSelected[i + 1];
                
                var duration = GameVar.BallMoveDuration;
                var elapsedTime = 0f;
                var startPosition = currentBall.transform.position;
                var endPosition = nextBall.transform.position;
                
                RemoveLineRendererFirstPosition(_lineRenderer);
                
                while (elapsedTime < duration) 
                {
                    currentBall.transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
                
                Destroy(currentBall.gameObject);
            }
            
            var lastNumBall = _ballsSelected[^1];
            lastNumBall.SetNum(lastNumBall.Number + 1);

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
        
        private static void RemoveLineRendererFirstPosition(LineRenderer lineRenderer) 
        {
            var positionsCount = lineRenderer.positionCount;
            
            if (positionsCount <= 1) 
            {
                lineRenderer.positionCount = 0;
                return;
            }

            var newPositions = new Vector3[positionsCount - 1];

            for (var i = 1; i < positionsCount; i++) 
            {
                newPositions[i - 1] = lineRenderer.GetPosition(i);
            }

            lineRenderer.positionCount = positionsCount - 1;
            lineRenderer.SetPositions(newPositions);
        }

        #endregion
    }
}
