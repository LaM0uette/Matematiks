using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Modules.Player.Inputs
{
    public class PlayerInputsReader : MonoBehaviour
    {
        #region Statements
        
        private InputActionAsset _inputActions;
        private InputAction _leftClickAction;

        public Action PressAction { get; set; }
        public Action ReleaseAction { get; set; }

        private void Awake()
        {
            _inputActions = GetComponent<PlayerInput>().actions;
            
            _leftClickAction = _inputActions.FindAction("Click");
            _leftClickAction.performed += OnPress;
            _leftClickAction.canceled += OnRelease;
            _leftClickAction.Enable();
        }

        private void OnPress(InputAction.CallbackContext obj)
        {
            PressAction?.Invoke();
        }
        
        private void OnRelease(InputAction.CallbackContext obj)
        {
            ReleaseAction?.Invoke();
        }

        #endregion
    }
}
