using System;
using Game.Modules.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.HeaderScores
{
    public class HeaderScores : VisualElement
    {
        #region Statements

        public new class UxmlFactory : UxmlFactory<HeaderScores> { }
        
        private const string _gemNumberKey = "GemScoreValue";
        private const string _highScoreKey = "HighScoreValue";
        private const string _highBallIconKey = "HighBallIcon";
        private const string _highBallKey = "HighBallValue";
    
        private Label _gemNumberLabel;
        private Label _highScoreLabel;
        
        private VisualElement _highBallIcon;
        private Label _highBallLabel;
        
        public HeaderScores()
        {
            var rootAsset = Resources.Load<VisualTreeAsset>( "HeaderScores" );
            if( rootAsset == null )
                throw new Exception( $"Cannot create {nameof( HeaderScores )}! HeaderScores.uxml is missing !" );
            rootAsset.CloneTree( this );
            
            RegisterCallback<AttachToPanelEvent>(OnAttachedToPanel);
        }
        
        private void OnAttachedToPanel( AttachToPanelEvent evt )
        {
            SetElements();
        }

        #endregion
        
        #region Functions
        
        public void UpdateHeaderScore()
        {
            _gemNumberLabel.text = Saver.Gem.LoadInt().ToString();
            _highScoreLabel.text = Saver.HighScore.LoadInt().ToString();
        }
        
        public void UpdateGem(int value)
        {
            _gemNumberLabel.text = value.ToString();
        }
        
        public void UpdateHighScore(int value)
        {
            _highScoreLabel.text = value.ToString();
        }
        
        public void UpdateHighBall(int highBall)
        {
            var color = GetBallColor(highBall);
            var textColor = GetContrastingTextColor(color);
            
            _highBallIcon.style.unityBackgroundImageTintColor = color;
            _highBallLabel.text = highBall.ToString();
            _highBallLabel.style.color = textColor;
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

        private void SetElements()
        {
            _gemNumberLabel = this.Q<Label>(_gemNumberKey);
            _highScoreLabel = this.Q<Label>(_highScoreKey);
            
            _highBallIcon = this.Q<VisualElement>(_highBallIconKey);
            _highBallLabel = this.Q<Label>(_highBallKey);
        }

        #endregion
    }
}
