using System;
using UnityEngine;

namespace Proto.Modules.Player.Inputs
{
    public class p_PlayerInputsReader : MonoBehaviour
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
