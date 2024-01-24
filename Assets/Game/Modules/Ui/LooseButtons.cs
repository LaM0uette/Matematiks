using Game.Modules.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Modules.Ui
{
    public class LooseButtons : MonoBehaviour
    {
        #region Events

        public void OnMenu()
        {
            SceneManager.LoadScene(GameVar.MenuScene);
        }
        
        public void OnReplay()
        {
            Saver.ResetCurrentScore();
            Saver.ResetCurrentBalls();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        #endregion
    }
}
