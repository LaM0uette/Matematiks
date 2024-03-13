using Game.Modules.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.Menu
{
    public class MenuHandler : MonoBehaviour
    {
        #region Statements
        
        private const string _midleScoreKey = "MiddleScoreValue";
        private const string _midleScoreTitleKey = "MiddleScoreTitle";

        private UIDocument _uiDocument;
        private VisualElement _root;
        
        private HeaderScores.HeaderScores _headerScores;
        
        private Label _midleScoreLabel;
        private Label _midleScoreTitleLabel;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;

            SetupHeaderScores();
            SetupMiddleScore();
        }

        private void Start()
        {
            InitHeaderScores();
            InitMiddleScore();
        }

        #endregion

        #region Functions

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

        #endregion
    }
}
