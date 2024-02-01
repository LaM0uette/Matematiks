using Game.Modules.Utils;
using Obvious.Soap;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Game.Modules.Ui.Score
{
    public class UiCurrentScoreHandler : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Score")]
        [SerializeField] private TMP_Text _tmpCurrentScore;
        [SerializeField] private ScriptableEventInt _scoreEvent;

        private void Start()
        {
            _tmpCurrentScore.text = Saver.CurrentScore.LoadInt().ToString();
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
            _tmpCurrentScore.text = value.ToString();
        }

        #endregion
    }
}
