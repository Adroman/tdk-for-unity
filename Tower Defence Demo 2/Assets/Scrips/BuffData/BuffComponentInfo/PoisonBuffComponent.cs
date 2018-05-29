using System;
using Scrips.EnemyData.Instances;

namespace Scrips.BuffData.BuffComponentInfo
{
    public class PoisonBuffComponent : BaseBuffComponent
    {
        public float PoisonAmount;

        public bool PercentageAmount;

        public bool TotalAmount;

        public override BaseBuffData CreateBuff(EnemyInstance target)
        {
            if (TotalAmount && InfiniteDuration)
                throw new InvalidOperationException("Cannot use total posion damage if we don't know how long it lasts.");

            if (InfiniteDuration)
                return PoisonBuffData.FromDps(target, PoisonAmount, CustomTypes.Duration.UntilDeath(target));

            return TotalAmount
                ? PoisonBuffData.FromTotalDamage(target, PoisonAmount, Duration)
                : PoisonBuffData.FromDps(target, PoisonAmount, CustomTypes.Duration.FromFixedTime(Duration));
        }
    }
}