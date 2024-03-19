using System.Collections;
using Game.Modules.Bonus;
using Game.Modules.Events;
using Game.Modules.Manager;
using Game.Modules.Save;
using Game.Modules.Utils;
using Game.Ui.Events;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game.Modules.Board.Balls
{
    public class Ball : MonoBehaviour
    {
        #region Statements
        
        [Title("Properties")]
        [ShowInInspector, ReadOnly] public int Number { get; set; }
        
        public Color Color { get; private set; }
        public Color BorderColor { get; private set; }
        public bool IsVisited { get; set; }
        
        [Space, Title("Ui")]
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private SpriteRenderer _spriteBorderRenderer;
        [SerializeField] private TMP_Text _tmpNumber;

        [Space, Title("Feedbacks")]
        [SerializeField] private MMF_Player _selectedFeedback;
        [SerializeField] private MMF_Player _destroyFeedback;
        [SerializeField] private MMF_Player _mergeFeedback;
        [SerializeField] private MMF_Player _bonus01Feedback;
        [SerializeField] private MMF_Player _bonus02Feedback;
        [SerializeField] private MMF_Player _bonus03Feedback;
        [SerializeField] private MMF_Player _bonus04Feedback;

        private void Start()
        {
            InitFeedbacks();
        }
        
        private void InitFeedbacks()
        {
            _selectedFeedback.Initialization();
            _destroyFeedback.Initialization();
            _mergeFeedback.Initialization();
            _bonus01Feedback.Initialization();
            _bonus02Feedback.Initialization();
            _bonus03Feedback.Initialization();
            _bonus04Feedback.Initialization();
        }

        #endregion
        
        #region Events
        
        private void OnMouseDown()
        {
            if (BonusManager.CurrentBonus == null)
            {
                OnBallSelected();
                return;
            }
            
            if (BoardManager.OngoingAction)
            {
                BonusManager.CurrentBonus = null;
                UiEvents.RefreshUiEvent.Invoke();
                return;
            }
            
            var bonusId = BonusManager.CurrentBonus.Id;
            
            switch (bonusId)
            {
                case 0: Bonus01(); break;
                case 1 when Number <= 1: return;
                case 1: Bonus02(); break;
                case 2: Bonus03(); break;
                case 3: Bonus04(); break;
            }
            
            var bonusData = BonusManager.CurrentBonus;
            var gem = Saver.Gem.Load();
            gem -= bonusData.Cost;
            Saver.Gem.Save(gem);
            GameEvents.GemEvent.Invoke(gem);
            
            BonusManager.CurrentBonus = null;
            UiEvents.RefreshUiEvent.Invoke();
        }
        
        private void OnMouseEnter()
        {
            OnBallSelected();
        }

        #endregion

        #region MouseEvents

        private void OnBallSelected()
        {
            if (BoardManager.IsPressing == false) 
                return;
            
            BallEvents.CurrentBallSelectedEvent.Invoke(this);
        }

        #endregion

        #region Functions

        public void SetNum(int number)
        {
            switch (number)
            {
                case > 99: return;
                case < 1: number = 1; break;
            }

            Number = number;
            SetBallColor(number);
        }

        public void PutInBackground()
        {
            StartCoroutine(FadeToBackground());
        }
        
        private IEnumerator FadeToBackground()
        {
            float duration = 0.1f; // Durée de la transition en secondes
            float currentTime = 0f;

            // Sauvegardez les couleurs initiales
            Color initialSpriteColor = _spriteRenderer.color;
            Color initialBorderColor = _spriteBorderRenderer.color;
            Color initialTxtColor = _tmpNumber.color;

            // Définissez les nouvelles opacités
            float targetSpriteOpacity = 0.5f;
            float targetBorderOpacity = 0.5f;
            float targetTxtOpacity = 0.75f;

            while (currentTime < duration)
            {
                if (!BoardManager.IsPressing)
                    RestoreBackground();
                
                // Calculez le ratio de progression de la transition
                float t = currentTime / duration;
                float smoothT = Mathf.SmoothStep(0f, 1f, t);

                // Interpolez les couleurs pour graduellement changer l'opacité
                _spriteRenderer.color = new Color(initialSpriteColor.r, initialSpriteColor.g, initialSpriteColor.b, Mathf.Lerp(initialSpriteColor.a, targetSpriteOpacity, smoothT));
                _spriteBorderRenderer.color = new Color(initialBorderColor.r, initialBorderColor.g, initialBorderColor.b, Mathf.Lerp(initialBorderColor.a, targetBorderOpacity, smoothT));
                _tmpNumber.color = new Color(initialTxtColor.r, initialTxtColor.g, initialTxtColor.b, Mathf.Lerp(initialTxtColor.a, targetTxtOpacity, smoothT));

                // Incrémentez le temps passé
                currentTime += Time.deltaTime;

                // Attendez jusqu'à la prochaine frame avant de continuer
                yield return null;
            }

            // Assurez-vous que les couleurs finales sont correctement appliquées
            _spriteRenderer.color = new Color(initialSpriteColor.r, initialSpriteColor.g, initialSpriteColor.b, targetSpriteOpacity);
            _spriteBorderRenderer.color = new Color(initialBorderColor.r, initialBorderColor.g, initialBorderColor.b, targetBorderOpacity);
            _tmpNumber.color = new Color(initialTxtColor.r, initialTxtColor.g, initialTxtColor.b, targetTxtOpacity);

            if (!BoardManager.IsPressing)
                RestoreBackground();
        }
        
        public void RestoreBackground()
        {
            StopCoroutine(FadeToBackground());
            StartCoroutine(RestoreBackgroundCoroutine());
        }
        
        private IEnumerator RestoreBackgroundCoroutine()
        {
            float duration = 0.1f; // Durée de la transition en secondes
            float currentTime = 0f;

            // Couleurs actuelles au début de la transition
            Color startSpriteColor = _spriteRenderer.color;
            Color startBorderColor = _spriteBorderRenderer.color;
            Color startTxtColor = _tmpNumber.color;

            // Couleurs cibles (originales ou spécifiques)
            Color targetSpriteColor = Color; // Mettez votre couleur cible ici
            Color targetBorderColor = BorderColor; // Mettez votre couleur cible ici
            Color targetTxtColor = GetContrastingTextColor(targetSpriteColor); // Calculez ou définissez votre couleur cible ici

            while (currentTime < duration)
            {
                float t = currentTime / duration;
                float smoothT = Mathf.SmoothStep(0f, 1f, t);

                _spriteRenderer.color = Color.Lerp(startSpriteColor, targetSpriteColor, smoothT);
                _spriteBorderRenderer.color = Color.Lerp(startBorderColor, targetBorderColor, smoothT);
                _tmpNumber.color = Color.Lerp(startTxtColor, targetTxtColor, smoothT);

                currentTime += Time.deltaTime;
                yield return null;
            }

            // Appliquez les couleurs finales pour garantir l'exactitude
            _spriteRenderer.color = targetSpriteColor;
            _spriteBorderRenderer.color = targetBorderColor;
            _tmpNumber.color = targetTxtColor;
        }

        private void SetBallColor(int number)
        {
            var color = GetBallColor(number);
            _spriteRenderer.color = color;
            Color = color;
            
            var borderColor = GetBallBorderColor(number);
            //borderColor.a = 0.7f;
            _spriteBorderRenderer.color = borderColor;
            BorderColor = borderColor;
            
            _tmpNumber.color = GetContrastingTextColor(_spriteRenderer.color);
            _tmpNumber.text = number.ToString();
        }
        
        private static Color GetBallColor(int number)
        {
            if (number % 10 == 0)
                return ColorVar.Ball10Color;

            var colorIndex = number % 10;

            return colorIndex switch
            {
                1 => ColorVar.Ball1Color,
                2 => ColorVar.Ball2Color,
                3 => ColorVar.Ball3Color,
                4 => ColorVar.Ball4Color,
                5 => ColorVar.Ball5Color,
                6 => ColorVar.Ball6Color,
                7 => ColorVar.Ball7Color,
                8 => ColorVar.Ball8Color,
                9 => ColorVar.Ball9Color,
                _ => ColorVar.Ball10Color
            };
        }

        private static Color GetBallBorderColor(int number)
        {
            return number switch
            {
                >= 1 and <= 10 => new Color(0, 0, 0, 0),
                >= 11 and <= 20 => ColorVar.Ball2Color,
                >= 21 and <= 30 => ColorVar.Ball3Color,
                >= 31 and <= 40 => ColorVar.Ball4Color,
                >= 41 and <= 50 => ColorVar.Ball5Color,
                >= 51 and <= 60 => ColorVar.Ball6Color,
                >= 61 and <= 70 => ColorVar.Ball7Color,
                >= 71 and <= 80 => ColorVar.Ball8Color,
                >= 81 and <= 90 => ColorVar.Ball9Color,
                >= 91 and <= 100 => ColorVar.Ball10Color,
                _ => ColorVar.Ball10Color
            };
        }
        
        private static Color GetContrastingTextColor(Color backgroundColor)
        {
            var luminance = 0.2126f * backgroundColor.r + 0.7152f * backgroundColor.g + 0.0722f * backgroundColor.b;
            return luminance > 0.5f ? ColorVar.BlackColor : ColorVar.WhiteColor;
        }
        
        private void Bonus01()
        {
            SoundManager.Instance.PlayBonus01Sound();
            _bonus01Feedback.PlayFeedbacks();
            
            //Destroy(gameObject);
        }
        
        private void Bonus02()
        {
            SoundManager.Instance.PlayBonus02Sound();
            _bonus02Feedback.PlayFeedbacks();
            
            SetNum(--Number);
        }
        
        private void Bonus03()
        {
            SoundManager.Instance.PlayBonus03Sound();
            _bonus03Feedback.PlayFeedbacks();
            
            SetNum(++Number);
            CheckForUpdateHighBall(Number);
        }
        
        private void Bonus04()
        {
            SoundManager.Instance.PlayBonus04Sound();
            
            var balls = FindObjectsOfType<Ball>();
            foreach (var ball in balls)
            {
                if (ball.Number == Number)
                {
                    ball._bonus04Feedback.PlayFeedbacks();
                }
            }
        }
        
        private static void CheckForUpdateHighBall(int ballNumber)
        {
            if (ballNumber <= Saver.HighBall.Load())
                return;
            
            Saver.HighBall.Save(ballNumber);
            GameEvents.HighBallEvent.Invoke(ballNumber);
        }

        #endregion

        #region Feedbacks

        public void PlaySelectedFeedback()
        {
            _selectedFeedback.PlayFeedbacks();
        }
        
        public void PlayDestroyFeedback()
        {
            _destroyFeedback.PlayFeedbacks();
        }
        
        public void PlayMergeFeedback()
        {
            _mergeFeedback.PlayFeedbacks();
        }

        #endregion
        
        #region Odin

        [Button]
        public void SetNumAndColor()
        {
            var number = Number + 1;
            SetNum(number);
        }

        #endregion
    }
}
