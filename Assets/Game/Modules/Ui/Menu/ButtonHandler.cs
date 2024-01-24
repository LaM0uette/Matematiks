using Game.Modules.Utils;
using UnityEngine;

namespace Game.Modules.Ui.Menu
{
    public class ButtonHandler : MonoBehaviour
    {
        #region Statements

        [SerializeField] private GameObject _buttonResume;
        [SerializeField] private GameObject _buttonResumeOff;

        private void Start()
        {
            var currentScore = Saver.GetCurrentScore();
            
            if (currentScore > 0)
            {
                _buttonResume.SetActive(true);
                _buttonResumeOff.SetActive(false);
            }
            else
            {
                _buttonResume.SetActive(false);
                _buttonResumeOff.SetActive(true);
            }
        }

        #endregion
    }
}
