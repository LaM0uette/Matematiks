using Proto.Modules.NumBall;
using UnityEngine;
using UnityEngine.Events;

namespace Obvious.Soap
{
    [AddComponentMenu("Soap/EventListeners/EventListenerP_NumBall")]
    public class EventListenerP_NumBall : EventListenerGeneric<p_NumBall>
    {
        [SerializeField] private EventResponse[] _eventResponses = null;
        protected override EventResponse<p_NumBall>[] EventResponses => _eventResponses;

        [System.Serializable]
        public class EventResponse : EventResponse<p_NumBall>
        {
            [SerializeField] private ScriptableEventP_NumBall _scriptableEvent = null;
            public override ScriptableEvent<p_NumBall> ScriptableEvent => _scriptableEvent;

            [SerializeField] private P_NumBallUnityEvent _response = null;
            public override UnityEvent<p_NumBall> Response => _response;
        }

        [System.Serializable]
        public class P_NumBallUnityEvent : UnityEvent<p_NumBall>
        {
            
        }
    }
}