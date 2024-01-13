using Obvious.Soap;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Proto.Modules.NumBall
{
    public class p_NumBall : MonoBehaviour
    {
        #region Statements
        
        [FormerlySerializedAs("num")] [Space, Title("Properties")]
        public int Num;

        [Space, Title("Soap")]
        [SerializeField] private BoolVariable _isDownVariable;
        [SerializeField] private ScriptableEventP_NumBall _numBallSelectedEvent;

        #endregion
        
        #region Events
        
        private void OnMouseDown()
        {
            _isDownVariable.Value = true;
        }
        
        private void OnMouseEnter()
        {
            if (_isDownVariable.Value == false) return;
            
            _numBallSelectedEvent.Raise(this);
        }

        #endregion
    }
}
