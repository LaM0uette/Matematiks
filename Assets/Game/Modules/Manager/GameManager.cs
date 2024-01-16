using System.Collections.Generic;
using Game.Modules.Board.Balls;
using UnityEngine;

namespace Game.Modules.Manager
{
    public class GameManager : MonoBehaviour
    {
        #region Statements

        public static GameManager Instance { get; private set; }
        
        [SerializeField] private List<BallNumber> _ballNumbers = new();
        public BallNumber[] BallNumbers => _ballNumbers.ToArray();

        private void Awake()
        {
            Instance ??= this;
        }

        #endregion
    }
}
