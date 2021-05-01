using System;
using Scrips.Modifiers.Stats;
using UnityEngine;

namespace Scrips.Towers.Specials.ReduceArmor
{
    [CreateAssetMenu(menuName = "Tower defense kit/Tower Specials/Reduce Armor")]
    public class ReduceArmorType : SpecialType
    {
        public float Amount;

        [Range(0, 1)]
        public float Chance;

        public bool ChanceHasUpperLimit;
        [Range(0, 1)]
        public float ChanceUpperLimit;

        public override SpecialComponent GetOrCreateSpecialComponent(GameObject go)
        {
            var component = go.GetComponent<ReduceArmorComponent>();
            if (component == null) component = go.AddComponent<ReduceArmorComponent>();
            component.SpecialType = this;
            component.Amount = new FloatModifiableStat {Value = Amount};
            component.Chance = Chance;
            component.ChanceUpperLimit = ChanceUpperLimit;
            component.ChanceHasUpperLimit = ChanceHasUpperLimit;
            return component;
        }

        public override SpecialComponent GetSpecialComponent(GameObject go)
        {
            return go.GetComponent<ReduceArmorComponent>();
        }

        public override string GetUiText()
        {
            return Math.Abs(Chance - 1f) < 0.001f
                ? $"Reduces {Amount} of armor per hit"
                : $"{Chance * 100:0}% chance to reduce armor by {Amount}";
        }
    }
}