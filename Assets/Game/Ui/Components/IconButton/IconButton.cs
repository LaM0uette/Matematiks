using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.Components.IconButton
{
    public class IconButton : Button
    {
        #region Statements

        public new class UxmlFactory : UxmlFactory<IconButton> { }

        public IconButton()
        {
            RegisterCallback<AttachToPanelEvent>( OnAttachedToPanel );
        }
    
        private void OnAttachedToPanel( AttachToPanelEvent evt )
        {
            AddStyleSheet();
        }

        #endregion
        
        #region Functions

        private void AddStyleSheet()
        {
            var styleSheet = Resources.Load<StyleSheet>( "IconButton" );
            styleSheets.Add( styleSheet );
            AddToClassList( "icon-button" );
        }

        #endregion
    }
}
