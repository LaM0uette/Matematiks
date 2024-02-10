using Game.Modules.Utils;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Modules.Ui.Menu
{
    public class UiMenuScoreHandler : MonoBehaviour
    {
        #region Statements
        
        [SerializeField] private ScriptableEventInt _shopGemEvent;

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
        
        #region Events

        private void OnEnable()
        {
            _shopGemEvent.OnRaised += OnShopGem;
        }
        
        private void OnDisable()
        {
            _shopGemEvent.OnRaised -= OnShopGem;
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
        
        private void OnShopGem(int _)
        {
            var score = Saver.Gem.LoadInt();
            _gemLabel.text = score.ToString();
        }

        #endregion
    }
}
