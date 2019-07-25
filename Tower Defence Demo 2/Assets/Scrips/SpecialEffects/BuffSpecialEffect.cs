using Scrips.BuffData.BuffComponentInfo;
using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.SpecialEffects
{
    public class BuffSpecialEffect : BaseSpecialEffect
    {
        public BaseBuffComponent Buff;

        public override void ApplySpecialEffect(EnemyInstance target)
        {
            if (Buff == null)
            {
                Debug.LogError("WROOOOONG");
                return;
            }

            var buff = Buff.CreateBuff(target);
            buff.Activate();
        }
    }
}