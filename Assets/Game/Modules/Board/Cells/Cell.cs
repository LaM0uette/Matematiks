using System.Collections;
using Game.Modules.Board.Balls;
using Game.Modules.Utils;
using JetBrains.Annotations;
using Obvious.Soap;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

namespace Game.Modules.Board.Cells
{
    public class Cell : MonoBehaviour
    {
        #region Statements

        [ShowInInspector, ReadOnly] public bool IsEmpty { get; set; }
        
        [SerializeField, CanBeNull] private GameObject _nextCell;
        [SerializeField] private BoolVariable _isInAnimationVariable;

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

        private IEnumerator AnimateMoveBalls([CanBeNull] GameObject childGo, Vector3 startPosition, Vector3 endPosition)
        {
            if (childGo == null) 
                yield break;
            
            _isInAnimationVariable.Value = true;
            
            var duration = GameVar.BallDropDuration;
            var elapsedTime = 0f;
            
            while (elapsedTime < duration)
            {
                if (childGo.IsDestroyed())
                {
                    _isInAnimationVariable.Value = false;
                    yield break;
                }
                
                elapsedTime += Time.deltaTime;
                var remainingTime = elapsedTime / duration;
                
                var ballPosition = Vector3.Lerp(startPosition, endPosition, remainingTime);
                childGo.transform.localPosition = ballPosition;
                
                yield return null;
            }
            
            _isInAnimationVariable.Value = false;
        }

        

        #endregion
    }
}
