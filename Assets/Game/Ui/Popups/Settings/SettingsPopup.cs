using Game.Modules.Manager;
using Game.Ui.Components.GameButton;
using Game.Ui.Events;
using UnityEngine.UIElements;

namespace Game.Ui.Popups.Settings
{
    public class SettingsPopup : Popup
    {
        #region Statements
        
        private const string _closeButtonKey = "SettingsCloseButton";
        private const string _backButtonKey = "SettingsBackButton";
        private const string _soundEffectsButtonKey = "SettingsSoundEffectsButton";
        
        private Button _closeButton;
        private Button _backButton;
        private GameButton _soundEffectsButton;

        public SettingsPopup(VisualElement topElement) : base(topElement)
        {
        }

        #endregion

        #region Functions
        
        protected override void SetVisualElements()
        {
            _closeButton = TopElement.Q<Button>(_closeButtonKey);
            _backButton = TopElement.Q<Button>(_backButtonKey);
            _soundEffectsButton = TopElement.Q<GameButton>(_soundEffectsButtonKey);
        }

        protected override void RegisterButtonCallbacks()
        {
            _closeButton.clicked += OnCloseButtonClicked;
            _backButton.clicked += OnBackButtonClicked;
            _soundEffectsButton.clicked += OnSoundEffectButtonClicked;
        }
        
        protected override void UnregisterButtonCallbacks()
        {
            _closeButton.clicked -= OnCloseButtonClicked;
            _backButton.clicked -= OnBackButtonClicked;
            _soundEffectsButton.clicked -= OnSoundEffectButtonClicked;
        }
        
        public override void Show()
        {
            base.Show();
            UpdateIcon();
        }
        
        private void OnCloseButtonClicked()
        {
            Hide();
            UiEvents.ClosePopupEvent.Invoke();
        }
        
        private void OnBackButtonClicked()
        {
            Hide();
            UiEvents.PauseEvent.Invoke();
        }
        
        private void OnSoundEffectButtonClicked()
        {
            BoardManager.VolumeIsMute = !BoardManager.VolumeIsMute;
            UpdateIcon();
        }

        private void UpdateIcon()
        {
            var iconName = BoardManager.VolumeIsMute ? "Icons/volume-mute" : "Icons/volume";
            _soundEffectsButton.IconName = iconName;
        }

        #endregion
    }
}
