using Game.Modules.Utils;
using Obvious.Soap;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game.Modules.Ui.Score
{
    public class UiHighScoreHandler : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Score")]
        [SerializeField] private TMP_Text _tmpHighScore;
        [SerializeField] private ScriptableEventInt _scoreEvent;

        private void Start()
        {
            _tmpHighScore.text = Saver.HighScore.LoadInt().ToString();
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

        private void OnScoreRaised(int value)
        {
            var bestScore = Saver.HighScore.LoadInt();

            if (value <= bestScore) return;
            
            Saver.HighScore.Save(value);
            _tmpHighScore.text = value.ToString();
        }

        #endregion
    }
}
