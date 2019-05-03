using System.Collections.Generic;
using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Events.Enemies
{
    [CreateAssetMenu(menuName = "Tower defense kit/Events/Enemy event", order = 10)]
    public class EnemyEvent : ScriptableObject
    {
        private readonly List<EnemyEventListener> _listeners = new List<EnemyEventListener>();

        public void AddListener(EnemyEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(EnemyEventListener listener)
        {
            _listeners.Remove(listener);
        }

        public void Invoke(EnemyInstance target)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].Invoke(target);
            }
        }
    }
}