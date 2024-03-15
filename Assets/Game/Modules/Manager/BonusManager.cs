using System;
using Game.Modules.Bonus;
using UnityEngine;

namespace Game.Modules.Manager
{
    public class BonusManager : MonoBehaviour
    {
        #region Statements

        public static BonusManager Instance { get; private set; }
        
        public Action<BonusData> BonusEvent;
        
        public BonusData CurrentBonus { get; set; }
        
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
