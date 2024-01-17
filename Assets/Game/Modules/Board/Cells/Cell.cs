using System.Collections;
using Game.Modules.Board.Balls;
using Game.Modules.Utils;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.Board.Cells
{
    public class Cell : MonoBehaviour
    {
        #region Statements

        [ShowInInspector, ReadOnly] public bool IsEmpty { get; set; }
        
        [SerializeField, CanBeNull] private GameObject _nextCell;

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
            var nextCellIsEmpty = NextCellIsEmpty();
            
            if (IsEmpty || !nextCellIsEmpty || _nextCell == null) 
                return;
            
            var childGo = transform.GetChild(0).gameObject;
            var ball = childGo.GetComponent<Ball>();
            
            if (ball.IsBlocked) 
                return;
            
            childGo.transform.SetParent(_nextCell.transform);
            StartCoroutine(AnimateMoveBalls(childGo, childGo.transform.localPosition, Vector3.zero));
        }
        
        private bool NextCellIsEmpty()
        {
            if (_nextCell == null) 
                return IsEmpty;
            
            var nextCell = _nextCell.GetComponent<Cell>();
            return nextCell.IsEmpty;
        }

        private static IEnumerator AnimateMoveBalls([CanBeNull] GameObject childGo, Vector3 startPosition, Vector3 endPosition)
        {
            if (childGo == null) 
                yield break;
            
            var duration = GameVar.BallDropDuration;
            var elapsedTime = 0f;
            
            while (elapsedTime < duration)
            {
                if (childGo == null) 
                    yield break;
                
                elapsedTime += Time.deltaTime;
                var remainingTime = elapsedTime / duration;
                
                var ballPosition = Vector3.Lerp(startPosition, endPosition, remainingTime);
                childGo.transform.localPosition = ballPosition;
                
                yield return null;
            }
        }

        

        #endregion
    }
}
