using Scrips.BuffData;
using Scrips.BuffData.BuffComponentInfo;
using Scrips.CustomTypes.IncreaseType;
using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Towers.Specials.ReduceSpeed
{
    public class ReduceSpeedComponent : SpecialComponent
    {
        public float Amount;

        public BaseIncreaseType AmountType;

        public float Duration;

        public override SpecialType SpecialType { get; set; }

        public override void ApplySpecialEffect(EnemyInstance target)
        {
            var slownessBuff = new SlownessBuffData(target, Amount, AmountType, Duration);
            slownessBuff.Activate();
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