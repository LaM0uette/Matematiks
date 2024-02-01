using System.Linq;
using Game.Modules.Board.Balls;
using UnityEngine;

namespace Game.Modules.Board.Spawners
{
    public class BaseSpawnMode : MonoBehaviour, ISpawnMode
    {
        #region Statements

        private Spawner _spawner;

        private void Awake()
        {
            _spawner = GetComponent<Spawner>();
        }

        #endregion
        
        #region Functions
        
        public void SpawnBall(GameObject prefab, Transform spawnTransform)
        {
            var ballGo = Instantiate(prefab, spawnTransform);
            ballGo.transform.localPosition = Vector3.zero;
            
            var ball = ballGo.GetComponent<Ball>();
            
            var weightedRandomNumber = GetWeightedRandomNumber();
            ball.SetNum(weightedRandomNumber);
        }
        
        private int GetWeightedRandomNumber()
        {
            var balls = _spawner.WeightedBalls;
            
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
