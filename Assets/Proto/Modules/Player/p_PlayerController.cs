using Obvious.Soap;
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
        }

        private void OnDisable()
        {
            _inputsReader.ReleaseAction -= OnRelease;
        }

        #endregion

        #region Functions
        
        private void OnRelease()
        {
            _isDownVariable.Value = false;
        }

        #endregion
    }
}
