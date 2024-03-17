using System.Collections.Generic;
using Game.Modules.Manager;
using Game.Ui.Popups.Loose;
using Game.Ui.Popups.Pause;
using Game.Ui.Popups.Settings;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.Popups
{
    public class PopupManager : MonoBehaviour
    {
        #region Statements
        
        private const string _mainLayoutKey = "Main";
        private const string _pausePopupKey = "PausePopup";
        private const string _loosePopupKey = "LoosePopup";
        private const string _settingsPopupKey = "SettingsPopup";

        private UIDocument _uiDocument;
        private VisualElement _root;
        private VisualElement _main;

        private readonly List<Popup> _allPopups = new();
        private Popup _currentPopup;

        private Popup _pausePopup;
        private Popup _settingsPopup;
        private Popup _loosePopup;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;
            _main = _root.Q<VisualElement>(_mainLayoutKey);
        }

        #endregion

        #region Events

        private void OnEnable()
        {
            SetupPopups();
            SubscribeToEvents();
            
            HideMainLayout();
        }
        
        private void OnDisable()
        {
            UnsubscribeFromEvents();
            
            foreach (var popup in _allPopups)
            {
                popup.Dispose();
            }
        }

        #endregion

        #region Functions

        private void SetupPopups()
        {
            _pausePopup = new PausePopup(_root.Q<VisualElement>(_pausePopupKey));
            _settingsPopup = new SettingsPopup(_root.Q<VisualElement>(_settingsPopupKey));
            _loosePopup = new LoosePopup(_root.Q<VisualElement>(_loosePopupKey));
            
            _allPopups.Add(_pausePopup);
            _allPopups.Add(_settingsPopup);
            _allPopups.Add(_loosePopup);
        }
        
        private void SubscribeToEvents()
        {
            UiEvents.ClosePopupEvent += HideMainLayout;
            UiEvents.PauseEvent += ShowPausePopup;
            UiEvents.SettingsEvent += ShowSettingsPopup;
            UiEvents.LooseEvent += ShowLoosePopup;
        }

        private void UnsubscribeFromEvents()
        {
            UiEvents.ClosePopupEvent -= HideMainLayout;
            UiEvents.PauseEvent -= ShowPausePopup;
            UiEvents.SettingsEvent -= ShowSettingsPopup;
            UiEvents.LooseEvent -= ShowLoosePopup;
        }
        
        private void ShowPopup(Popup newPopup)
        {
            BoardManager.OngoingAction = true;
            _main.style.display = DisplayStyle.Flex;
            
            _currentPopup?.Hide();
            _currentPopup = newPopup;
            _currentPopup?.Show();
        }

        private void HideMainLayout()
        {
            BoardManager.OngoingAction = false;
            _main.style.display = DisplayStyle.None;
        }
        
        private void ShowPausePopup()
        {
            ShowPopup(_pausePopup);
        }
        
        private void ShowSettingsPopup()
        {
            ShowPopup(_settingsPopup);
        }
        
        private void ShowLoosePopup()
        {
            ShowPopup(_loosePopup);
        }

        #endregion
    }
}
