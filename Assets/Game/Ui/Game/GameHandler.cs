using Game.Modules.Utils;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.Game
{
    public class GameHandler : MonoBehaviour
    {
        #region Statements
        
        private const string _currentScoreKey = "CurrentScoreValue";

        [SerializeField] private ScriptableEventInt _gemEvent;
        [SerializeField] private ScriptableEventInt _currentScoreEvent;
        
        private UIDocument _uiDocument;
        private VisualElement _root;
        
        private HeaderScores.HeaderScores _headerScores;
        
        private Label _currentScoreLabel;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
            
            SetupHeaderScores();
            SetupCurrentScore();
        }
        
        private void Start()
        {
            UpdateHeaderScores();
            UpdateCurrentScore(Saver.CurrentScore.LoadInt());
        }

        #endregion
        
        #region Setup/Update
        
        private void SetupHeaderScores()
        {
            _headerScores = _root.Q<HeaderScores.HeaderScores>();
        }
        private void UpdateHeaderScores()
        {
            _headerScores.UpdateHeaderScore();
            _headerScores.UpdateHighBall(Saver.HighBall.LoadInt());
        }
        
        private void SetupCurrentScore()
        {
            _currentScoreLabel = _root.Q<Label>(_currentScoreKey);
        }
        private void UpdateCurrentScore(int value)
        {
            _currentScoreLabel.text = value.ToString();
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            _gemEvent.OnRaised += OnGemRaised;
            _currentScoreEvent.OnRaised += OnCurrentScoreRaised;
        }
        
        private void OnDisable()
        {
            _gemEvent.OnRaised -= OnGemRaised;
            _currentScoreEvent.OnRaised -= OnCurrentScoreRaised;
        }

        #endregion
        
        #region Button Events
        
        
        #endregion

        #region Functions
        
        private void OnGemRaised(int value)
        {
            _headerScores.UpdateGem(value);
        }
        
        private void OnCurrentScoreRaised(int value)
        {
            _currentScoreLabel.text = value.ToString();
            
            var highScore = Saver.HighScore.LoadInt();
            if (value <= highScore) 
                return;
            
            Saver.HighScore.Save(value);
            _headerScores.UpdateHighScore(value);
        }

        #endregion
    }
}
