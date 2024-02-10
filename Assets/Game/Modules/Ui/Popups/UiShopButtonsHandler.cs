using Game.Modules.Ads;
using Game.Modules.Utils;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Modules.Ui.Popups
{
    public class UiShopButtonsHandler : MonoBehaviour
    {
        #region Statements
        
        [SerializeField] private ScriptableEventInt _shopGemEvent;
        
        private RewardedAdsGem _rewardedAdsGem;
        
        private UIDocument _uiDocument;
        private VisualElement _rootElement;
        private VisualElement _veContainer;
        
        private Button _closeButton;
        private Button _buyGemButtonAds;
        private Button _buyGemButton0;
        private Button _buyGemButton1;
        private Button _buyGemButton2;
        private Button _buyGemButton3;
        private Button _buyGemButton4;
        private Button _buyGemButton5;
        private Button _buyGemButton6;
        
        private Label _gemLabel;

        private void Awake()
        {
            _rewardedAdsGem = GetComponent<RewardedAdsGem>();
            _uiDocument = GetComponent<UIDocument>();
            _rootElement = _uiDocument.rootVisualElement;
            
            SetElements();
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            _shopGemEvent.OnRaised += OnShopGem;
            
            _closeButton.clicked += OnClose;
            _buyGemButtonAds.clicked += OnAdsGem;
            _buyGemButton0.clicked += () => OnBuyGem(0);
            _buyGemButton1.clicked += () => OnBuyGem(1);
            _buyGemButton2.clicked += () => OnBuyGem(2);
            _buyGemButton3.clicked += () => OnBuyGem(3);
            _buyGemButton4.clicked += () => OnBuyGem(4);
            _buyGemButton5.clicked += () => OnBuyGem(5);
            _buyGemButton6.clicked += () => OnBuyGem(6);
        }
        
        private void OnDisable()
        {
            _shopGemEvent.OnRaised -= OnShopGem;
            
            _closeButton.clicked -= OnClose;
            _buyGemButtonAds.clicked -= OnAdsGem;
        }

        #endregion
        
        #region Functions
        
        public void Show()
        {
            var score = Saver.Gem.LoadInt();
            
            SetGem(score);
            _veContainer.style.display = DisplayStyle.Flex;
        }
        
        public void Hide()
        {
            _veContainer.style.display = DisplayStyle.None;
        }
        
        public void SetGem(int score)
        {
            _gemLabel.text = score.ToString();
        }

        private void SetElements()
        {
            _veContainer = _rootElement.Q<VisualElement>("ve_container");
            
            _closeButton = _rootElement.Q<Button>("button_loose-close");
            _buyGemButtonAds = _rootElement.Q<Button>("button_shop_ads");
            _buyGemButton0 = _rootElement.Q<Button>("button_shop_0");
            _buyGemButton1 = _rootElement.Q<Button>("button_shop_1");
            _buyGemButton2 = _rootElement.Q<Button>("button_shop_2");
            _buyGemButton3 = _rootElement.Q<Button>("button_shop_3");
            _buyGemButton4 = _rootElement.Q<Button>("button_shop_4");
            _buyGemButton5 = _rootElement.Q<Button>("button_shop_5");
            _buyGemButton6 = _rootElement.Q<Button>("button_shop_6");
            
            _gemLabel = _rootElement.Q<Label>("label_shop-gem");
        }
        
        private void OnShopGem(int _)
        {
            var score = Saver.Gem.LoadInt();
            SetGem(score);
        }

        private void OnClose()
        {
            Hide();
        }
        
        private void OnAdsGem()
        {
            _rewardedAdsGem.ShowAd();
            //Hide();
        }

        private void OnBuyGem(int value)
        {
            Debug.Log("Buy gem: " + value);
            //Hide();
        }
        #endregion
    }
}
