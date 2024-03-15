using System.Collections.Generic;
using Game.Modules.Bonus;
using Game.Modules.Events;
using Game.Modules.Manager;
using Game.Modules.Utils;
using Game.Ui.Components.BonusCard;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.Game
{
    public class GameHandler : MonoBehaviour
    {
        #region Statements
        
        private const string _currentScoreKey = "CurrentScoreValue";
        private const string _bonusCard01Key = "BonusCard01";
        private const string _bonusCard02Key = "BonusCard02";
        private const string _bonusCard03Key = "BonusCard03";
        private const string _bonusCard04Key = "BonusCard04";
        private const string _pauseButtonKey = "PauseButton";
        
        private UIDocument _uiDocument;
        private VisualElement _root;
        
        private HeaderScores.HeaderScores _headerScores;
        
        private Label _currentScoreLabel;
        
        private Dictionary<int, BonusData> _bonusDataDictionary;
        private BonusCard _bonusCard01;
        private BonusCard _bonusCard02;
        private BonusCard _bonusCard03;
        private BonusCard _bonusCard04;
        
        private Button _pauseButton;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
            
            LoadBonusData();
            
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
        
        private void LoadBonusData()
        {
            var bonuses = Resources.LoadAll<BonusData>("BonusData");
            _bonusDataDictionary = new Dictionary<int, BonusData>();

            foreach (var bonus in bonuses)
            {
                if (!_bonusDataDictionary.TryAdd(bonus.BonusId, bonus))
                {
                    Debug.LogWarning($"Duplicate Bonus ID found: {bonus.BonusId}. Ignoring duplicate.");
                }
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
            
            DataEvents.GemEvent += OnGemRaised;
            DataEvents.CurrentScoreEvent += OnCurrentScoreRaised;
            
            _bonusCard01.clicked += () => OnBonusCardClicked(_bonusCard01, 0);
            _bonusCard02.clicked += () => OnBonusCardClicked(_bonusCard02, 1);
            _bonusCard03.clicked += () => OnBonusCardClicked(_bonusCard03, 2);
            _bonusCard04.clicked += () => OnBonusCardClicked(_bonusCard04, 3);
            
            _pauseButton.clicked += OnPauseButtonClicked;
        }
        
        private void OnDisable()
        {
            UiEvents.LooseEvent -= OnLooseEvent;
            UiEvents.RefreshUiEvent -= OnUpdateBoardEvent;
            
            DataEvents.GemEvent -= OnGemRaised;
            DataEvents.CurrentScoreEvent -= OnCurrentScoreRaised;
            
            _pauseButton.clicked -= OnPauseButtonClicked;
        }

        #endregion
        
        #region Button Events
        
        private void OnBonusCardClicked(BonusCard bonusCard, int bonusId)
        {
            HideBonusCards();
            
            bonusCard.Show();
            bonusCard.Select();
            
            if (_bonusDataDictionary.TryGetValue(bonusId, out var bonusData))
            {
                BonusManager.Instance.BonusEvent?.Invoke(bonusData);
            }
            else
            {
                Debug.LogWarning($"No bonus data found for ID: {bonusId}");
                ShowBonusCards();
            }
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
