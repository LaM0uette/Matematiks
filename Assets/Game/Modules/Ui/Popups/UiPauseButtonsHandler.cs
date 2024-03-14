using Game.Modules.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Modules.Ui.Popups
{
    public class UiPauseButtonsHandler : MonoBehaviour
    {
        #region Statements
        
        private UIDocument _uiDocument;
        private VisualElement _rootElement;
        private VisualElement _veContainer;
        
        private Button _closeButton;
        private Button _homeButton;
        private Button _resumeButton;
        private Button _replayButton;
        private Button _settingsButton;

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
            _closeButton.clicked += OnClose;
            _homeButton.clicked += OnHome;
            _resumeButton.clicked += OnResume;
            _replayButton.clicked += OnReplay;
            _settingsButton.clicked += OnSettings;
            
            _settingsButton.SetEnabled(false);
        }
        
        private void OnDisable()
        {
            _closeButton.clicked -= OnClose;
            _homeButton.clicked -= OnHome;
            _resumeButton.clicked -= OnResume;
            _replayButton.clicked -= OnReplay;
            _settingsButton.clicked -= OnSettings;
        }

        #endregion
        
        #region Functions

        private void SetElements()
        {
            _veContainer = _rootElement.Q<VisualElement>("ve_container");
            
            _closeButton = _rootElement.Q<Button>("button_pause-close");
            _homeButton = _rootElement.Q<Button>("button_pause-home");
            _resumeButton = _rootElement.Q<Button>("button_pause-resume");
            _replayButton = _rootElement.Q<Button>("button_pause-replay");
            _settingsButton = _rootElement.Q<Button>("button_pause-settings");
        }

        private void OnClose()
        {
            _veContainer.style.display = DisplayStyle.None;
        }
        
        private void OnHome()
        {
            OnClose();
            SceneManager.LoadScene(GameVar.MenuScene);
        }
        
        private void OnResume()
        {
            OnClose();
        }
        
        private void OnReplay()
        {
            OnClose();
            
            Saver.CurrentScore.Delete();
            Saver.CurrentBalls.Delete();
            Saver.CurrentWeightedBalls.Delete();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        private void OnSettings()
        {
            Debug.Log("Settings button clicked");
        }
        
        #endregion
    }
}
