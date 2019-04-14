using System;
using System.Collections;
using Scrips.EnemyData.Instances;
using UnityEngine;
using UnityEngine.Events;

namespace Scrips.Events.Enemies
{
    [Serializable]
    public class EnemyUnityEvent : UnityEvent<EnemyInstance>
    {
    }

    public class EnemyEventListener : MonoBehaviour
    {
        public EnemyEvent Event;
        public EnemyUnityEvent Response;

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

        public void Invoke(EnemyInstance target)
        {
            StartCoroutine(InvokeAfterCaller(target));
        }

        private IEnumerator InvokeAfterCaller(EnemyInstance target)
        {
            Response?.Invoke(target);
            yield break;
        }
    }
}