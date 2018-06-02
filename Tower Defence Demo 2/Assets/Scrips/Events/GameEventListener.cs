using System.Collections;
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
            StartCoroutine(InvokeAfterCaller());
        }

        private IEnumerator InvokeAfterCaller()
        {
            Response.Invoke();
            yield break;
        }
    }
}