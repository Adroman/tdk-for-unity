using UnityEngine;

namespace Scrips.Towers.Specials.ReduceArmor
{
    [CreateAssetMenu(menuName = "Tower Specials/Reduce Armor")]
    public class ReduceArmorType : SpecialType
    {
        public float Amount;

        [Range(0, 1)]
        public float Chance;

        [Range(0, 1)]
        public float ChanceUpperLimit;

        public override SpecialComponent GetOrCreateSpecialComponent(GameObject go)
        {
            var component = go.GetComponent<ReduceArmorComponent>();
            if (component == null) component = go.AddComponent<ReduceArmorComponent>();
            component.SpecialType = this;
            component.Amount = Amount;
            component.Chance = Chance;
            component.ChanceUpperLimit = ChanceUpperLimit;
            return component;
        }

        public override SpecialComponent GetSpecialComponent(GameObject go)
        {
            return go.GetComponent<ReduceArmorComponent>();
        }
    }
}