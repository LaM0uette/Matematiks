using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Modules.Ui
{
    public class ReplayButton : MonoBehaviour
    {
        #region Events

        private void OnMouseDown()
        {
            SceneManager.LoadScene("Game");
        }

        #endregion
    }
}
