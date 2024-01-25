using Obvious.Soap;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game.Modules.Ui.Score
{
    public class GameScore : MonoBehaviour
    {
        #region Statements

        [Space, Title("Score")]
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private IntVariable _scoreVariable;

        private void Start()
        {
            InvokeRepeating(nameof(UpdateScore), 0, 0.2f);
        }

        #endregion

        #region Functions

        private void UpdateScore()
        {
            _scoreText.text = _scoreVariable.Value.ToString();
        }

        #endregion
    }
}
