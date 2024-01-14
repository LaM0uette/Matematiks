using System;
using UnityEngine;

namespace Proto.Modules.Point
{
    public class p_PointSpawner : MonoBehaviour
    {
        #region Statements

        [SerializeField] private GameObject _ballPrefab;
        [SerializeField] private GameObject _firstPoint;

        #endregion

        #region Events

        private void LateUpdate()
        {
            if (!FirstPointIsEmpty())
                return;
            
            SpawnBall();
        }

        #endregion

        #region Functions

        private bool FirstPointIsEmpty()
        {
            var firstPoint = _firstPoint.GetComponent<p_Point>();
            return firstPoint.IsEmpty;
        }
        
        private void SpawnBall()
        {
            var ballGo = Instantiate(_ballPrefab, _firstPoint.transform);
            ballGo.transform.localPosition = Vector3.zero;
        }

        #endregion
    }
}
