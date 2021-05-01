using System;
using Scrips.Towers.Specials.ReduceArmor;
using UnityEngine;

namespace Scrips.Modifiers.Towers.TowerSpecials
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Tower/Specials/Reduce armor amount modifier")]
    public class ReduceArmorModifier : BaseTowerSpecialModifier<ReduceArmorComponent, int>
    {
        public override void AddToTowerSpecial(ReduceArmorComponent targetComponent)
        {
            targetComponent.Amount.AddModifier(this);
        }

        public override void RemoveFromTowerSpecial(ReduceArmorComponent targetComponent)
        {
            targetComponent.Amount.AddModifier(this);
        }
    }
}