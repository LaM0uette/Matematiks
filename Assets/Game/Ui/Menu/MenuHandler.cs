using Game.Modules.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Ui.Menu
{
    public class MenuHandler : MonoBehaviour
    {
        #region Statements
        
        private const string _midleScoreKey = "MiddleScoreValue";
        private const string _midleScoreTitleKey = "MiddleScoreTitle";
        private const string _resumeButtonKey = "ResumeGameButton";
        private const string _newGameButtonKey = "NewGameGameButton";

        private UIDocument _uiDocument;
        private VisualElement _root;
        
        private HeaderScores.HeaderScores _headerScores;
        
        private Label _midleScoreLabel;
        private Label _midleScoreTitleLabel;
        
        private Button _resumeButton;
        private Button _newGameButton;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;

            SetupHeaderScores();
            SetupMiddleScore();
            SetupGameButtons();
        }

        private void Start()
        {
            UpdateHeaderScores();
            InitMiddleScore();
            UpdateGameButtons();
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
        }
        
        private void SetupMiddleScore()
        {
            _midleScoreLabel = _root.Q<Label>(_midleScoreKey);
            _midleScoreTitleLabel = _root.Q<Label>(_midleScoreTitleKey);
        }
        private void InitMiddleScore()
        {
            var currentScore = Saver.CurrentScore.LoadInt();
            _midleScoreTitleLabel.text = currentScore > 0 ? "CURRENT SCORE" : "LAST SCORE";
            
            var score = currentScore > 0 ? currentScore : Saver.LastScore.LoadInt();
            _midleScoreLabel.text = score.ToString();
        }
        
        private void SetupGameButtons()
        {
            _resumeButton = _root.Q<Button>(_resumeButtonKey);
            _newGameButton = _root.Q<Button>(_newGameButtonKey);
        }
        private void UpdateGameButtons()
        {
            SetResumeButtonState();
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            _resumeButton.clicked += OnResumeButtonCliked;
            _newGameButton.clicked += OnNewGameButtonCliked;
        }
        
        private void OnDisable()
        {
            _resumeButton.clicked -= OnResumeButtonCliked;
            _newGameButton.clicked -= OnNewGameButtonCliked;
        }

        #endregion
        
        #region Button Events
        
        private static void OnResumeButtonCliked()
        {
            LoadGameScene();
        }
        
        private static void OnNewGameButtonCliked()
        {
            Saver.ResetAllCurrentScores();
            LoadGameScene();
        }
        
        #endregion

        #region Functions

        private void SetResumeButtonState()
        {
            var currentScore = Saver.CurrentScore.LoadInt();
            _resumeButton.SetEnabled(currentScore > 0);
        }
        
        private static void LoadGameScene()
        {
            SceneManager.LoadScene(GameVar.GameScene);
        }

        #endregion
    }
}
