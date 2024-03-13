using Game.Modules.Utils;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.Game
{
    public class GameHandler : MonoBehaviour
    {
        #region Statements
        
        // TODO: a supprimer probablement
        [SerializeField] private UIDocument _pausePanel;
        [SerializeField] private ScriptableEventInt _bonusEvent;
        [SerializeField] private ScriptableEventNoParam _looseEvent;
        [SerializeField] private ScriptableEventNoParam _updateBoardEvent;
        
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
        
        private Button _bonusCard01;
        private Button _bonusCard02;
        private Button _bonusCard03;
        private Button _bonusCard04;
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
            InitButtons();
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
            _bonusCard01 = _root.Q<Button>(_bonusCard01Key);
            _bonusCard02 = _root.Q<Button>(_bonusCard02Key);
            _bonusCard03 = _root.Q<Button>(_bonusCard03Key);
            _bonusCard04 = _root.Q<Button>(_bonusCard04Key);
            _pauseButton = _root.Q<Button>(_pauseButtonKey);
        }
        private void InitButtons()
        {
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            _gemEvent.OnRaised += OnGemRaised;
            _currentScoreEvent.OnRaised += OnCurrentScoreRaised;
            _updateBoardEvent.OnRaised += OnUpdateBoardEvent;
            _looseEvent.OnRaised += OnLooseEvent;
            
            _bonusCard01.clicked += OnBonusCard01Clicked;
            _bonusCard02.clicked += OnBonusCard02Clicked;
            _bonusCard03.clicked += OnBonusCard03Clicked;
            _bonusCard04.clicked += OnBonusCard04Clicked;
            _pauseButton.clicked += OnPauseButtonClicked;
        }
        
        private void OnDisable()
        {
            _gemEvent.OnRaised -= OnGemRaised;
            _currentScoreEvent.OnRaised -= OnCurrentScoreRaised;
            _updateBoardEvent.OnRaised -= OnUpdateBoardEvent;
            _looseEvent.OnRaised -= OnLooseEvent;
            
            _bonusCard01.clicked -= OnBonusCard01Clicked;
            _bonusCard02.clicked -= OnBonusCard02Clicked;
            _bonusCard03.clicked -= OnBonusCard03Clicked;
            _bonusCard04.clicked -= OnBonusCard04Clicked;
            _pauseButton.clicked -= OnPauseButtonClicked;
        }

        #endregion
        
        #region Button Events
        
        private void OnBonusCard01Clicked()
        {
            HideBonusCards();
            //_veBonus1.style.display = DisplayStyle.Flex;
            _bonusEvent.Raise(1);
        }
        
        private void OnBonusCard02Clicked()
        {
            HideBonusCards();
            //_veBonus1.style.display = DisplayStyle.Flex;
            _bonusEvent.Raise(2);
        }
        
        private void OnBonusCard03Clicked()
        {
            HideBonusCards();
            //_veBonus1.style.display = DisplayStyle.Flex;
            _bonusEvent.Raise(3);
        }
        
        private void OnBonusCard04Clicked()
        {
            HideBonusCards();
            //_veBonus1.style.display = DisplayStyle.Flex;
            _bonusEvent.Raise(4);
        }
        
        private void OnPauseButtonClicked()
        {
            _pausePanel.rootVisualElement.Q<VisualElement>("ve_container").style.display = DisplayStyle.Flex;
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
        
        private void ShowBonusCards()
        {
            _bonusCard01.style.display = DisplayStyle.Flex;
            _bonusCard02.style.display = DisplayStyle.Flex;
            _bonusCard03.style.display = DisplayStyle.Flex;
            _bonusCard04.style.display = DisplayStyle.Flex;
        }
        
        private void HideBonusCards()
        {
            _bonusCard01.style.display = DisplayStyle.None;
            _bonusCard02.style.display = DisplayStyle.None;
            _bonusCard03.style.display = DisplayStyle.None;
            _bonusCard04.style.display = DisplayStyle.None;
        }

        #endregion
        
        // TODO: a supprimer probablement
        
        private void OnUpdateBoardEvent()
        {
            ShowBonusCards();
            //_veBonus1.style.display = DisplayStyle.None;
            //_veBonus2.style.display = DisplayStyle.None;
            //_veBonus3.style.display = DisplayStyle.None;
            //_veBonus4.style.display = DisplayStyle.None;
        }
        
        private void OnLooseEvent()
        {
            //_veBonus.style.display = DisplayStyle.None;
            //_veLoose.style.display = DisplayStyle.Flex;
            
            _pauseButton.style.visibility = Visibility.Hidden;
        }
    }
}
