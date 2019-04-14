using System;
using System.Collections;
using Scrips.Waves;
using UnityEngine;
using UnityEngine.Events;

namespace Scrips.Events.Waves
{
    [Serializable]
    public class WaveUnityEvent : UnityEvent<Wave>
    {
    }

    public class WaveEventListener : MonoBehaviour
    {
        public WaveEvent Event;
        public WaveUnityEvent Response;

        private void OnEnable()
        {
            if (Event != null) Event.AddListener(this);
        }

        private void OnDisable()
        {
            if (Event != null) Event.RemoveListener(this);
        }

        public void Invoke(Wave target)
        {
            StartCoroutine(InvokeAfterCaller(target));
        }

        private IEnumerator InvokeAfterCaller(Wave target)
        {
            Response?.Invoke(target);
            yield break;
        }
    }
}