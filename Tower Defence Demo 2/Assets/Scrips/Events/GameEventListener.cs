using UnityEngine;
using UnityEngine.Events;

namespace Scrips.Events
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;
        public UnityEvent Response;

        private void OnEnable()
        {
            if (Event == null) return;
            Event.AddListener(this);
        }

        private void OnDisable()
        {
            if (Event == null) return;
            Event.RemoveListener(this);
        }

        public void OnInvoked()
        {
            Invoke(nameof(InvokeAfterCaller), 0);
        }

        private void InvokeAfterCaller()
        {
            Response.Invoke();
        }
    }
}