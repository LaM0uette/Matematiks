using Game.Modules.Utils;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Modules.Ui.Score
{
    public class UiHighBallHandler : MonoBehaviour
    {
        #region Statements
        
        [SerializeField] private ScriptableEventInt _scoreEvent;

        private int _highBallNulmber;
        
        private UIDocument _uiDocument;
        private VisualElement _rootElement;
        
        private VisualElement _veHighball;
        private Label _labelHighball;

        private void Awake()
        {
            _uiDocument = GetComponent<UIDocument>();
            _rootElement = _uiDocument.rootVisualElement;
            
            SetElements();
        }
        
        private void Start()
        {
            var highBall = Saver.HighBall.LoadInt();
            if (highBall <= 0) highBall = 1;
            
            if (highBall <= _highBallNulmber)
                return;
            
            _highBallNulmber = highBall;
            SetUi(highBall);
        }
        
        #endregion
        
        #region Events

        private void OnEnable()
        {
            _scoreEvent.OnRaised += OnScoreRaised;
        }
        
        private void OnDisable()
        {
            _scoreEvent.OnRaised -= OnScoreRaised;
        }

        #endregion
        
        #region Functions
        
        private void SetElements()
        {
            _veHighball = _rootElement.Q<VisualElement>("ve_highball");
            _labelHighball = _rootElement.Q<Label>("label_highball");
        }

        private void OnScoreRaised(int _)
        {
            var maxBall = Saver.HighBall.LoadInt();
            var currentBall = int.Parse(_labelHighball.text);
            
            if (maxBall > currentBall)
            {
                SetUi(maxBall);
            }
        }
        
        private void SetUi(int highBall)
        {
            var color = GetBallColor(highBall);
            var textColor = GetContrastingTextColor(color);
            
            _labelHighball.text = highBall.ToString();
            _veHighball.style.unityBackgroundImageTintColor = color;
            _labelHighball.style.color = textColor;
        }
        
        private static Color GetBallColor(int number)
        {
            if (number % 10 == 0)
                return ColorVar.Ball10Color;

            var colorIndex = number % 10;
            var isDark = number > 10;

            return colorIndex switch
            {
                1 => isDark ? ColorVar.Ball1ColorDark : ColorVar.Ball1Color,
                2 => isDark ? ColorVar.Ball2ColorDark : ColorVar.Ball2Color,
                3 => isDark ? ColorVar.Ball3ColorDark : ColorVar.Ball3Color,
                4 => isDark ? ColorVar.Ball4ColorDark : ColorVar.Ball4Color,
                5 => isDark ? ColorVar.Ball5ColorDark : ColorVar.Ball5Color,
                6 => isDark ? ColorVar.Ball6ColorDark : ColorVar.Ball6Color,
                7 => isDark ? ColorVar.Ball7ColorDark : ColorVar.Ball7Color,
                8 => isDark ? ColorVar.Ball8ColorDark : ColorVar.Ball8Color,
                9 => isDark ? ColorVar.Ball9ColorDark : ColorVar.Ball9Color,
                _ => ColorVar.Ball10ColorDark
            };
        }
        
        private static Color GetContrastingTextColor(Color backgroundColor)
        {
            var luminance = 0.2126f * backgroundColor.r + 0.7152f * backgroundColor.g + 0.0722f * backgroundColor.b;
            return luminance > 0.5f ? ColorVar.BlackColor : ColorVar.WhiteColor;
        }

        

        #endregion
    }
}
