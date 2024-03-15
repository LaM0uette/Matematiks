using Game.Modules.Board;
using Game.Modules.Manager;
using Game.Modules.Player.Inputs;
using UnityEngine;

namespace Game.Modules.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Statements

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

        private static void OnPress()
        {
            if (BoardHandler.OngoingAction || BonusManager.CurrentBonus != null) 
                return;
            
            BoardHandler.IsPressing = true;
        }
        
        private static void OnRelease()
        {
            if (BoardHandler.IsPressing == false || BonusManager.CurrentBonus != null) 
                return;

            BoardHandler.IsPressing = false;
            BoardEvents.ReleaseEvent?.Invoke();
        }

        #endregion
    }
}
