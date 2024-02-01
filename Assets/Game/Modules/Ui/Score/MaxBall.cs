using Game.Modules.Board.Balls;
using Game.Modules.Utils;
using Obvious.Soap;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.Ui.Score
{
    public class MaxBall : MonoBehaviour
    {
        #region Statements

        [Space, Title("Ball")]
        [SerializeField] private Ball _maxBall;

        private void Start()
        {
            var highBall = Saver.HighBall.LoadInt();
            if (highBall <= 0) highBall = 1;
            
            _maxBall.SetNum(highBall);
            InvokeRepeating(nameof(UpdateMaxBall), 0, 0.2f);
        }
        
        #endregion
        
        #region Functions

        private void UpdateMaxBall()
        {
            var maxBall = Saver.HighBall.LoadInt();
            
            if (maxBall > _maxBall.Number)
            {
                _maxBall.SetNum(maxBall);
            }
        }

        #endregion
    }
}
