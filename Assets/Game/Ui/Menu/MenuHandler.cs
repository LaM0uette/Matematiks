using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Ui.Menu
{
    public class MenuHandler : MonoBehaviour
    {
        #region Statements

        private UIDocument _uiDocument;
        private VisualElement _root;
        
        private HeaderScores.HeaderScores _headerScores;
        
        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _root = _uiDocument.rootVisualElement;

            SetupHeaderScores();
        }

        private void Start()
        {
            InitHeaderScores();
        }

        #endregion

        #region Functions

        private void SetupHeaderScores()
        {
            _headerScores = _root.Q<HeaderScores.HeaderScores>();
        }
        
        private void InitHeaderScores()
        {
            _headerScores.UpdateScore();
        }

        #endregion
    }
}
