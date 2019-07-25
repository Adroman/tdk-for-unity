using Scrips.CustomTypes.IncreaseType;
using UnityEngine;

namespace Scrips.Towers.Specials.ReduceSpeed
{
    [CreateAssetMenu(menuName = "Tower defense kit/Tower Specials/Reduce speed")]
    public class ReduceSpeedType : SpecialType
    {
        public float Amount;

        public BaseIncreaseType AmountType;

        public float Duration;
        
        public override SpecialComponent GetOrCreateSpecialComponent(GameObject go)
        {
            var component = go.GetComponent<ReduceSpeedComponent>();
            if (component == null) component = go.AddComponent<ReduceSpeedComponent>();
            component.SpecialType = this;
            component.Amount = Amount;
            component.AmountType = AmountType;
            component.Duration = Duration;
            return component;
        }

        public override SpecialComponent GetSpecialComponent(GameObject go)
        {
            return go.GetComponent<ReduceSpeedComponent>();
        }

        public override string GetUiText()
        {
            switch (AmountType)
            {
                case MultiplicativeIncreaseType _:
                    return $"Reduces speed by {Amount * 100:2} percent for {Duration} second(s)";
                case AdditiveIncreaseType _:
                    return $"Reduces speed by {Amount} for {Duration} second(s)";
                case FixedIncreaseType _:
                    return $"Changes speed to {Amount} for {Duration} second(s)";
                default:
                    return "Does nothing";
            }
        }
    }
}