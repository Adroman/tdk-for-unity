using Scrips.CustomTypes.IncreaseType;
using UnityEngine;

namespace Scrips.Modifiers
{
    public abstract class BaseModifier : ScriptableObject
    {
        [Min(1)]
        public int Level;

        public float Amount;

        public BaseIncreaseType IncreaseType;
    }




}