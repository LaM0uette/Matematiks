using Game.Modules.Player.Inputs;
using Obvious.Soap;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Soap")]
        [SerializeField] private ScriptableEventNoParam _releaseEvent;
        [SerializeField] private BoolVariable _mouseDownVariable;
        [SerializeField] private BoolVariable _ongoingAction;

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
        }

        private void OnDisable()
        {
            _inputsReader.PressAction -= OnPress;
            _inputsReader.ReleaseAction -= OnRelease;
        }
        
        #endregion

        #region InputsReaderEvents

        private void OnPress()
        {
            if (_ongoingAction.Value) 
                return;
            
            _mouseDownVariable.Value = true;
        }
        
        private void OnRelease()
        {
            if (_mouseDownVariable.Value == false) 
                return;
            
            _mouseDownVariable.Value = false;
            _releaseEvent.Raise();
        }

        #endregion
    }
}
