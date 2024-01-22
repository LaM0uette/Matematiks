using Game.Modules.Board.Balls;
using UnityEngine;
using UnityEngine.Events;

namespace Obvious.Soap
{
    [AddComponentMenu("Soap/EventListeners/EventListenerWeightedBall")]
    public class EventListenerWeightedBall : EventListenerGeneric<WeightedBall>
    {
        [SerializeField] private EventResponse[] _eventResponses = null;
        protected override EventResponse<WeightedBall>[] EventResponses => _eventResponses;

        [System.Serializable]
        public class EventResponse : EventResponse<WeightedBall>
        {
            [SerializeField] private ScriptableEventWeightedBall _scriptableEvent = null;
            public override ScriptableEvent<WeightedBall> ScriptableEvent => _scriptableEvent;

            [SerializeField] private WeightedBallUnityEvent _response = null;
            public override UnityEvent<WeightedBall> Response => _response;
        }

        [System.Serializable]
        public class WeightedBallUnityEvent : UnityEvent<WeightedBall>
        {
            
        }
    }
}