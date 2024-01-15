using Game.Modules.Board.Cells;
using UnityEngine;

namespace Game.Modules.Board.Balls
{
    public class BallSpawner : MonoBehaviour
    {
        #region Statements

        [SerializeField] private GameObject _ballPrefab;
        [SerializeField] private GameObject _firstCell;

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
            Instantiate(_ballPrefab, Vector3.zero, Quaternion.identity, _firstCell.transform);
        }

        #endregion
    }
}
