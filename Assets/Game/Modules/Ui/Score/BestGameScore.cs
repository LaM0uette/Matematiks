using Game.Modules.Utils;
using Obvious.Soap;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game.Modules.Ui.Score
{
    public class BestGameScore : MonoBehaviour
    {
        #region Statements

        [Space, Title("Score")]
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private IntVariable _scoreVariable;

        private void Start()
        {
            _scoreText.text = Saver.GetBestScore().ToString();
            InvokeRepeating(nameof(UpdateScore), 0, 0.2f);
        }
        
        #endregion
        
        #region Functions

        private void UpdateScore()
        {
            var bestScore = Saver.GetBestScore();
            
            if (_scoreVariable.Value > bestScore)
            {
                Saver.SaveBestScore(_scoreVariable.Value);
                _scoreText.text = _scoreVariable.Value.ToString();
            }
        }

        #endregion
    }
}