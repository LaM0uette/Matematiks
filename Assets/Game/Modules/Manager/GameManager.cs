using Game.Modules.Board.Balls;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.Manager
{
    public class GameManager : MonoBehaviour
    {
        #region Statements

        public static GameManager Instance { get; private set; }
        public static Ball BallScore { get; private set; }
        
        [Space, Title("Score")]
        [ShowInInspector, ReadOnly] public int MaxBallNumber = 1;
        
        private void Awake()
        {
            Instance ??= this;
        }

        private void Start()
        {
            MaxBallNumber = 1;
            BallScore = GameObject.FindGameObjectWithTag("BallMaxScore").transform.GetComponent<Ball>();
            BallScore.gameObject.SetActive(false);
            
            SetQualitySettings();
            
            //Saver.ResetBestScore();
            //Saver.ResetMaxBall();
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
