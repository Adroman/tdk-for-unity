using Scrips.CustomTypes;
using Scrips.EnemyData.Instances;
namespace Scrips.BuffData
{
    public class PoisonBuffData : BaseBuffData
    {
        public static PoisonBuffData FromTotalDamage(EnemyInstance target, float totalDamage, float duration)
        {
            return new PoisonBuffData(target, Duration.FromFixedTime(duration), totalDamage / duration);
        }

        public static PoisonBuffData FromDps(EnemyInstance target, float dps, Duration duration)
        {
            return new PoisonBuffData(target, duration, dps);
        }

        private PoisonBuffData(EnemyInstance target, Duration duration, float dps) : base(target, duration)
        {
            OnUpdate += (deltaTime) => Target.TakeCumulutativeDamage(dps, true, deltaTime);
        }
    }
}