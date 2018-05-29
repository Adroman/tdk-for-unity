using Scrips.CustomTypes;
using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.BuffData
{
    public class BurningBuffData : BaseBuffData
    {
        public static BurningBuffData FromTotalDamage(EnemyInstance target, float totalDamage, float duration)
        {
            return new BurningBuffData(target, Duration.FromFixedTime(duration), totalDamage / duration);
        }

        public static BurningBuffData FromDps(EnemyInstance target, float dps, Duration duration)
        {
            return new BurningBuffData(target, duration, dps);
        }

        private BurningBuffData(EnemyInstance target, Duration duration, float dps) : base(target, duration)
        {
            OnUpdate += (deltaTime) => Target.TakeCumulutativeDamage(dps, false, deltaTime);
        }
    }
}