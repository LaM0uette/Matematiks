using Game.Modules.Utils;
using Obvious.Soap;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game.Modules.Board.Balls
{
    public class Ball : MonoBehaviour
    {
        #region Statements
        
        public bool IsVisited { get; set; }
        public Color Color { get; private set; }
        
        [Title("Number")]
        [ShowInInspector, ReadOnly] public int Number { get; set; }
        
        [Space, Title("Ui")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TMP_Text _tmpNumber;

        [Space, Title("Soap")]
        [SerializeField] private BoolVariable _mouseIsDownVariable;
        [SerializeField] private ScriptableEventBall _ballSelectedEvent;

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

        #region MouseEvents

        private void OnBallSelected()
        {
            if (_mouseIsDownVariable.Value == false) 
                return;
            
            _ballSelectedEvent.Raise(this);
        }

        #endregion

        #region Functions

        public void SetNum(int number)
        {
            if (number > 99) 
                return;
            
            if (number < 1) number = 1;
            
            Number = number;
            SetBallColor(number);
        }

        private void SetBallColor(int number)
        {
            var color = GetBallColor(number);
            _spriteRenderer.color = color;
            Color = color;
            
            _tmpNumber.color = GetContrastingTextColor(_spriteRenderer.color);
            _tmpNumber.text = number.ToString();
        }
        
        private static Color GetBallColor(int number)
        {
            return number switch
            {
                1 => ColorVar.Ball1Color,
                2 => ColorVar.Ball2Color,
                3 => ColorVar.Ball3Color,
                4 => ColorVar.Ball4Color,
                5 => ColorVar.Ball5Color,
                6 => ColorVar.Ball6Color,
                7 => ColorVar.Ball7Color,
                8 => ColorVar.Ball8Color,
                9 => ColorVar.Ball9Color,
                10 or 20 or 30 or 40 or 50 or 60 or 70 or 80 or 90 => ColorVar.Ball10Color,
                11 or 21 or 31 or 41 or 51 or 61 or 71 or 81 or 91 => ColorVar.Ball1ColorDark,
                12 or 22 or 32 or 42 or 52 or 62 or 72 or 82 or 92 => ColorVar.Ball2ColorDark,
                13 or 23 or 33 or 43 or 53 or 63 or 73 or 83 or 93 => ColorVar.Ball3ColorDark,
                14 or 24 or 34 or 44 or 54 or 64 or 74 or 84 or 94 => ColorVar.Ball4ColorDark,
                15 or 25 or 35 or 45 or 55 or 65 or 75 or 85 or 95 => ColorVar.Ball5ColorDark,
                16 or 26 or 36 or 46 or 56 or 66 or 76 or 86 or 96 => ColorVar.Ball6ColorDark,
                17 or 27 or 37 or 47 or 57 or 67 or 77 or 87 or 97 => ColorVar.Ball7ColorDark,
                18 or 28 or 38 or 48 or 58 or 68 or 78 or 88 or 98 => ColorVar.Ball8ColorDark,
                19 or 29 or 39 or 49 or 59 or 69 or 79 or 89 or 99 => ColorVar.Ball9ColorDark,
                _ => ColorVar.Ball10ColorDark
            };
        }
        
        private static Color GetContrastingTextColor(Color backgroundColor)
        {
            var luminance = 0.2126f * backgroundColor.r + 0.7152f * backgroundColor.g + 0.0722f * backgroundColor.b;
            return luminance > 0.5f ? ColorVar.BlackColor : ColorVar.WhiteColor;
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
