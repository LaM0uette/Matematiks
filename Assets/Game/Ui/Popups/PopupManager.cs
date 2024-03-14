using System.Collections.Generic;
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

        private UIDocument _uiDocument;
        private VisualElement _root;
        private VisualElement _main;
        
        List<Popup> _allPopups = new();
        Popup _currentPopup;
        
        Popup _pausePopup;
        
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
            
            _allPopups.Add(_pausePopup);
        }
        
        private void SubscribeToEvents()
        {
            UiEvents.ClosePopup += HideMainLayout;
            UiEvents.PausePopupShow += ShowPausePopup;
        }

        private void UnsubscribeFromEvents()
        {
            UiEvents.ClosePopup -= HideMainLayout;
            UiEvents.PausePopupShow -= ShowPausePopup;
        }
        
        private void ShowPopup(Popup newPopup)
        {
            _main.style.display = DisplayStyle.Flex;
            
            _currentPopup?.Hide();
            _currentPopup = newPopup;
            _currentPopup?.Show();
        }

        private void HideMainLayout()
        {
            _main.style.display = DisplayStyle.None;
        }
        
        private void ShowPausePopup()
        {
            ShowPopup(_pausePopup);
        }

        #endregion
    }
}
