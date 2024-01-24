using Game.Modules.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Modules.Ui.Menu
{
    public class ButtonPlayGame : MonoBehaviour
    {
        #region Events

        public void OnResumeGame()
        {
            SceneManager.LoadScene(GameVar.GameScene);
        }
        
        public void OnNewGame()
        {
            Saver.ResetCurrentScore();
            Saver.ResetCurrentBalls();
            SceneManager.LoadScene(GameVar.GameScene);
        }

        #endregion
    }
}
