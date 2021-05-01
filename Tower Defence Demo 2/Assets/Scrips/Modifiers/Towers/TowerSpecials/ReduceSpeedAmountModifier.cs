using Scrips.Towers.Specials.ReduceSpeed;
using UnityEngine;

namespace Scrips.Modifiers.Towers.TowerSpecials
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Tower/Specials/Reduce speed amount modifier")]
    public class ReduceSpeedAmountModifier : BaseTowerSpecialModifier<ReduceSpeedComponent, int>
    {
        public override void AddToTowerSpecial(ReduceSpeedComponent targetComponent)
        {
            targetComponent.Amount.AddModifier(this);
        }

        public override void RemoveFromTowerSpecial(ReduceSpeedComponent targetComponent)
        {
            targetComponent.Amount.RemoveModifier(this);
        }
    }
}