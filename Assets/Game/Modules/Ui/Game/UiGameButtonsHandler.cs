using Game.Modules.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Modules.Ui.Game
{
    public class UiGameButtonsHandler : MonoBehaviour
    {
        #region Statements

        private UIDocument _uiDocument;
        private VisualElement _rootElement;
        
        private Button _shopButton;
        private Button _pauseButton;
        private Button _bonusButton1;
        private Button _bonusButton2;
        private Button _bonusButton3;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _rootElement = _uiDocument.rootVisualElement;
            
            SetElements();
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            _shopButton.clicked += OnShopButton;
            _pauseButton.clicked += OnPauseButton;
            _bonusButton1.clicked += OnBonus1Button;
            _bonusButton2.clicked += OnBonus2Button;
            _bonusButton3.clicked += OnBonus3Button;
        }
        
        private void OnDisable()
        {
            _shopButton.clicked -= OnShopButton;
            _pauseButton.clicked -= OnPauseButton;
            _bonusButton1.clicked -= OnBonus1Button;
            _bonusButton2.clicked -= OnBonus2Button;
            _bonusButton3.clicked -= OnBonus3Button;
        }

        #endregion
        
        #region Functions

        private void SetElements()
        {
            _shopButton = _rootElement.Q<Button>("button_shop");
            _pauseButton = _rootElement.Q<Button>("button_pause");
            _bonusButton1 = _rootElement.Q<Button>("button_bonus1");
            _bonusButton2 = _rootElement.Q<Button>("button_bonus2");
            _bonusButton3 = _rootElement.Q<Button>("button_bonus3");
        }
        
        private void OnShopButton()
        {
            Debug.Log("Shop button clicked");
        }
        
        private void OnPauseButton()
        {
            SceneManager.LoadScene(GameVar.MenuScene);
        }
        
        private void OnBonus1Button()
        {
            Debug.Log("Bonus 1 button clicked");
        }
        
        private void OnBonus2Button()
        {
            Debug.Log("Bonus 2 button clicked");
        }
        
        private void OnBonus3Button()
        {
            Debug.Log("Bonus 3 button clicked");
        }
        
        #endregion
    }
}
