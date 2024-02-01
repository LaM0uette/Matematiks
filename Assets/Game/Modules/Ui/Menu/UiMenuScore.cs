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
            _lastscoreLabel.text = Saver.LastScore.LoadInt().ToString();
        }

        #endregion

        #region Functions

        private void SetElements()
        {
            _gemLabel = _rootElement.Q<Label>("label_nbrgems");
            _highscoreLabel = _rootElement.Q<Label>("label_highscore");
            _lastscoreLabel = _rootElement.Q<Label>("label_lastscore");
        }

        #endregion
    }
}
