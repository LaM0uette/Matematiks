using System.Collections.Generic;
using Game.Modules.Board;
using Game.Ui.Popups.Loose;
using Game.Ui.Popups.Pause;
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

        private UIDocument _uiDocument;
        private VisualElement _root;
        private VisualElement _main;

        private readonly List<Popup> _allPopups = new();
        private Popup _currentPopup;

        private Popup _pausePopup;
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
            _loosePopup = new LoosePopup(_root.Q<VisualElement>(_loosePopupKey));
            
            _allPopups.Add(_pausePopup);
            _allPopups.Add(_loosePopup);
        }
        
        private void SubscribeToEvents()
        {
            UiEvents.ClosePopupEvent += HideMainLayout;
            UiEvents.PauseEvent += ShowPausePopup;
            
            UiEvents.LooseEvent += ShowLoosePopup;
        }

        private void UnsubscribeFromEvents()
        {
            UiEvents.ClosePopupEvent -= HideMainLayout;
            UiEvents.PauseEvent -= ShowPausePopup;
            
            UiEvents.LooseEvent -= ShowLoosePopup;
        }
        
        private void ShowPopup(Popup newPopup)
        {
            BoardHandler.OngoingAction = true;
            _main.style.display = DisplayStyle.Flex;
            
            _currentPopup?.Hide();
            _currentPopup = newPopup;
            _currentPopup?.Show();
        }

        private void HideMainLayout()
        {
            BoardHandler.OngoingAction = false;
            _main.style.display = DisplayStyle.None;
        }
        
        private void ShowPausePopup()
        {
            ShowPopup(_pausePopup);
        }
        
        private void ShowLoosePopup()
        {
            ShowPopup(_loosePopup);
        }

        #endregion
    }
}
