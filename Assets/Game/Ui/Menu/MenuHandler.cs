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
        private const string _continueButtonKey = "ContinueGameButton";
        private const string _newGameButtonKey = "NewGameGameButton";

        private UIDocument _uiDocument;
        private VisualElement _root;
        
        private HeaderScores.HeaderScores _headerScores;
        
        private Label _midleScoreLabel;
        private Label _midleScoreTitleLabel;
        
        private Button _continueButton;
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
            InitHeaderScores();
            InitMiddleScore();
            InitGameButtons();
        }

        #endregion

        #region Setup/Init

        private void SetupHeaderScores()
        {
            _headerScores = _root.Q<HeaderScores.HeaderScores>();
        }
        private void InitHeaderScores()
        {
            _headerScores.UpdateHighScore();
            _headerScores.UpdateHighBall(Saver.HighBall.LoadInt());
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
            _continueButton = _root.Q<Button>(_continueButtonKey);
            _newGameButton = _root.Q<Button>(_newGameButtonKey);
        }
        private void InitGameButtons()
        {
            SetContinueButtonState();
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            _continueButton.clicked += OnContinueButtonCliked;
            _newGameButton.clicked += OnNewGameButtonCliked;
        }
        
        private void OnDisable()
        {
            _continueButton.clicked -= OnContinueButtonCliked;
            _newGameButton.clicked -= OnNewGameButtonCliked;
        }

        #endregion
        
        #region Button Events
        
        private static void OnContinueButtonCliked()
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

        private void SetContinueButtonState()
        {
            var currentScore = Saver.CurrentScore.LoadInt();
            _continueButton.SetEnabled(currentScore > 0);
        }
        
        private static void LoadGameScene()
        {
            SceneManager.LoadScene(GameVar.GameScene);
        }

        #endregion
    }
}
