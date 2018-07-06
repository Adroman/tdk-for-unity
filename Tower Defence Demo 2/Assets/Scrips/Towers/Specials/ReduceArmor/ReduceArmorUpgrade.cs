using System;
using Scrips.CustomTypes.IncreaseType;
using UnityEngine;

namespace Scrips.Towers.Specials.ReduceArmor
{
    [CreateAssetMenu(menuName = "Tower Upgrades/Reduce Armor")]
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

            reduceArmor.Amount = PowerIncreaseType.Increase(reduceArmor.Amount, PowerByAmount);
            reduceArmor.Chance = Math.Min(
                reduceArmor.ChanceUpperLimit,
                ChanceIncreaseType.Increase(reduceArmor.Chance, ChanceByAmount));
        }
    }
}