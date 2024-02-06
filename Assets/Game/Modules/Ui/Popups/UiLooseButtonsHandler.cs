using Game.Modules.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Modules.Ui.Popups
{
    public class UiLooseButtonsHandler : MonoBehaviour
    {
        #region Statements
        
        private UIDocument _uiDocument;
        private VisualElement _rootElement;
        private VisualElement _veContainer;
        
        private Button _closeButton;
        private Button _homeButton;
        private Button _replayButton;
        
        private Label _scoreLabel;

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
            _replayButton.clicked += OnReplay;
        }
        
        private void OnDisable()
        {
            _closeButton.clicked -= OnClose;
            _homeButton.clicked -= OnHome;
            _replayButton.clicked -= OnReplay;
        }

        #endregion
        
        #region Functions
        
        public void Show()
        {
            var score = Saver.LastScore.LoadInt();
            
            SetScore(score);
            _veContainer.style.display = DisplayStyle.Flex;
        }
        
        public void Hide()
        {
            _veContainer.style.display = DisplayStyle.None;
        }
        
        public void SetScore(int score)
        {
            _scoreLabel.text = score.ToString();
        }

        private void SetElements()
        {
            _veContainer = _rootElement.Q<VisualElement>("ve_container");
            
            _closeButton = _rootElement.Q<Button>("button_loose-close");
            _homeButton = _rootElement.Q<Button>("button_loose-home");
            _replayButton = _rootElement.Q<Button>("button_loose-replay");
            
            _scoreLabel = _rootElement.Q<Label>("label_loose-score");
        }

        private void OnClose()
        {
            Hide();
        }
        
        private void OnHome()
        {
            Hide();
            SceneManager.LoadScene(GameVar.MenuScene);
        }
        
        private void OnReplay()
        {
            Hide();
            
            Saver.CurrentScore.Delete();
            Saver.CurrentBalls.Delete();
            Saver.CurrentWeightedBalls.Delete();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        #endregion
    }
}
