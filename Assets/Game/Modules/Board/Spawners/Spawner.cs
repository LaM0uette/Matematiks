using Game.Modules.Board.Cells;
using Obvious.Soap;
using UnityEngine;

namespace Game.Modules.Board.Spawners
{
    public class Spawner : MonoBehaviour
    {
        #region Statements

        public ScriptableListWeightedBall WeightedBalls;
        public GameObject BallPrefab;
        public GameObject FirstCell;
        
        private ISpawnMode _spawnMode;

        private void Awake()
        {
            _spawnMode = gameObject.AddComponent<BaseSpawnMode>();
        }

        #endregion
        
        #region Events

        private void LateUpdate()
        {
            if (!FirstCellIsEmpty())
                return;
            
            _spawnMode.SpawnBall();
        }

        #endregion
        
        #region Functions

        private bool FirstCellIsEmpty()
        {
            var firstCell = FirstCell.GetComponent<Cell>();
            return firstCell.IsEmpty;
        }

        #endregion
    }
}
