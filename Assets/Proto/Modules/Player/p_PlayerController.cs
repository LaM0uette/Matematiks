using System.Linq;
using Obvious.Soap;
using Proto.Modules.NumBall;
using Proto.Modules.Player.Inputs;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Proto.Modules.Player
{
    public class p_PlayerController : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Properties")]
        [SerializeField] private float _maxDistanceBetweenNumBalls = 1.3f;
        
        [Space, Title("Soap")]
        [SerializeField] private BoolVariable _isDownVariable;
        [SerializeField] private ScriptableEventP_NumBall _numBallSelectedEvent;
        [SerializeField] private ScriptableListP_NumBall _numBallsSelected;

        private p_PlayerInputsReader _inputsReader;
        private LineRenderer _lineRenderer;

        private void Awake()
        {
            _inputsReader = GetComponent<p_PlayerInputsReader>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        #endregion

        #region Events

        private void OnEnable()
        {
            _inputsReader.ReleaseAction += OnRelease;
            _numBallSelectedEvent.OnRaised += OnNumBallSelected;
        }

        private void OnDisable()
        {
            _inputsReader.ReleaseAction -= OnRelease;
            _numBallSelectedEvent.OnRaised -= OnNumBallSelected;
        }

        #endregion

        #region Functions
        
        private void OnRelease()
        {
            if (_isDownVariable.Value == false) 
                return;
            
            if (_numBallsSelected.Count >= 3)
            {
                var lastNumBall = _numBallsSelected[^1];
                var numBallsToDelete = _numBallsSelected.Take(_numBallsSelected.Count - 1).ToList();
                
                foreach (var numBall in numBallsToDelete)
                {
                    Destroy(numBall.gameObject);
                }
                
                lastNumBall.SetNum(lastNumBall.Num + 1);
            }
            
            _isDownVariable.Value = false;
            _numBallsSelected.Clear();
            _lineRenderer.positionCount = 0;
        }
        
        private void OnNumBallSelected(p_NumBall pNumBall)
        {
            if (_isDownVariable.Value == false) 
                return;
            
            if (_numBallsSelected.Count == 0)
            {
                _numBallsSelected.Add(pNumBall);
                _lineRenderer.positionCount = 1;
                _lineRenderer.SetPosition(0, pNumBall.transform.position);
                return;
            }
            
            if (_numBallsSelected.Contains(pNumBall))
            {
                if (_numBallsSelected[^2] == pNumBall)
                {
                    _numBallsSelected.Remove(_numBallsSelected[^1]);
                    _lineRenderer.positionCount = _numBallsSelected.Count;
                }
                
                return;
            }

            var firstNumBall = _numBallsSelected[0];
            if (firstNumBall.Num != pNumBall.Num) 
                return;
            
            _numBallsSelected.Add(pNumBall);
            
            if (_numBallsSelected.Count >= 2)
            {
                var distance = Vector3.Distance(_numBallsSelected[^2].transform.position, _numBallsSelected[^1].transform.position);

                if (distance > _maxDistanceBetweenNumBalls)
                {
                    _numBallsSelected.Remove(_numBallsSelected[^1]);
                    _lineRenderer.positionCount = _numBallsSelected.Count;
                    return;
                }
            }
            
            _lineRenderer.positionCount = _numBallsSelected.Count;
            _lineRenderer.SetPosition(_numBallsSelected.Count - 1, pNumBall.transform.position);
        }

        #endregion
    }
}
