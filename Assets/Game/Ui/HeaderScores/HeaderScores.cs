using System;
using Game.Modules.Save;
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
        private const string _highBallBorderIconKey = "HighBallIconBorder";
        private const string _highBallIconKey = "HighBallIcon";
        private const string _highBallKey = "HighBallValue";
    
        private Label _gemNumberLabel;
        private Label _highScoreLabel;
        
        private VisualElement _highBallBorderIcon;
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
            _gemNumberLabel.text = Saver.Gem.Load().ToString();
            _highScoreLabel.text = Saver.HighScore.Load().ToString();
            UpdateHighBall(Saver.HighBall.Load());
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
            var borderColor = GetBallBorderColor(highBall);
            var textColor = GetContrastingTextColor(color);
            
            _highBallIcon.style.unityBackgroundImageTintColor = color;
            _highBallBorderIcon.style.unityBackgroundImageTintColor = borderColor;
            _highBallLabel.text = highBall.ToString();
            _highBallLabel.style.color = textColor;
        }
        
        private static Color GetBallColor(int number)
        {
            if (number % 10 == 0)
                return ColorVar.Ball10Color;

            var colorIndex = number % 10;

            return colorIndex switch
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
                _ => ColorVar.Ball10Color
            };
        }

        private static Color GetBallBorderColor(int number)
        {
            return number switch
            {
                >= 1 and <= 10 => new Color(0, 0, 0, 0),
                >= 11 and <= 20 => ColorVar.Ball2Color,
                >= 21 and <= 30 => ColorVar.Ball3Color,
                >= 31 and <= 40 => ColorVar.Ball4Color,
                >= 41 and <= 50 => ColorVar.Ball5Color,
                >= 51 and <= 60 => ColorVar.Ball6Color,
                >= 61 and <= 70 => ColorVar.Ball7Color,
                >= 71 and <= 80 => ColorVar.Ball8Color,
                >= 81 and <= 90 => ColorVar.Ball9Color,
                >= 91 and <= 100 => ColorVar.Ball10Color,
                _ => ColorVar.Ball10Color
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
            
            _highBallBorderIcon = this.Q<VisualElement>(_highBallBorderIconKey);
            _highBallIcon = this.Q<VisualElement>(_highBallIconKey);
            _highBallLabel = this.Q<Label>(_highBallKey);
        }

        #endregion
    }
}
