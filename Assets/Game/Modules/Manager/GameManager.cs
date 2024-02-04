using UnityEngine;

namespace Game.Modules.Manager
{
    public class GameManager : MonoBehaviour
    {
        #region Statements

        public static GameManager Instance { get; private set; }
        
        public int CurrentBonus { get; set; }
        
        private void Awake()
        {
            Instance ??= this;
        }

        private void Start()
        {
            SetQualitySettings();
        }

        #endregion

        #region Functions

        private static void SetQualitySettings()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 240;
        }

        #endregion
    }
}
