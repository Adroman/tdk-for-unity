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

        public bool ChanceHasUpperLimit;
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

        public override string GetUiText()
        {
            return Math.Abs(Chance - 1f) < 0.001f ? $"Reduces {Amount} of armor per hit" : $"{Chance * 100:0}% chance for reducing armor by {Amount}";
        }

        public override void CopyDataToTargetComponent(SpecialComponent targetComponent)
        {
            var target = targetComponent as ReduceArmorComponent;
            if (target == null)
            {
                Debug.LogError("Invalid component");
                return;
            }

            target.Amount = Amount;
            target.Chance = Chance;
            target.ChanceHasUpperLimit = ChanceHasUpperLimit;
            target.ChanceUpperLimit = ChanceUpperLimit;
        }
    }
}