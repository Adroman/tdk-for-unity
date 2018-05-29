using UnityEngine;

namespace Scrips.CustomTypes.IncreaseType
{
    public abstract class BaseIncreaseType : ScriptableObject
    {
        public abstract int Increase(int fromValue, float byValue);

        public abstract double Increase(float fromValue, float byValue);
    }
}