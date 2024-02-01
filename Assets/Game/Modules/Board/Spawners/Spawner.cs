using Game.Modules.Board.Cells;
using Obvious.Soap;
using UnityEngine;

namespace Game.Modules.Board.Spawners
{
    public class Spawner : MonoBehaviour
    {
        #region Statements

        public ScriptableListWeightedBall WeightedBalls;
        
        [SerializeField] private GameObject _ballPrefab;
        [SerializeField] private GameObject _firstCell;
        
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
            
            _spawnMode.SpawnBall(_ballPrefab, _firstCell.transform);
        }

        #endregion
        
        #region Functions

        private bool FirstCellIsEmpty()
        {
            var firstCell = _firstCell.GetComponent<Cell>();
            return firstCell.IsEmpty;
        }

        #endregion
    }
}
