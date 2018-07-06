using System;
using Scrips.EnemyData.Instances;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scrips.Towers.Specials.ReduceArmor
{
    public class ReduceArmorComponent : SpecialComponent
    {
        public float Amount;

        [Range(0, 1)]
        public float Chance;

        [Range(0, 1)]
        public float ChanceUpperLimit;

        public override SpecialType SpecialType { get; set; }

        public override void ApplySpecialEffect(EnemyInstance target)
        {
            float random = Random.Range(0f, 1f);

            if (random <= Chance)
            {
                target.Armor = Math.Max(0, target.Armor - Amount);
            }
        }
    }
}