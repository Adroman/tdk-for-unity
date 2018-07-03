using UnityEngine;

namespace Scrips.CustomTypes.IncreaseType
{
    public abstract class BaseIncreaseType : ScriptableObject
    {
        [SerializeField] [HideInInspector] private string _serializationId;

        protected BaseIncreaseType()
        {
            _serializationId = GetType().FullName;
        }

        public abstract int Increase(int fromValue, float byValue);

        public abstract float Increase(float fromValue, float byValue);
    }
}