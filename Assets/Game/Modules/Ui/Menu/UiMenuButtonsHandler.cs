using System;
using Game.Modules.Ui.Popups;
using Game.Modules.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Modules.Ui.Menu
{
    public class UiMenuButtonsHandler : MonoBehaviour
    {
        #region Statements

        [Space, Title("Ui")]
        [SerializeField] private UiShopButtonsHandler _uiShopButtonsHandler;
        private UIDocument _uiDocument;
        private VisualElement _rootElement;
        
        private Button _continueButton;
        private Button _newGameButton;
        private Button _settingsButton;
        
        private Button _shopButtonIcon;
        private Button _profilButtonIcon;
        private Button _rankButtonIcon;

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
            _continueButton.clicked += OnContinueButton;
            _newGameButton.clicked += OnNewGameButton;
            _settingsButton.clicked += OnSettingsButton;
            
            _shopButtonIcon.clicked += OnShopButton;
            _profilButtonIcon.clicked += OnProfilButton;
            _rankButtonIcon.clicked += OnRankButton;
            
            _settingsButton.SetEnabled(false);
            _profilButtonIcon.SetEnabled(false);
            _rankButtonIcon.SetEnabled(false);
        }
        
        private void OnDisable()
        {
            _continueButton.clicked -= OnContinueButton;
            _newGameButton.clicked -= OnNewGameButton;
            _settingsButton.clicked -= OnSettingsButton;
            
            _shopButtonIcon.clicked -= OnShopButton;
            _profilButtonIcon.clicked -= OnProfilButton;
            _rankButtonIcon.clicked -= OnRankButton;
        }

        #endregion

        #region Button Events

        private static void OnContinueButton()
        {
            SceneManager.LoadScene(GameVar.GameScene);
        }
        
        private static void OnNewGameButton()
        {
            Saver.CurrentScore.Delete();
            Saver.CurrentBalls.Delete();
            Saver.CurrentWeightedBalls.Delete();
            SceneManager.LoadScene(GameVar.GameScene);
        }
        
        private static void OnSettingsButton()
        {
            Debug.Log("Settings");
        }
        
        private void OnShopButton()
        {
            _uiShopButtonsHandler.Show();
        }
        
        private static void OnProfilButton()
        {
            Debug.Log("Profil");
        }
        
        private static void OnRankButton()
        {
            Debug.Log("Rank");
        }

        #endregion

        #region Functions

        private void SetElements()
        {
            _continueButton = _rootElement.Q<Button>("button_continue");
            _newGameButton = _rootElement.Q<Button>("button_newgame");
            _settingsButton = _rootElement.Q<Button>("button_settings");
            
            _shopButtonIcon = _rootElement.Q<Button>("button_shop");
            _profilButtonIcon = _rootElement.Q<Button>("button_profil");
            _rankButtonIcon = _rootElement.Q<Button>("button_rank");
        }

        #endregion
    }
}
