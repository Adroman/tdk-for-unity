using Scrips.CustomTypes.IncreaseType;
using UnityEngine;

namespace Scrips.Modifiers
{
    public abstract class BaseModifier : ScriptableObject
    {
        public float Amount;

        public BaseIncreaseType IncreaseType;
    }



    public abstract class BaseEnemyModifer : BaseModifier
    {
    }
}