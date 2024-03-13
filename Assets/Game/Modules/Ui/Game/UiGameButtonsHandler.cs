using Game.Modules.Utils;
using Obvious.Soap;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Modules.Ui.Game
{
    public class UiGameButtonsHandler : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Ui")]
        [SerializeField] private UIDocument _pausePanel;
        private VisualElement _pauseRootElement;
        private VisualElement _veContainer;
        
        [Space, Title("Soap")]
        [SerializeField] private ScriptableEventInt _bonusEvent;
        [SerializeField] private ScriptableEventNoParam _looseEvent;
        [SerializeField] private ScriptableEventNoParam _updateBoardEvent;

        private UIDocument _uiDocument;
        private VisualElement _rootElement;
        
        private VisualElement _veBonus;
        private VisualElement _veLoose;
        
        private Button _pauseButton;
        
        private Button _bonusButton1;
        private Button _bonusButton2;
        private Button _bonusButton3;
        private Button _bonusButton4;
        private VisualElement _veBonus1;
        private VisualElement _veBonus2;
        private VisualElement _veBonus3;
        private VisualElement _veBonus4;
        
        private Button _looseHomeButton;
        private Button _looseReplayButton;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _rootElement = _uiDocument.rootVisualElement;
            _pauseRootElement = _pausePanel.rootVisualElement;
            
            SetElements();
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            _updateBoardEvent.OnRaised += OnUpdateBoardEvent;
            _looseEvent.OnRaised += OnLooseEvent;
            
            _pauseButton.clicked += OnPauseButton;
            _bonusButton1.clicked += OnBonus1Button;
            _bonusButton2.clicked += OnBonus2Button;
            _bonusButton3.clicked += OnBonus3Button;
            _bonusButton4.clicked += OnBonus4Button;
            
            _looseHomeButton.clicked += OnLooseHomeButton;
            _looseReplayButton.clicked += OnLooseReplayButton;
            
            _pauseButton.style.visibility = Visibility.Visible;
            _veBonus.style.display = DisplayStyle.Flex;
            _veLoose.style.display = DisplayStyle.None;
            
            _bonusButton1.style.display = DisplayStyle.Flex;
            _bonusButton2.style.display = DisplayStyle.Flex;
            _bonusButton3.style.display = DisplayStyle.Flex;
            _bonusButton4.style.display = DisplayStyle.Flex;
            _veBonus1.style.display = DisplayStyle.None;
            _veBonus2.style.display = DisplayStyle.None;
            _veBonus3.style.display = DisplayStyle.None;
            _veBonus4.style.display = DisplayStyle.None;
        }
        
        private void OnDisable()
        {
            _updateBoardEvent.OnRaised -= OnUpdateBoardEvent;
            
            _looseEvent.OnRaised -= OnLooseEvent;
            
            _pauseButton.clicked -= OnPauseButton;
            _bonusButton1.clicked -= OnBonus1Button;
            _bonusButton2.clicked -= OnBonus2Button;
            _bonusButton3.clicked -= OnBonus3Button;
            _bonusButton4.clicked += OnBonus4Button;
            
            _looseHomeButton.clicked -= OnLooseHomeButton;
            _looseReplayButton.clicked -= OnLooseReplayButton;
        }

        #endregion
        
        #region Functions

        private void SetElements()
        {
            _veBonus = _rootElement.Q<VisualElement>("ve_bonusicons");
            _veLoose = _rootElement.Q<VisualElement>("ve_loose");
            _veContainer = _pauseRootElement.Q<VisualElement>("ve_container");
            
            _pauseButton = _rootElement.Q<Button>("button_pause");
            
            _bonusButton1 = _rootElement.Q<Button>("button_bonus1");
            _bonusButton2 = _rootElement.Q<Button>("button_bonus2");
            _bonusButton3 = _rootElement.Q<Button>("button_bonus3");
            _bonusButton4 = _rootElement.Q<Button>("button_bonus4");
            _veBonus1 = _rootElement.Q<VisualElement>("ve_bonus1");
            _veBonus2 = _rootElement.Q<VisualElement>("ve_bonus2");
            _veBonus3 = _rootElement.Q<VisualElement>("ve_bonus3");
            _veBonus4 = _rootElement.Q<VisualElement>("ve_bonus4");
            
            _looseHomeButton = _rootElement.Q<Button>("button_loose-home");
            _looseReplayButton = _rootElement.Q<Button>("button_loose-replay");
        }

        private void OnLooseEvent()
        {
            _veBonus.style.display = DisplayStyle.None;
            _veLoose.style.display = DisplayStyle.Flex;
            
            _pauseButton.style.visibility = Visibility.Hidden;
        }
        
        private void OnPauseButton()
        {
            _veContainer.style.display = DisplayStyle.Flex;
        }

        private void OnUpdateBoardEvent()
        {
            _bonusButton1.style.display = DisplayStyle.Flex;
            _bonusButton2.style.display = DisplayStyle.Flex;
            _bonusButton3.style.display = DisplayStyle.Flex;
            _bonusButton4.style.display = DisplayStyle.Flex;
            _veBonus1.style.display = DisplayStyle.None;
            _veBonus2.style.display = DisplayStyle.None;
            _veBonus3.style.display = DisplayStyle.None;
            _veBonus4.style.display = DisplayStyle.None;
        }

        private void HideButtons()
        {
            _bonusButton1.style.display = DisplayStyle.None;
            _bonusButton2.style.display = DisplayStyle.None;
            _bonusButton3.style.display = DisplayStyle.None;
            _bonusButton4.style.display = DisplayStyle.None;
        }
        
        private void OnBonus1Button()
        {
            HideButtons();
            _veBonus1.style.display = DisplayStyle.Flex;
            _bonusEvent.Raise(1);
        }
        
        private void OnBonus2Button()
        {
            HideButtons();
            _veBonus2.style.display = DisplayStyle.Flex;
            _bonusEvent.Raise(2);
        }
        
        private void OnBonus3Button()
        {
            HideButtons();
            _veBonus3.style.display = DisplayStyle.Flex;
            _bonusEvent.Raise(3);
        }
        
        private void OnBonus4Button()
        {
            HideButtons();
            _veBonus4.style.display = DisplayStyle.Flex;
            _bonusEvent.Raise(4);
        }
        
        private void OnLooseHomeButton()
        {
            SceneManager.LoadScene(GameVar.MenuScene);
        }
        
        private void OnLooseReplayButton()
        {
            Saver.CurrentScore.Delete();
            Saver.CurrentBalls.Delete();
            Saver.CurrentWeightedBalls.Delete();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        #endregion
    }
}
