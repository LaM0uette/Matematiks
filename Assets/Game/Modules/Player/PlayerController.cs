using Game.Modules.Board.Balls;
using Game.Modules.Bonus;
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
            if (BoardManager.OngoingAction || BonusManager.CurrentBonus != null) 
                return;
            
            BoardManager.IsPressing = true;
        }
        
        private static void OnRelease()
        {
            if (BoardManager.IsPressing == false || BonusManager.CurrentBonus != null) 
                return;

            BoardManager.IsPressing = false;
            BallEvents.ReleaseEvent?.Invoke();
        }

        #endregion
    }
}
