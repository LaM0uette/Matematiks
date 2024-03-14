using System;
using UnityEngine.UIElements;

namespace Game.Ui.Popups
{
    public class Popup : IDisposable
    {
        #region Statements

        protected VisualElement _topElement;
        protected bool _hideOnAwake = true;
        
        public Popup(VisualElement topElement)
        {
            _topElement = topElement ?? throw new ArgumentNullException(nameof(topElement));
            Initialize();
        }
        
        public virtual void Initialize()
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
            _topElement.style.display = DisplayStyle.Flex;
        }

        public virtual void Hide()
        {
            _topElement.style.display = DisplayStyle.None;
        }

        public virtual void Dispose()
        {
            UnregisterButtonCallbacks();
        }

        #endregion
    }
}
