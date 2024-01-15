using System;
using UnityEngine;

namespace Game.Modules.Player.Inputs
{
    public class PlayerInputsReader : MonoBehaviour
    {
        #region Statements

        public Action PressAction { get; set; }
        public Action ReleaseAction { get; set; }

        #endregion

        #region Events
        
        public void OnPress()
        {
            PressAction?.Invoke();
        }

        public void OnRelease()
        {
            ReleaseAction?.Invoke();
        }
        
        #endregion
    }
}
