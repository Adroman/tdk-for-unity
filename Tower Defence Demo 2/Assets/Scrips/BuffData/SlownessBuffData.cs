using System.Collections;
using System.Linq;
using Scrips.CustomTypes;
using Scrips.EnemyData.Instances;
using Scrips.Extensions;
using UnityEngine;

namespace Scrips.BuffData
{
    public class SlownessBuffData : BaseBuffData
    {
        public Amount Amount { get; }

        public SlownessBuffData(EnemyInstance target, Duration duration, Amount amount ) : base(target, duration)
        {
            Amount = amount;

            OnStartEffect += () =>
            {
                Target.CalculateCurrentSpeed();
            };

            OnStopEffect += () =>
            {
                Target.CalculateCurrentSpeed();
            };
        }

        protected override void TryAddDebuff()
        {
            float thisSpeed = Target.InitialSpeed.Substract(Amount);
            foreach (var slowness in Target.ActiveDebuffs.OfType<SlownessBuffData>().ToList())
            {
                float activeSpeed = Target.InitialSpeed.Substract(slowness.Amount);
                if (activeSpeed <= thisSpeed && slowness.TimeLeft >= TimeLeft)
                {
                    // one of active buffs is more effective, so we don't add this
                    FailedToStart = true;
                    return;
                }

                if (activeSpeed >= thisSpeed && slowness.TimeLeft <= TimeLeft)
                {
                    // this buff is better than the active one, so we stop the active one
                    Target.StopCoroutine(slowness.ActiveCoroutine);
                    slowness.StopEffect();
                }
            }

            Target.ActiveDebuffs.Add(this);
            FailedToStart = false;
        }
    }
}