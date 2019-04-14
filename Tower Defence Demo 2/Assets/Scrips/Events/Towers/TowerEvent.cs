using System.Collections.Generic;
using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Events.Towers
{
    [CreateAssetMenu(menuName = "Events/Tower event")]
    public class TowerEvent : ScriptableObject
    {
        private readonly List<TowerEventListener> _listeners = new List<TowerEventListener>();

        public void AddListener(TowerEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(TowerEventListener listener)
        {
            _listeners.Remove(listener);
        }

        public void Invoke(TowerInstance target)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].Invoke(target);
            }
        }
    }
}