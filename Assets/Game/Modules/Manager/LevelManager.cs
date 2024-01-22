using Game.Modules.GameMode;
using Obvious.Soap;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Modules.Manager
{
    public class LevelManager : MonoBehaviour
    {
        #region Statements
        
        [Space, Title("Board")]
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _maxDistanceBetweenTwoBalls = 1.3f;
        
        [Space, Title("Soap")]
        [SerializeField] private BoolVariable _mouseIsDownVariable;
        [SerializeField] private BoolVariable _isInAnimationVariable;
        [SerializeField] private IntVariable _scoreVariable;
        [SerializeField] private ScriptableEventBall _ballSelectedEvent;
        [SerializeField] private ScriptableListBall _ballsSelected;

        private IGameMode _gameMode;

        private void Start()
        {
            _gameMode = gameObject.AddComponent<StandardGameMode>();
            _gameMode.Start();
        }

        #endregion

        #region Events

        private void Update()
        {
            _gameMode.Update();
        }

        #endregion
    }
}