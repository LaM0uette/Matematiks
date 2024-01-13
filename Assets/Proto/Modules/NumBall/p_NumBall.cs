using Obvious.Soap;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Proto.Modules.NumBall
{
    public class p_NumBall : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Properties")]
        public int num;

        [Space, Title("Soap")]
        [SerializeField] private BoolVariable _isDownVariable;

        #endregion
        
        #region Events
        
        private void OnMouseDown()
        {
            _isDownVariable.Value = true;
        }
        
        private void OnMouseEnter()
        {
            Debug.Log("Enter");
        }

        #endregion
    }
}
