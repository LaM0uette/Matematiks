using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.Components.BonusCard
{
    public class BonusCard : Button
    {
        #region Statements

        public new class UxmlFactory : UxmlFactory<BonusCard, UxmlTraits> { }
    
        public new class UxmlTraits : Button.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _iconName = new() { name = "icon-name" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                if (ve is not BonusCard bonusCard) 
                    return;
                
                bonusCard.IconName = _iconName.GetValueFromBag(bag, cc);
            }
        }

        private const string _gemIconKey = "Icons/diamond";
        
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
        private VisualElement _priceIconElement;
        private Label _priceLabel;
        
        public BonusCard()
        {
            AddStyleSheet();
            CreateIcon();
            CreatePrice();
        }

        #endregion

        #region Functions

        public void SetPrice(int value)
        {
            _priceLabel.text = value.ToString();
        }
        
        public void Select() => AddToClassList("bonus-card--selected");
        public void Unselect() => RemoveFromClassList("bonus-card--selected");
        public void Show() => style.display = DisplayStyle.Flex;
        public void Hide() => style.display = DisplayStyle.None;
        
        private void AddStyleSheet()
        {
            var styleSheet = Resources.Load<StyleSheet>( "BonusCard");
            styleSheets.Add(styleSheet);
            AddToClassList("bonus-card");
        }

        private void CreateIcon()
        {
            _iconElement = new VisualElement { name = "Icon", pickingMode = PickingMode.Ignore };
            _iconElement.AddToClassList("bonus-card__icon");
            Add(_iconElement);
        }
        
        private void CreatePrice()
        {
            var priceContainerElement = new VisualElement { name = "PriceContainer", pickingMode = PickingMode.Ignore };
            priceContainerElement.AddToClassList("price-container");

            _priceIconElement = new VisualElement { name = "PriceIcon", pickingMode = PickingMode.Ignore };
            var icon = Resources.Load<VectorImage>(_gemIconKey);
            _priceIconElement.style.backgroundImage = new StyleBackground(icon);
            _priceIconElement.AddToClassList("price-container__icon");
            
            _priceLabel = new Label { name = "PriceLabel", pickingMode = PickingMode.Ignore };
            _priceLabel.AddToClassList("price-container__value");
            
            priceContainerElement.Add(_priceIconElement);
            priceContainerElement.Add(_priceLabel);
            Add(priceContainerElement);
        }

        #endregion
    }
}
