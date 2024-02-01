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
            Saver.CurrentScore.Delete();
            Saver.CurrentBalls.Delete();
            Saver.CurrentWeightedBalls.Delete();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        #endregion
    }
}
