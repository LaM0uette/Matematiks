using System.Linq;
using Game.Modules.Board.Cells;
using Obvious.Soap;
using UnityEngine;

namespace Game.Modules.Board.Balls
{
    public class BallSpawner : MonoBehaviour
    {
        #region Statements

        [SerializeField] private GameObject _ballPrefab;
        [SerializeField] private GameObject _firstCell;
        [SerializeField] private ScriptableListWeightedBall _weightedBalls;

        #endregion

        #region Events

        private void LateUpdate()
        {
            if (!FirstCellIsEmpty())
                return;
            
            SpawnBall();
        }

        #endregion

        #region Functions

        private bool FirstCellIsEmpty()
        {
            var firstCell = _firstCell.GetComponent<Cell>();
            return firstCell.IsEmpty;
        }
        
        private void SpawnBall()
        {
            var ballGo = Instantiate(_ballPrefab, _firstCell.transform);
            ballGo.transform.localPosition = Vector3.zero;
            
            var ball = ballGo.GetComponent<Ball>();
            ball.SetNum(GetWeightedRandomNumber());
        }
        
        private int GetWeightedRandomNumber()
        {
            var balls = _weightedBalls;
            
            var totalWeight = balls.Sum(ballNumber => ballNumber.Weight);
            var randomNumber = Random.Range(0, totalWeight);
            
            foreach (var ball in balls)
            {
                randomNumber -= ball.Weight;
                if (randomNumber <= 0)
                {
                    return ball.Number;
                }
            }

            return 1;
        }

        #endregion
    }
}
