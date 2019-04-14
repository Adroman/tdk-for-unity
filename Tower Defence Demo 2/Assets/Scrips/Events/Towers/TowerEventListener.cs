using System;
using System.Collections;
using Scrips.Towers.BaseData;
using UnityEngine;
using UnityEngine.Events;

namespace Scrips.Events.Towers
{
    [Serializable]
    public class TowerUnityEvent : UnityEvent<TowerInstance>
    {
    }

    public class TowerEventListener : MonoBehaviour
    {
        public TowerEvent Event;
        public TowerUnityEvent Response;

        private void OnEnable()
        {
            if (Event != null) Event.AddListener(this);
        }

        private void OnDisable()
        {
            if (Event != null) Event.RemoveListener(this);
        }

        public void Invoke(TowerInstance target)
        {
            StartCoroutine(InvokeAfterCaller(target));
        }

        private IEnumerator InvokeAfterCaller(TowerInstance target)
        {
            Response?.Invoke(target);
            yield break;
        }
    }
}