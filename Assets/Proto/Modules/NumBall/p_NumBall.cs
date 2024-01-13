using UnityEngine;

namespace Proto.Modules.NumBall
{
    public class p_NumBall : MonoBehaviour
    {
        #region Statements

        public int num;

        #endregion
        
        #region Events
        
        private void OnMouseDown()
        {
            Debug.Log("Down");
        }
        
        private void OnMouseEnter()
        {
            Debug.Log("Enter");
        }

        #endregion
    }
}
