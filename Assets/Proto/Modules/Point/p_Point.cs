using JetBrains.Annotations;
using Proto.Modules.NumBall;
using UnityEngine;

namespace Proto.Modules.Point
{
    public class p_Point : MonoBehaviour
    {
        #region Statements

        public bool IsEmpty;
        
        [SerializeField, CanBeNull] private GameObject _nextPoint;

        #endregion

        #region Events

        private void LateUpdate()
        {
            CheckIsEmpty();
            MoveBall();
        }

        #endregion

        #region Functions
        
        private void CheckIsEmpty()
        {
            var childCount = transform.childCount;
            IsEmpty = childCount == 0;
        }
        
        private void MoveBall()
        {
            var nextIsPointEmpty = NextPointIsEmpty();
            if (IsEmpty || !nextIsPointEmpty || _nextPoint == null) 
                return;
            
            var childGo = transform.GetChild(0).gameObject;
            var pNumBall = childGo.GetComponent<p_NumBall>();
            
            if (pNumBall.IsBlocked) 
                return;
            
            childGo.transform.SetParent(_nextPoint.transform);
            childGo.transform.localPosition = Vector3.zero;
        }
        
        private bool NextPointIsEmpty()
        {
            if (_nextPoint == null) 
                return IsEmpty;
            
            var nextPoint = _nextPoint.GetComponent<p_Point>();
            return nextPoint.IsEmpty;
        }

        #endregion
    }
}
