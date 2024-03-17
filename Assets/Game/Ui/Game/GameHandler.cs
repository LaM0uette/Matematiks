using System;
using System.Collections.Generic;
using Game.Modules.Bonus;
using Game.Modules.Events;
using Game.Modules.Utils;
using Game.Ui.Components.BonusCard;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Game.Ui.Game
{
    public class GameHandler : MonoBehaviour
    {
        #region Statements

        private struct BonusCardData
        {
            public BonusCard BonusCard;
            public BonusData BonusData;
        }
        
        private const string _currentScoreGameOverKey = "CurrentScoreGameOver";
        private const string _currentScoreKey = "CurrentScoreValue";
        private const string _bonusCard01Key = "BonusCard01";
        private const string _bonusCard02Key = "BonusCard02";
        private const string _bonusCard03Key = "BonusCard03";
        private const string _bonusCard04Key = "BonusCard04";
        private const string _menuButtonKey = "GameMenuButton";
        private const string _restartButtonKey = "GameRestartButton";
        private const string _pauseButtonKey = "PauseButton";
        
        private UIDocument _uiDocument;
        private VisualElement _root;
        
        private HeaderScores.HeaderScores _headerScores;
        
        private Label _currentScoreGameOver;
        private Label _currentScoreLabel;
        
        private List<BonusCardData> _bonusCardDataList;
        private List<BonusCard> _bonusCardList;
        private BonusCard _bonusCard01;
        private BonusCard _bonusCard02;
        private BonusCard _bonusCard03;
        private BonusCard _bonusCard04;
        
        private Button _menuButton;
        private Button _restartButton;
        private Button _pauseButton;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
            
            SetupButtons();
            InitButtons();
            
            SetupHeaderScores();
            SetupCurrentScore();
            InitCurrentScore();
            
            LoadBonusData();
        }
        
        private void Start()
        {
            InitBonusData();
            UpdateHeaderScores();
            UpdateCurrentScore(Saver.CurrentScore.LoadInt());
            DisableBonusCardsToExpensives(Saver.Gem.LoadInt());
        }

        #endregion
        
        #region Setup/Update
        
        private void SetupButtons()
        {
            _bonusCard01 = _root.Q<BonusCard>(_bonusCard01Key);
            _bonusCard02 = _root.Q<BonusCard>(_bonusCard02Key);
            _bonusCard03 = _root.Q<BonusCard>(_bonusCard03Key);
            _bonusCard04 = _root.Q<BonusCard>(_bonusCard04Key);
            
            _bonusCardList = new List<BonusCard> { _bonusCard01, _bonusCard02, _bonusCard03, _bonusCard04 };
            
            _menuButton = _root.Q<Button>(_menuButtonKey);
            _restartButton = _root.Q<Button>(_restartButtonKey);
            _pauseButton = _root.Q<Button>(_pauseButtonKey);
        }
        private void InitButtons()
        {
            _menuButton.style.display = DisplayStyle.None;
            _restartButton.style.display = DisplayStyle.None;
        }
        
        private void LoadBonusData()
        {
            _bonusCardDataList = new List<BonusCardData>();
            
            var bonuses = Resources.LoadAll<BonusData>("BonusData");
            
            foreach (var bonus in bonuses)
            {
                var newBonusCard = new BonusCardData
                {
                    BonusCard = _bonusCardList[bonus.Id],
                    BonusData = bonus
                };
                
                _bonusCardDataList.Add(newBonusCard);
            }
        }
        private void InitBonusData()
        {
            if (_bonusCardDataList.Count < 4)
                throw new Exception("Not enough bonus data found. Skipping initialization.");
            
            foreach (var bonusCardData in _bonusCardDataList)
            {
                bonusCardData.BonusCard.SetPrice(bonusCardData.BonusData.Cost);
            }
        }
        
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
            _currentScoreGameOver = _root.Q<Label>(_currentScoreGameOverKey);
            _currentScoreLabel = _root.Q<Label>(_currentScoreKey);
        }
        private void InitCurrentScore()
        {
            _currentScoreGameOver.style.display = DisplayStyle.None;
        }
        private void UpdateCurrentScore(int value)
        {
            _currentScoreLabel.text = value.ToString();
        }

        #endregion
        
        #region Events

        private void OnEnable()
        {
            UiEvents.LooseEvent += OnLooseEvent;
            UiEvents.RefreshUiEvent += OnUpdateBoardEvent;
            
            GameEvents.GemEvent += OnGemRaised;
            GameEvents.CurrentScoreEvent += OnCurrentScoreRaised;
            GameEvents.HighScoreEvent += OnHighScoreRaised;
            GameEvents.HighBallEvent += OnHighBallRaised;
            
            _bonusCard01.clicked += () => OnBonusCardClicked(_bonusCard01, 0);
            _bonusCard02.clicked += () => OnBonusCardClicked(_bonusCard02, 1);
            _bonusCard03.clicked += () => OnBonusCardClicked(_bonusCard03, 2);
            _bonusCard04.clicked += () => OnBonusCardClicked(_bonusCard04, 3);
            
            _menuButton.clicked += OnMenuButtonClicked;
            _restartButton.clicked += OnRestartButtonClicked;
            _pauseButton.clicked += OnPauseButtonClicked;
        }
        
        private void OnDisable()
        {
            UiEvents.LooseEvent -= OnLooseEvent;
            UiEvents.RefreshUiEvent -= OnUpdateBoardEvent;
            
            GameEvents.GemEvent -= OnGemRaised;
            GameEvents.CurrentScoreEvent -= OnCurrentScoreRaised;
            GameEvents.HighScoreEvent -= OnHighScoreRaised;
            GameEvents.HighBallEvent -= OnHighBallRaised;
            
            _menuButton.clicked -= OnMenuButtonClicked;
            _restartButton.clicked -= OnRestartButtonClicked;
            _pauseButton.clicked -= OnPauseButtonClicked;
        }

        #endregion
        
        #region Button Events
        
        private void OnBonusCardClicked(BonusCard bonusCard, int bonusId)
        {
            HideBonusCards();
            
            bonusCard.Show();
            bonusCard.Select();
            
            var bonusData = _bonusCardDataList[bonusId].BonusData;
            BonusManager.BonusEvent?.Invoke(bonusData);
        }
        
        private static void OnMenuButtonClicked()
        {
            SceneManager.LoadScene(GameVar.MenuScene);
        }
        
        private static void OnRestartButtonClicked()
        {
            Saver.ResetAllCurrentScores();
            SceneManager.LoadScene(GameVar.GameScene);
        }
        
        private static void OnPauseButtonClicked()
        {
            UiEvents.PauseEvent.Invoke();
        }
        
        #endregion

        #region Functions
        
        private void OnGemRaised(int value)
        {
            _headerScores.UpdateGem(value);
            DisableBonusCardsToExpensives(value);
        }
        
        private void OnCurrentScoreRaised(int value)
        {
            _currentScoreLabel.text = value.ToString();
        }
        
        private void OnHighScoreRaised(int value)
        {
            _headerScores.UpdateHighScore(value);
        }
        
        private void OnHighBallRaised(int value)
        {
            _headerScores.UpdateHighBall(value);
        }
        
        private void OnUpdateBoardEvent()
        {
            ShowBonusCards();
        }
        
        private void OnLooseEvent()
        {
            HideBonusCards();
            _pauseButton.style.visibility = Visibility.Hidden;
            
            _currentScoreGameOver.style.display = DisplayStyle.Flex;
            _menuButton.style.display = DisplayStyle.Flex;
            _restartButton.style.display = DisplayStyle.Flex;
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
        
        private void DisableBonusCardsToExpensives(int gem)
        {
            foreach (var bonusCardData in _bonusCardDataList)
            {
                if (bonusCardData.BonusData.Cost > gem )
                    bonusCardData.BonusCard.Disable();
                else
                    bonusCardData.BonusCard.Enable();
            }
        }

        #endregion
    }
}
