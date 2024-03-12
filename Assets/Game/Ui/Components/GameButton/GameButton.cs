using UnityEngine;
using UnityEngine.UIElements;

public class GameButton : Button
{
    public new class UxmlFactory : UxmlFactory<GameButton, UxmlTraits> { }
    
    public new class UxmlTraits : Button.UxmlTraits
    {
        private readonly UxmlStringAttributeDescription _iconName = new() { name = "icon-name" };

        public override void Init( VisualElement ve, IUxmlAttributes bag, CreationContext cc )
        {
            base.Init( ve, bag, cc );

            if( ve is GameButton gameButton )
                gameButton.IconName = _iconName.GetValueFromBag( bag, cc );
        }
    }

    public string IconName { get; set; }

    public GameButton()
    {
        RegisterCallback<AttachToPanelEvent>( OnAttachedToPanel );
    }
    
    private void OnAttachedToPanel( AttachToPanelEvent evt )
    {
        AddStyleSheet();
        AddIcon();
    }

    private void AddStyleSheet()
    {
        var styleSheet = Resources.Load<StyleSheet>( "GameButton" );
        styleSheets.Add( styleSheet );
        AddToClassList( "game-button" );
    }

    private void AddIcon()
    {
        var icon = Resources.Load<VectorImage>( IconName );
        if( icon == null )
            return;

        VisualElement iconElement = new() { name = "Icon", pickingMode = PickingMode.Ignore };
        iconElement.AddToClassList( "game-button__icon" );
        iconElement.style.backgroundImage = new StyleBackground( icon );
        Add( iconElement );
    }
}
