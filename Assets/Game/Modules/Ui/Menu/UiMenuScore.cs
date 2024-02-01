using Game.Modules.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Modules.Ui.Menu
{
    public class UiMenuScore : MonoBehaviour
    {
        #region Statements

        private UIDocument _uiDocument;
        private VisualElement _rootElement;
        
        private Label _gemLabel;
        private Label _highscoreLabel;
        private Label _lastscoreLabel;
        private Label _lastscoreTitleLabel;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _rootElement = _uiDocument.rootVisualElement;
            
            SetElements();
        }
        
        private void Start()
        {
            _gemLabel.text = Saver.Gem.LoadInt().ToString();
            _highscoreLabel.text = Saver.HighScore.LoadInt().ToString();
            
            var currentScore = Saver.CurrentScore.LoadInt();
            _lastscoreTitleLabel.text = currentScore > 0 ? "Current score" : "Last score";
            
            var score = currentScore > 0 ? currentScore : Saver.LastScore.LoadInt();
            _lastscoreLabel.text = score.ToString();
        }

        #endregion

        #region Functions

        private void SetElements()
        {
            _gemLabel = _rootElement.Q<Label>("label_nbrgems");
            _highscoreLabel = _rootElement.Q<Label>("label_highscore");
            _lastscoreLabel = _rootElement.Q<Label>("label_lastscore");
            _lastscoreTitleLabel = _rootElement.Q<Label>("label_lastscore_title");
        }

        #endregion
    }
}
