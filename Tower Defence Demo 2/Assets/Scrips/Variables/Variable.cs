using System.Collections.Generic;
using JetBrains.Annotations;
using Scrips.Events;
using Scrips.Variables.Watchers;
using UnityEngine;

namespace Scrips.Variables
{
    [PublicAPI]
    public class Variable<T> : ScriptableObject
    {
        [SerializeField]
        [HideInInspector]
        private T _value;

        [Multiline]
        public string Description;

        [SerializeField]
        [HideInInspector]
        private List<IVariableWatcher<T>> _watchers = new List<IVariableWatcher<T>>();

        public Sprite Icon;

        public Color IconColor;

        public GameEvent OnValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                if (OnValueChanged != null) OnValueChanged.Invoke();
                foreach (var watcher in _watchers)
                {
                    watcher.Raise(_value);
                }
            }
        }

        public void AddWatcher(IVariableWatcher<T> watcher)
        {
            _watchers.Add(watcher);
        }

        public void RemoveWatcher(IVariableWatcher<T> watcher)
        {
            _watchers.Remove(watcher);
        }
    }
}