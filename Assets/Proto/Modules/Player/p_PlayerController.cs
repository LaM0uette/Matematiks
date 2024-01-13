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
        
        [Space, Title("Soap")]
        [SerializeField] private BoolVariable _isDownVariable;
        [SerializeField] private ScriptableEventP_NumBall _numBallSelectedEvent;
        [SerializeField] private ScriptableListP_NumBall _numBallsSelected;

        private p_PlayerInputsReader _inputsReader;

        private void Awake()
        {
            _inputsReader = GetComponent<p_PlayerInputsReader>();
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
            _isDownVariable.Value = false;
            _numBallsSelected.Clear();
        }
        
        private void OnNumBallSelected(p_NumBall pNumBall)
        {
            if (_isDownVariable.Value == false) return;
            
            if (_numBallsSelected.Count == 0)
            {
                _numBallsSelected.Add(pNumBall);
                return;
            }

            var firstNumBall = _numBallsSelected[0];
            if (firstNumBall.Num != pNumBall.Num) 
                return;
            
            _numBallsSelected.Add(pNumBall);
        }

        #endregion
    }
}
