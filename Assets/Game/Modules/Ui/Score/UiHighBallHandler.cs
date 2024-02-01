using Game.Modules.Board.Balls;
using Game.Modules.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.Ui.Score
{
    public class UiHighBallHandler : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Ball")]
        [SerializeField] private Ball _highBall;

        private void Start()
        {
            var highBall = Saver.HighBall.LoadInt();
            if (highBall <= 0) highBall = 1;
            
            _highBall.SetNum(highBall);
            InvokeRepeating(nameof(UpdateHighBall), 0, 0.2f);
        }
        
        #endregion
        
        #region Functions

        private void UpdateHighBall()
        {
            var maxBall = Saver.HighBall.LoadInt();
            
            if (maxBall > _highBall.Number)
            {
                _highBall.SetNum(maxBall);
            }
        }

        #endregion
    }
}
