using Scrips.CustomTypes;
using Scrips.EnemyData.Instances;
using Unity.UNetWeaver;

namespace Scrips.BuffData
{
    public class PoisonBuffData : BaseBuffData
    {
        private readonly float _dps;

        public override float Power => _dps;

        public static PoisonBuffData FromTotalDamage(EnemyInstance target, float totalDamage, float duration)
        {
            if (float.IsPositiveInfinity(duration))
            {
                Log.Error("Cannot calculate dps from total damage and infinite duration");
            }

            return new PoisonBuffData(target, duration, totalDamage / duration);
        }

        public static PoisonBuffData FromDps(EnemyInstance target, float dps, float duration)
        {
            return new PoisonBuffData(target, duration, dps);
        }

        private PoisonBuffData(EnemyInstance target, float duration, float dps) : base(target, duration)
        {
            _dps = dps;
        }

        protected override void ActivateEffect()
        {
            // do nothing
        }

        protected override void FinishEffect()
        {
            // do nothing
        }

        protected override void UpdateEffect(float deltaTime)
        {
            float trueAmount = _dps * deltaTime;
            
            Target.TakeDamage(trueAmount, true);
        }
    }
}