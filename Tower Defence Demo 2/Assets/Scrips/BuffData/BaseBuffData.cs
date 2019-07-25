using System;
using System.Collections;
using System.Linq;
using Scrips.CustomTypes;
using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.BuffData
{
    public abstract class BaseBuffData
    {
        public float TimeLeft { get; private set; }
        public bool Ended => TimeLeft > 0;

        public abstract float Power { get; }

        public EnemyInstance Target { get; }

        protected BaseBuffData(EnemyInstance target, float duration)
        {
            Target = target;
            TimeLeft = duration;
        }

        public void Update(float deltaTime)
        {
            float timeLeft = TimeLeft - deltaTime;

            if (timeLeft < 0)    // buff has ended
            {
                UpdateEffect(TimeLeft);    // use the remaining time
                FinishEffect();
            }
            else
            {
                UpdateEffect(deltaTime);
            }

            TimeLeft = timeLeft;
        }

        public void Activate()
        {
            if (TryAddBuff())
                ActivateEffect();
        }

        public void End()
        {
            Target.ActiveDebuffs.Remove(this);
            FinishEffect();
        }

        protected abstract void ActivateEffect();

        protected abstract void FinishEffect();

        protected abstract void UpdateEffect(float deltaTime);

        private bool TryAddBuff()
        {
            foreach (var debuff in Target.ActiveDebuffs.Where(b => b.GetType() == GetType()))
            {
                if (debuff.Power > Power && debuff.TimeLeft > TimeLeft)
                {
                    return false;    // there is more powerful buff, no need to add this one
                }
            }

            Target.ActiveDebuffs.Add(this);
            return true;
        }
    }
}