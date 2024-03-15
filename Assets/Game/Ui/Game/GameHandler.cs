using Game.Modules.Utils;
using Game.Ui.Components.BonusCard;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.Game
{
    public class GameHandler : MonoBehaviour
    {
        #region Statements
        
        // TODO: a supprimer probablement
        [SerializeField] private ScriptableEventInt _bonusEvent;
        
        private const string _currentScoreKey = "CurrentScoreValue";
        private const string _bonusCard01Key = "BonusCard01";
        private const string _bonusCard02Key = "BonusCard02";
        private const string _bonusCard03Key = "BonusCard03";
        private const string _bonusCard04Key = "BonusCard04";
        private const string _pauseButtonKey = "PauseButton";

        [SerializeField] private ScriptableEventInt _gemEvent;
        [SerializeField] private ScriptableEventInt _currentScoreEvent;
        
        private UIDocument _uiDocument;
        private VisualElement _root;
        
        private HeaderScores.HeaderScores _headerScores;
        
        private Label _currentScoreLabel;
        
        private BonusCard _bonusCard01;
        private BonusCard _bonusCard02;
        private BonusCard _bonusCard03;
        private BonusCard _bonusCard04;
        
        private Button _pauseButton;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
            
            SetupHeaderScores();
            SetupCurrentScore();
            SetupButtons();
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
        
        private void SetupButtons()
        {
            _bonusCard01 = _root.Q<BonusCard>(_bonusCard01Key);
            _bonusCard02 = _root.Q<BonusCard>(_bonusCard02Key);
            _bonusCard03 = _root.Q<BonusCard>(_bonusCard03Key);
            _bonusCard04 = _root.Q<BonusCard>(_bonusCard04Key);
            
            _pauseButton = _root.Q<Button>(_pauseButtonKey);
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            UiEvents.LooseEvent += OnLooseEvent;
            UiEvents.RefreshUiEvent += OnUpdateBoardEvent;
            
            _gemEvent.OnRaised += OnGemRaised;
            _currentScoreEvent.OnRaised += OnCurrentScoreRaised;
            
            _bonusCard01.clicked += () => OnBonusCardClicked(_bonusCard01, 1);
            _bonusCard02.clicked += () => OnBonusCardClicked(_bonusCard02, 2);
            _bonusCard03.clicked += () => OnBonusCardClicked(_bonusCard03, 3);
            _bonusCard04.clicked += () => OnBonusCardClicked(_bonusCard04, 4);
            
            _pauseButton.clicked += OnPauseButtonClicked;
        }
        
        private void OnDisable()
        {
            UiEvents.LooseEvent -= OnLooseEvent;
            UiEvents.RefreshUiEvent -= OnUpdateBoardEvent;
            
            _gemEvent.OnRaised -= OnGemRaised;
            _currentScoreEvent.OnRaised -= OnCurrentScoreRaised;
            
            _pauseButton.clicked -= OnPauseButtonClicked;
        }

        #endregion
        
        #region Button Events
        
        private void OnBonusCardClicked(BonusCard bonusCard, int bonusId)
        {
            HideBonusCards();
            
            bonusCard.Show();
            bonusCard.Select();
            
            _bonusEvent.Raise(bonusId);
        }
        
        private static void OnPauseButtonClicked()
        {
            UiEvents.PausePopupShow.Invoke();
        }
        
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
        
        private void OnUpdateBoardEvent()
        {
            ShowBonusCards();
        }
        
        private void OnLooseEvent()
        {
            HideBonusCards();
            _pauseButton.style.visibility = Visibility.Hidden;
        }
        
        private void ShowBonusCards()
        {
            _bonusCard01.Unselect();
            _bonusCard01.Show();
            _bonusCard02.Unselect();
            _bonusCard02.Show();
            _bonusCard03.Unselect();
            _bonusCard03.Show();
            _bonusCard04.Unselect();
            _bonusCard04.Show();
        }
        
        private void HideBonusCards()
        {
            _bonusCard01.Unselect();
            _bonusCard01.Hide();
            _bonusCard02.Unselect();
            _bonusCard02.Hide();
            _bonusCard03.Unselect();
            _bonusCard03.Hide();
            _bonusCard04.Unselect();
            _bonusCard04.Hide();
        }

        #endregion
    }
}
