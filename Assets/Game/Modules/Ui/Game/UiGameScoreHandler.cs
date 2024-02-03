using Game.Modules.Utils;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Modules.Ui.Game
{
    public class UiGameScoreHandler : MonoBehaviour
    {
        #region Statements
        
        [SerializeField] private ScriptableEventInt _gemEvent;
        [SerializeField] private ScriptableEventInt _scoreEvent;

        private UIDocument _uiDocument;
        private VisualElement _rootElement;
        
        private Label _gemLabel;
        private Label _highscoreLabel;
        private Label _currentscoreLabel;

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
            _currentscoreLabel.text = Saver.CurrentScore.LoadInt().ToString();
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            _gemEvent.OnRaised += OnGemRaised;
            _scoreEvent.OnRaised += OnScoreRaised;
        }
        
        private void OnDisable()
        {
            _gemEvent.OnRaised -= OnGemRaised;
            _scoreEvent.OnRaised -= OnScoreRaised;
        }

        #endregion

        #region Functions

        private void SetElements()
        {
            _gemLabel = _rootElement.Q<Label>("label_nbrgems");
            _highscoreLabel = _rootElement.Q<Label>("label_highscore");
            _currentscoreLabel = _rootElement.Q<Label>("label_currentscore");
        }
        
        private void OnGemRaised(int value)
        {
            _gemLabel.text = value.ToString();
        }
        
        private void OnScoreRaised(int value)
        {
            _currentscoreLabel.text = value.ToString();
            
            var bestScore = Saver.HighScore.LoadInt();

            if (value <= bestScore) return;
            
            Saver.HighScore.Save(value);
            _highscoreLabel.text = value.ToString();
        }

        #endregion
    }
}
