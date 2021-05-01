using System;
using Scrips.CustomTypes.IncreaseType;
using UnityEngine;

namespace Scrips.Towers.Specials.ReduceArmor
{
    [CreateAssetMenu(menuName = "Tower defense kit/Tower Upgrades/Reduce Armor")]
    public class ReduceArmorUpgrade : SpecialUpgrade
    {
        public BaseIncreaseType ChanceIncreaseType;
        public float ChanceByAmount;

        public BaseIncreaseType PowerIncreaseType;
        public float PowerByAmount;

        public override void Upgrade(SpecialComponent component)
        {
            var reduceArmor = component as ReduceArmorComponent;

            if (reduceArmor == null)
            {
                Debug.LogWarning("Invalid Component");
                return;
            }

            reduceArmor.Amount.Value = PowerIncreaseType.Increase(reduceArmor.Amount.Value, PowerByAmount);

            float newChance = ChanceIncreaseType.Increase(reduceArmor.Chance, ChanceByAmount);

            if (reduceArmor.ChanceHasUpperLimit)
            {
                reduceArmor.Chance = Math.Min(
                    newChance,
                    reduceArmor.ChanceUpperLimit
                );
            }
            else
            {
                reduceArmor.Chance = newChance;
            }
        }
    }
}