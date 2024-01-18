using System;
using System.Linq;
using Game.Modules.Manager;
using Game.Modules.Utils;
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
        public bool IsVisited { get; set; }
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
            
            if (number > GameManager.Instance.MaxBallNumber)
            {
                GameManager.Instance.MaxBallNumber = number;
                GameManager.Instance.BallScore.SetNum(GameManager.Instance.MaxBallNumber);
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
            return number switch
            {
                1 => ColorVar.WhiteColor,
                2 => new Color32(152, 213, 235, 255),
                3 => new Color32(179, 238, 179, 255),
                4 => new Color32(255, 247, 146, 255),
                5 => new Color32(255, 197, 128, 255),
                6 => new Color32(192, 99, 101, 255),
                7 => new Color32(152, 140, 203, 255),
                8 => new Color32(44, 71, 131, 255),
                _ => new Color32(5, 10, 21, 255)
            };
        }
        
        private static Color GetContrastingTextColor(Color backgroundColor)
        {
            var luminance = 0.2126f * backgroundColor.r + 0.7152f * backgroundColor.g + 0.0722f * backgroundColor.b;
            return luminance > 0.5f ? ColorVar.BlackColor : ColorVar.WhiteColor;
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
