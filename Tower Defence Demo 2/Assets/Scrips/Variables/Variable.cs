using JetBrains.Annotations;
using Scrips.Events;
using UnityEngine;

namespace Scrips.Variables
{
    [PublicAPI]
    public class Variable<T> : ScriptableObject
    {
        [SerializeField]
        [HideInInspector]
        private T _value;

        public Sprite Icon;

        public Color IconColor;

        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                if (OnValueChanged != null) OnValueChanged.Invoke();
            }
        }

        public GameEvent OnValueChanged;
    }
}