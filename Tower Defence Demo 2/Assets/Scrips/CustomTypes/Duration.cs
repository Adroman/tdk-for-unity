using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.CustomTypes
{
    public abstract class Duration
    {
        public static Duration FromFixedTime(float time) => new FixedTimeDuration(time);

        public static Duration UntilDeath(EnemyInstance target) => new UntilDeathDuration(target);

        public abstract object UntilEnds();

        public abstract float ToFloat();

        private sealed class UntilDeathDuration : Duration
        {
            private readonly EnemyInstance _target;

            public UntilDeathDuration(EnemyInstance target)
            {
                _target = target;
            }

            public override object UntilEnds()
            {
                return new WaitUntil(() => _target != null && _target.Hitpoints > 0);
            }

            public override float ToFloat() => float.PositiveInfinity;
        }

        private sealed class FixedTimeDuration : Duration
        {
            private readonly float _time;

            public FixedTimeDuration(float time)
            {
                _time = time;
            }

            public override object UntilEnds()
            {
                return new WaitForSeconds(_time);
            }

            public override float ToFloat() => _time;
        }
    }
}