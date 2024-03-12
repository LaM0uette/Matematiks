using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.HeaderScores
{
    public class HeaderScores : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<HeaderScores> { }
    
        private Label _gemNumberLabel;
        private Label _highscoreLabel;
        private Label _lastscoreLabel;
        private Label _lastscoreTitleLabel;
        
        public HeaderScores()
        {
            var rootAsset = Resources.Load<VisualTreeAsset>( "HeaderScores" );
            if( rootAsset == null )
                throw new System.Exception( $"Cannot create {nameof( HeaderScores )}! HeaderScores.uxml is missing !" );
            rootAsset.CloneTree( this );
        }
    }
}
