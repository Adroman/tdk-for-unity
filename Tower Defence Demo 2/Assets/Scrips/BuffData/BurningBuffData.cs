using Scrips.CustomTypes;
using Scrips.EnemyData.Instances;
using Unity.UNetWeaver;
using UnityEngine;

namespace Scrips.BuffData
{
    public class BurningBuffData : BaseBuffData
    {
        private readonly float _dps;

        public override float Power => _dps;

        public static BurningBuffData FromTotalDamage(EnemyInstance target, float totalDamage, float duration)
        {
            if (float.IsPositiveInfinity(duration))
            {
                Log.Error("Cannot calculate dps from total damage and infinite duration");
            }

            return new BurningBuffData(target, duration, totalDamage / duration);
        }

        public static BurningBuffData FromDps(EnemyInstance target, float dps, float duration)
        {
            return new BurningBuffData(target, duration, dps);
        }

        private BurningBuffData(EnemyInstance target, float duration, float dps) : base(target, duration)
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