using Obvious.Soap;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Proto.Modules.NumBall
{
    public class p_NumBall : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Properties")]
        public int Num;
        
        [Space, Title("TMP")]
        [SerializeField] private TMP_Text _numText;

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

        #region Functions

        public void SetNum(int num)
        {
            Num = num;
            _numText.text = num.ToString();
        }

        #endregion
    }
}
