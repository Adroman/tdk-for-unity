using System.Collections.Generic;
using Scrips.Waves;
using UnityEngine;

namespace Scrips.Events.Waves
{
    [CreateAssetMenu(menuName = "Events/Wave event")]
    public class WaveEvent : ScriptableObject
    {
        private readonly List<WaveEventListener> _listeners = new List<WaveEventListener>();

        public void AddListener(WaveEventListener listener)
        {
            _listeners.Add(listener);
        }

        public void RemoveListener(WaveEventListener listener)
        {
            _listeners.Remove(listener);
        }

        public void Invoke(Wave target)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].Invoke(target);
            }
        }
    }
}