using System.Collections;
using Game.Modules.Board.Balls;
using Game.Modules.Manager;
using Game.Modules.Player.Inputs;
using Game.Modules.Utils;
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
        [SerializeField] private ScriptableEventNoParam _mergeBallsEvent;
        [SerializeField] private ScriptableEventBall _ballSelectedEvent;
        [SerializeField] private ScriptableListBall _ballsSelected;
        [SerializeField] private BoolVariable _mouseIsDownVariable;
        [SerializeField] private BoolVariable _isInAnimationVariable;
        [SerializeField] private IntVariable _scoreVariable;

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
            if (_isInAnimationVariable.Value) 
                return;
            
            _mouseIsDownVariable.Value = true;
        }
        
        private void OnRelease()
        {
            if (_mouseIsDownVariable.Value == false) 
                return;
            
            _mouseIsDownVariable.Value = false;
            _mergeBallsEvent.Raise();
        }
        
        private void OnBallSelected(Ball ball)
        {
            if (_mouseIsDownVariable.Value == false) 
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

                if (distance > _maxDistanceBetweenTwoBalls)
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
