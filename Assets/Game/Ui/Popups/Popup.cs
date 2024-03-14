using System;
using UnityEngine.UIElements;

namespace Game.Ui.Popups
{
    public class Popup : IDisposable
    {
        #region Statements

        protected readonly VisualElement TopElement;
        
        private readonly bool _hideOnAwake;

        protected Popup(VisualElement topElement, bool hideOnAwake = true)
        {
            TopElement = topElement ?? throw new ArgumentNullException(nameof(topElement));
            _hideOnAwake = hideOnAwake;
            
            Initialize();
        }

        protected virtual void Initialize()
        {
            if (_hideOnAwake)
            {
                Hide();
            }
            
            SetVisualElements();
            RegisterButtonCallbacks();
        }

        #endregion

        #region Functions

        protected virtual void SetVisualElements() {}
        
        protected virtual void RegisterButtonCallbacks() {}
        
        protected virtual void UnregisterButtonCallbacks() {}

        public virtual void Show()
        {
            TopElement.style.display = DisplayStyle.Flex;
        }

        public virtual void Hide()
        {
            TopElement.style.display = DisplayStyle.None;
        }

        public virtual void Dispose()
        {
            UnregisterButtonCallbacks();
        }

        #endregion
    }
}
