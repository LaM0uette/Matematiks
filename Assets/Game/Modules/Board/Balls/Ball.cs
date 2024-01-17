using System;
using System.Linq;
using Game.Modules.Manager;
using Obvious.Soap;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Modules.Board.Balls
{
    public class Ball : MonoBehaviour
    {
        #region Statements
        
        public bool IsBlocked { get; set; }
        [ShowInInspector, ReadOnly] public int Number { get; set; }
        
        [Space, Title("Ui")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
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
        
        private static int GetWeightedRandomNumber()
        {
            var ballNumbers = GameManager.Instance.BallNumbers;
            var totalWeight = ballNumbers.Where(ballNumber => !ballNumber.IsLocked).Sum(ballNumber => ballNumber.Weight);

            var randomNumber = Random.Range(0, totalWeight + 1);
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

        public void SetNum(int number)
        {
            if (number < 1)
            {
                number = 1;
            }
            
            Number = number;
            SetBallColor(number);
        }

        private void SetBallColor(int number)
        {
            _spriteRenderer.color = GetBallColor(number);
            _tmpNumber.color = GetContrastingTextColor(_spriteRenderer.color);
            _tmpNumber.text = number.ToString();
        }
        
        private static Color GetBallColor(int number) 
        {
            if (number <= 3) return new Color32(244, 242, 243, 255);
            if (number >= 9) return new Color32(4, 4, 10, 255);

            return number switch
            {
                4 => new Color32(64, 143, 178, 255),
                5 => new Color32(65, 181, 129, 255),
                6 => new Color32(212, 204, 34, 255),
                7 => new Color32(196, 95, 84, 255),
                8 => new Color32(192, 113, 205, 255),
                _ => throw new Exception("Invalid number")
            };
        }
        
        private static Color GetContrastingTextColor(Color backgroundColor)
        {
            var luminance = 0.2126f * backgroundColor.r + 0.7152f * backgroundColor.g + 0.0722f * backgroundColor.b;
            return luminance > 0.5f ? new Color32(4, 4, 10, 255) : new Color32(244, 242, 243, 255);
        }

        private void OnBallSelected()
        {
            if (_mouseIsDownVariable.Value == false) 
                return;
            
            _ballSelectedEvent.Raise(this);
        }

        #endregion

        #region Odin

        [Button]
        public void SetNumAndColor()
        {
            Number += 1;
            SetNum(Number);
        }

        #endregion
    }
}
