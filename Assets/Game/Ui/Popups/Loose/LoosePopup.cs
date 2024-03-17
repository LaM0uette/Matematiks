using Game.Modules.Utils;
using Game.Ui.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Ui.Popups.Loose
{
    public class LoosePopup : Popup
    {
        #region Statements
        
        private const string _closeButtonKey = "LooseCloseButton";
        private const string _looseScoreKey = "LooseScoreValue";
        private const string _menuButtonKey = "LooseMenuButton";
        private const string _restartButtonKey = "LooseRestartButton";
        
        private Label _looseScore;
        
        private Button _closeButton;
        private Button _menuButton;
        private Button _restartButton;

        public LoosePopup(VisualElement topElement) : base(topElement)
        {
        }

        #endregion

        #region Functions
        
        protected override void SetVisualElements()
        {
            _looseScore = TopElement.Q<Label>(_looseScoreKey);
            
            _closeButton = TopElement.Q<Button>(_closeButtonKey);
            _menuButton = TopElement.Q<Button>(_menuButtonKey);
            _restartButton = TopElement.Q<Button>(_restartButtonKey);
        }

        protected override void RegisterButtonCallbacks()
        {
            _closeButton.clicked += OnCloseButtonClicked;
            _menuButton.clicked += OnMenuButtonClicked;
            _restartButton.clicked += OnRestartButtonClicked;
        }
        
        protected override void UnregisterButtonCallbacks()
        {
            _closeButton.clicked -= OnCloseButtonClicked;
            _menuButton.clicked -= OnMenuButtonClicked;
            _restartButton.clicked -= OnRestartButtonClicked;
        }
        
        private void OnCloseButtonClicked()
        {
            Hide();
            UiEvents.ClosePopupEvent.Invoke();
        }

        public override void Show()
        {
            base.Show();
            _looseScore.text = OldSaver.LastScore.LoadInt().ToString();
        }
        
        private static void OnMenuButtonClicked()
        {
            SceneManager.LoadScene(GameVar.MenuScene);
        }
        
        private static void OnRestartButtonClicked()
        {
            OldSaver.ResetAllCurrentScores();
            SceneManager.LoadScene(GameVar.GameScene);
        }

        #endregion
    }
}
