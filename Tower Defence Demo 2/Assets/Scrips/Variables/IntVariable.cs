using Scrips.Events;
using UnityEngine;

namespace Scrips.Variables
{
    [CreateAssetMenu(menuName = "Variables/Int variable")]
    public class IntVariable : ScriptableObject
    {
        [SerializeField]
        [HideInInspector]
        private int _value;

        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnValueChanged?.Invoke();
            }
        }

        public GameEvent OnValueChanged;
    }
}