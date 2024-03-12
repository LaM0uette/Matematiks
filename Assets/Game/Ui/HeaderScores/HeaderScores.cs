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
        private const string _highBallKey = "HighBallValue";
    
        private Label _gemNumberLabel;
        private Label _highScoreLabel;
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
        
        public void UpdateScore()
        {
            _gemNumberLabel.text = Saver.Gem.LoadInt().ToString();
            _highScoreLabel.text = Saver.HighScore.LoadInt().ToString();
        }

        private void SetElements()
        {
            _gemNumberLabel = this.Q<Label>(_gemNumberKey);
            _highScoreLabel = this.Q<Label>(_highScoreKey);
            _highBallLabel = this.Q<Label>(_highBallKey);
        }

        #endregion
    }
}
