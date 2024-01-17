using System.Linq;
using Game.Modules.Manager;
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
            var randomNumber = GetWeightedRandomNumber();
            SetNum(randomNumber);
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
            if (number < 1)
            {
                number = 1;
            }
            
            Number = number;
            _tmpNumber.text = number.ToString();
        }

        private static int GetWeightedRandomNumber()
        {
            var ballNumbers = GameManager.Instance.BallNumbers;
            var totalWeight = ballNumbers.Where(ballNumber => !ballNumber.IsLocked).Sum(ballNumber => ballNumber.Weight);

            var randomNumber = Random.Range(1, totalWeight + 1);
            var sum = 0f;
            
            foreach (var ballNumber in ballNumbers)
            {
                if (ballNumber.IsLocked) 
                    continue;
                
                sum += ballNumber.Weight;
                if (randomNumber <= sum)
                {
                    return ballNumber.Number;
                }
            }

            return -1;
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
