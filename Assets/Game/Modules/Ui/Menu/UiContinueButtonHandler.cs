using Game.Modules.Utils;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Modules.Ui.Menu
{
    public class UiContinueButtonHandler : MonoBehaviour
    {
        #region Statements

        private UIDocument _uiDocument;
        private VisualElement _rootElement;
        
        private Button _continueButton;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _rootElement = _uiDocument.rootVisualElement;
            
            SetElements();
        }
        
        private void Start()
        {
            var currentScore = Saver.GetCurrentScore();
            
            if (currentScore > 0)
            {
                _continueButton.SetEnabled(true);
            }
            else
            {
                _continueButton.SetEnabled(false);
            }
        }

        #endregion

        #region Functions

        private void SetElements()
        {
            _continueButton = _rootElement.Q<Button>("button_continue");
        }

        #endregion
    }
}
