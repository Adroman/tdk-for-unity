using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Towers.Specials
{
    public abstract class SpecialComponent : MonoBehaviour
    {
        public abstract SpecialType SpecialType { get; set; }

        public abstract void ApplySpecialEffect(EnemyInstance target);
    }
}