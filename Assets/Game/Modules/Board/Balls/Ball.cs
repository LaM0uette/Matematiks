using Game.Modules.Events;
using Game.Modules.Manager;
using Game.Modules.Utils;
using Game.Ui;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game.Modules.Board.Balls
{
    public class Ball : MonoBehaviour
    {
        #region Statements
        
        [Title("Properties")]
        [ShowInInspector, ReadOnly] public int Number { get; set; }
        
        public Color Color { get; private set; }
        public bool IsVisited { get; set; }
        
        [Space, Title("Ui")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TMP_Text _tmpNumber;

        [Space, Title("Feedbacks")]
        [SerializeField] private MMF_Player _selectedFeedback;

        private void Start()
        {
            InitFeedbacks();
        }
        
        private void InitFeedbacks()
        {
            _selectedFeedback.Initialization();
        }

        #endregion
        
        #region Events
        
        private void OnMouseDown()
        {
            if (BonusManager.CurrentBonus == null)
            {
                OnBallSelected();
                return;
            }
            
            if (BoardHandler.OngoingAction)
            {
                BonusManager.CurrentBonus = null;
                UiEvents.RefreshUiEvent.Invoke();
                return;
            }
            
            var bonusId = BonusManager.CurrentBonus.Id;
            
            switch (bonusId)
            {
                case 0: Bonus01(); break;
                case 1 when Number <= 1: return;
                case 1: Bonus02(); break;
                case 2: Bonus03(); break;
                case 3: Bonus04(); break;
            }
            
            var bonusData = BonusManager.CurrentBonus;
            var gem = Saver.Gem.LoadInt();
            gem -= bonusData.Cost;
            Saver.Gem.Save(gem);
            GameEvents.GemEvent.Invoke(gem);
            
            BonusManager.CurrentBonus = null;
            UiEvents.RefreshUiEvent.Invoke();
        }
        
        private void OnMouseEnter()
        {
            OnBallSelected();
        }

        #endregion

        #region MouseEvents

        private void OnBallSelected()
        {
            if (BoardHandler.IsPressing == false) 
                return;
            
            BoardEvents.CurrentBallSelectedEvent.Invoke(this);
        }

        #endregion

        #region Functions

        public void SetNum(int number)
        {
            switch (number)
            {
                case > 99: return;
                case < 1: number = 1; break;
            }

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
            if (number % 10 == 0)
                return ColorVar.Ball10Color;

            var colorIndex = number % 10;
            var isDark = number > 10;

            return colorIndex switch
            {
                1 => isDark ? ColorVar.Ball1ColorDark : ColorVar.Ball1Color,
                2 => isDark ? ColorVar.Ball2ColorDark : ColorVar.Ball2Color,
                3 => isDark ? ColorVar.Ball3ColorDark : ColorVar.Ball3Color,
                4 => isDark ? ColorVar.Ball4ColorDark : ColorVar.Ball4Color,
                5 => isDark ? ColorVar.Ball5ColorDark : ColorVar.Ball5Color,
                6 => isDark ? ColorVar.Ball6ColorDark : ColorVar.Ball6Color,
                7 => isDark ? ColorVar.Ball7ColorDark : ColorVar.Ball7Color,
                8 => isDark ? ColorVar.Ball8ColorDark : ColorVar.Ball8Color,
                9 => isDark ? ColorVar.Ball9ColorDark : ColorVar.Ball9Color,
                _ => ColorVar.Ball10ColorDark
            };
        }
        
        private static Color GetContrastingTextColor(Color backgroundColor)
        {
            var luminance = 0.2126f * backgroundColor.r + 0.7152f * backgroundColor.g + 0.0722f * backgroundColor.b;
            return luminance > 0.5f ? ColorVar.BlackColor : ColorVar.WhiteColor;
        }
        
        private void Bonus01()
        {
            Destroy(gameObject);
        }
        
        private void Bonus02()
        {
            SetNum(--Number);
        }
        
        private void Bonus03()
        {
            SetNum(++Number);
            CheckForUpdateHighBall(Number);
        }
        
        private void Bonus04()
        {
            var balls = FindObjectsOfType<Ball>();
            foreach (var ball in balls)
            {
                if (ball.Number == Number)
                    Destroy(ball.gameObject);
            }
        }
        
        private static void CheckForUpdateHighBall(int ballNumber)
        {
            if (ballNumber <= Saver.HighBall.LoadInt())
                return;
            
            Saver.HighBall.Save(ballNumber);
            GameEvents.HighBallEvent.Invoke(ballNumber);
        }

        #endregion

        #region Feedbacks

        public void PlaySelectedFeedback()
        {
            _selectedFeedback.PlayFeedbacks();
        }

        #endregion
        
        #region Odin

        [Button]
        public void SetNumAndColor()
        {
            var number = Number + 1;
            SetNum(number);
        }

        #endregion
    }
}
