using System.Collections.Generic;
using UnityEngine;

namespace Scrips.Events.Alerts
{
    [CreateAssetMenu(menuName = "Tower defense kit/Events/Alert event", order = 11)]
    public class AlertEvent : ScriptableObject
    {
        private readonly List<AlertEventListener> _eventListeners = new List<AlertEventListener>();

        public void AddListener(AlertEventListener listener)
        {
            _eventListeners.Add(listener);
        }

        public void RemoveListener(AlertEventListener listener)
        {
            _eventListeners.Remove(listener);
        }

        public void Invoke(string title, string message)
        {
            for (int i = _eventListeners.Count - 1; i >= 0; i--)
            {
                _eventListeners[i].Invoke(title, message);
            }
        }
    }
}