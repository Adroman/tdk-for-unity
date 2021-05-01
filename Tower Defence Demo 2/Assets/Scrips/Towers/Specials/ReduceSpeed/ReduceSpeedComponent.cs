using Scrips.BuffData;
using Scrips.BuffData.BuffComponentInfo;
using Scrips.CustomTypes.IncreaseType;
using Scrips.EnemyData.Instances;
using Scrips.Modifiers.Stats;
using UnityEngine;

namespace Scrips.Towers.Specials.ReduceSpeed
{
    public class ReduceSpeedComponent : SpecialComponent
    {
        public FloatModifiableStat Amount;

        public BaseIncreaseType AmountType;

        public FloatModifiableStat Duration;

        public override SpecialType SpecialType { get; set; }

        public override void ApplySpecialEffect(EnemyInstance target)
        {
            var slownessBuff = new SlownessBuffData(target, Amount.Value, AmountType, Duration.Value);
            slownessBuff.Activate();
        }

        public override string GetUiText()
        {
            switch (AmountType)
            {
                case MultiplicativeIncreaseType _:
                    return $"Reduces speed by {Amount.Value * 100:P2} for {Duration.Value} second(s)";
                case AdditiveIncreaseType _:
                    return $"Reduces speed by {Amount.Value} for {Duration.Value} second(s)";
                case FixedIncreaseType _:
                    return $"Changes speed to {Amount.Value} for {Duration.Value} second(s)";
                default:
                    return "Does nothing";
            }
        }

        public override void CopyDataToTargetComponent(SpecialComponent targetComponent)
        {
            var target = targetComponent as ReduceSpeedComponent;
            if (target == null)
            {
                Debug.LogError("Invalid component");
                return;
            }

            target.Amount = Amount;
            target.AmountType = AmountType;
            target.Duration = Duration;
        }
    }
}