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
    }
}
