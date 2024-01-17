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

        public void SetNum(int number)
        {
            if (number < 1)
            {
                number = 1;
            }
            
            Number = number;
            
            _spriteRenderer.color = GetBallColor(number);
            _tmpNumber.color = GetContrastingTextColor(_spriteRenderer.color);
            _tmpNumber.text = number.ToString();
        }

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
        
        private static Color GetBallColor(int number) 
        {
            if (number <= 3) return Color.white;
            if (number >= 99) return Color.black;

            var hue = (number - 1) / 99f;
            const float saturation = 0.7f;
            const float value = 0.8f;

            return Color.HSVToRGB(hue, saturation, value);
        }
        
        private static Color GetContrastingTextColor(Color backgroundColor)
        {
            var luminance = 0.2126f * backgroundColor.r + 0.7152f * backgroundColor.g + 0.0722f * backgroundColor.b;
            return luminance > 0.5f ? Color.black : Color.white;
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
