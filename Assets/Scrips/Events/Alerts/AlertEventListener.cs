using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Scrips.Events.Alerts
{
    [Serializable]
    public class AlertUnityEvent : UnityEvent<string, string>
    {
    }

    public class AlertEventListener : MonoBehaviour
    {
        public AlertEvent Event;
        public AlertUnityEvent Response;
        
        private void OnEnable()
        {
            if (Event != null) 
                Event.AddListener(this);
        }

        private void OnDisable()
        {
            if (Event != null)
                Event.RemoveListener(this);
        }

        public void Invoke(string title, string message)
        {
            StartCoroutine(InvokeAfterCaller(title, message));
        }

        private IEnumerator InvokeAfterCaller(string title, string message)
        {
            Response?.Invoke(title, message);
            yield break;
        }
    }
}