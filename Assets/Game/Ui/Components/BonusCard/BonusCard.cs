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
            private readonly UxmlIntAttributeDescription _price = new() { name = "price" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                if (ve is not BonusCard bonusCard) 
                    return;
                
                bonusCard.IconName = _iconName.GetValueFromBag(bag, cc);
                bonusCard.Price = _price.GetValueFromBag(bag, cc);
            }
        }

        private const string _gemIconKey = "diamond";
        
        public string IconName { get; set; }
        public int Price { get; set; }

        public BonusCard()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachedToPanel);
        }
    
        private void OnAttachedToPanel(AttachToPanelEvent evt)
        {
            AddStyleSheet();
            AddIcon();
            AddPrice();
        }

        #endregion

        #region Functions
        
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

        private void AddIcon()
        {
            var icon = Resources.Load<VectorImage>(IconName);
            if(icon == null)
                return;

            VisualElement iconElement = new() { name = "Icon", pickingMode = PickingMode.Ignore };
            iconElement.AddToClassList("bonus-card__icon");
            iconElement.style.backgroundImage = new StyleBackground(icon);
            Add(iconElement);
        }
        
        private void AddPrice()
        {
            VisualElement priceContainerElement = new() { name = "PriceContainer", pickingMode = PickingMode.Ignore };
            priceContainerElement.AddToClassList("price-container");
            
            var icon = Resources.Load<VectorImage>(_gemIconKey);
            if(icon == null)
                return;

            VisualElement iconElement = new() { name = "PriceIcon", pickingMode = PickingMode.Ignore };
            iconElement.AddToClassList("price-container__icon");
            iconElement.style.backgroundImage = new StyleBackground(icon);
            
            Label priceLabel = new() { name = "PriceLabel", pickingMode = PickingMode.Ignore };
            priceLabel.AddToClassList("price-container__value");
            priceLabel.text = Price.ToString();
            
            priceContainerElement.Add(iconElement);
            priceContainerElement.Add(priceLabel);
            Add(priceContainerElement);
        }

        #endregion
    }
}
