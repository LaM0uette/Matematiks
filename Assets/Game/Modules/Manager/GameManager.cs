using System.Collections.Generic;
using Game.Modules.Board.Balls;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.Manager
{
    public class GameManager : MonoBehaviour
    {
        #region Statements

        public static GameManager Instance { get; private set; }
        
        [SerializeField] private List<BallNumber> _ballNumbers = new();
        [ShowInInspector, ReadOnly] public BallNumber[] BallNumbers;

        private void Awake()
        {
            Instance ??= this;

            BallNumbers = _ballNumbers.ToArray();
        }

        #endregion

        #region Functions

        public void UpdateBallNumbers(int ballNumber)
        {
            if (BallNumbers[ballNumber - 1].IsLocked)
            {
                BallNumbers[ballNumber - 1].IsLocked = false;
            }

            if (ballNumber > 2)
            {
                BallNumbers[ballNumber - 1].Weight += ballNumber / 20f;
            }
        }

        #endregion
    }
}
