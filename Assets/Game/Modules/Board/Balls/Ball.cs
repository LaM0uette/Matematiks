using Obvious.Soap;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game.Modules.Board.Balls
{
    public class Ball : MonoBehaviour
    {
        #region Statements
        
        public bool IsBlocked { get; set; }
        [ShowInInspector, ReadOnly] public int Number { get; set; }
        
        [Space, Title("TMP")]
        [SerializeField] private TMP_Text _tmpNumber;

        [Space, Title("Soap")]
        [SerializeField] private BoolVariable _mouseIsDownVariable;
        [SerializeField] private ScriptableEventBall _ballSelectedEvent;

        private void Start()
        {
            var randomInt = Random.Range(1, 4);
            SetNum(1);
        }

        #endregion
        
        #region Events
        
        private void OnMouseDown()
        {
            OnBallSelected();
        }
        
        private void OnMouseEnter()
        {
            OnBallSelected();
        }

        #endregion

        #region Functions

        public void SetNum(int number)
        {
            Number = number;
            _tmpNumber.text = number.ToString();
        }

        private void OnBallSelected()
        {
            if (_mouseIsDownVariable.Value == false) 
                return;
            
            _ballSelectedEvent.Raise(this);
        }

        #endregion
    }
}
