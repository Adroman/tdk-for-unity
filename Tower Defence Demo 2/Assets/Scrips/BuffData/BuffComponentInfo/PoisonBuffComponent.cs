using System;
using System.Collections.Generic;
using Scrips.Attributes;
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
                throw new InvalidOperationException("Cannot use total poison damage if we don't know how long it lasts.");

            if (InfiniteDuration)
                return PoisonBuffData.FromDps(target, PoisonAmount, float.PositiveInfinity);

            return TotalAmount
                ? PoisonBuffData.FromTotalDamage(target, PoisonAmount, Duration)
                : PoisonBuffData.FromDps(target, PoisonAmount, Duration);
        }
    }
}