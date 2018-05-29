using System.Collections.Generic;
using UnityEngine;

namespace Scrips.Events
{
    [CreateAssetMenu(menuName = "Events/Game event", order = 0)]
    public class GameEvent : ScriptableObject
    {
        private List<GameEventListener> _listeners = new List<GameEventListener>();

        public void AddListener(GameEventListener listener)
        {
            if (!_listeners.Contains(listener)) _listeners.Add(listener);
        }

        public void RemoveListener(GameEventListener listener)
        {
            _listeners.Remove(listener);
        }

        public void Invoke()
        {
            // we want to do this backwards, because if we remove the listener during enumeration,
            // we won't have deal with indexing
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnInvoked();
            }
        }
    }
}