using Game.Modules.Board.Balls;
using UnityEngine;
using UnityEngine.Events;

namespace Obvious.Soap
{
    [AddComponentMenu("Soap/EventListeners/EventListenerBall")]
    public class EventListenerBall : EventListenerGeneric<Ball>
    {
        [SerializeField] private EventResponse[] _eventResponses;
        protected override EventResponse<Ball>[] EventResponses => _eventResponses;

        [System.Serializable]
        public class EventResponse : EventResponse<Ball>
        {
            [SerializeField] private ScriptableEventBall _scriptableEvent = null;
            public override ScriptableEvent<Ball> ScriptableEvent => _scriptableEvent;

            [SerializeField] private BallUnityEvent _response;
            public override UnityEvent<Ball> Response => _response;
        }

        [System.Serializable]
        public class BallUnityEvent : UnityEvent<Ball>
        {
            
        }
    }
}