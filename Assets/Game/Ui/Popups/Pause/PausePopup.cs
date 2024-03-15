using Game.Modules.Utils;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Ui.Popups.Pause
{
    public class PausePopup : Popup
    {
        #region Statements
        
        private const string _menuButtonKey = "PauseMenuButton";
        private const string _resumeButtonKey = "PauseResumeButton";
        private const string _restartButtonKey = "PauseRestartButton";
        
        private Button _menuButton;
        private Button _resumeButton;
        private Button _restartButton;

        public PausePopup(VisualElement topElement) : base(topElement)
        {
        }

        #endregion

        #region Functions
        
        protected override void SetVisualElements()
        {
            _menuButton = TopElement.Q<Button>(_menuButtonKey);
            _resumeButton = TopElement.Q<Button>(_resumeButtonKey);
            _restartButton = TopElement.Q<Button>(_restartButtonKey);
        }

        protected override void RegisterButtonCallbacks()
        {
            _menuButton.clicked += OnMenuButtonClicked;
            _resumeButton.clicked += OnResumeButtonClicked;
            _restartButton.clicked += OnRestartButtonClicked;
        }
        
        protected override void UnregisterButtonCallbacks()
        {
            _menuButton.clicked -= OnMenuButtonClicked;
            _resumeButton.clicked -= OnResumeButtonClicked;
            _restartButton.clicked -= OnRestartButtonClicked;
        }
        
        private static void OnMenuButtonClicked()
        {
            SceneManager.LoadScene(GameVar.MenuScene);
        }
        
        private void OnResumeButtonClicked()
        {
            Hide();
            UiEvents.ClosePopup.Invoke();
        }
        
        private static void OnRestartButtonClicked()
        {
            Saver.ResetAllCurrentScores();
            SceneManager.LoadScene(GameVar.GameScene);
        }

        #endregion
    }
}