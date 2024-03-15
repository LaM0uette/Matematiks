using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.Components.GameButton
{
    public class GameButton : Button
    {
        #region Statements

        public new class UxmlFactory : UxmlFactory<GameButton, UxmlTraits> { }
    
        public new class UxmlTraits : Button.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _iconName = new() { name = "icon-name" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                if(ve is GameButton gameButton)
                    gameButton.IconName = _iconName.GetValueFromBag(bag, cc);
            }
        }

        private string _iconName;
        public string IconName
        {
            get => _iconName;
            set
            {
                var icon = Resources.Load<VectorImage>(value);
            
                if(icon == null)
                    return;
            
                _iconName = value;
                _iconElement.style.backgroundImage = new StyleBackground(icon);
            }
        }

        private VisualElement _iconElement;

        public GameButton()
        {
            AddStyleSheet();
            CreateIcon();
        }
        
        #endregion

        #region Functions

        private void AddStyleSheet()
        {
            var styleSheet = Resources.Load<StyleSheet>("GameButton");
            styleSheets.Add(styleSheet);
            AddToClassList("game-button");
        }

        private void CreateIcon()
        {
            _iconElement = new VisualElement { name = "Icon", pickingMode = PickingMode.Ignore };
            _iconElement.AddToClassList("game-button__icon");
            Add(_iconElement);
        }

        #endregion
    }
}
