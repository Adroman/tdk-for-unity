using Scrips.BuffData;
using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Towers.Specials.PoisonDamage
{
    public class PoisonComponent : SpecialComponent
    {
        public float Dps;

        public float Duration;

        public override SpecialType SpecialType { get; set; }

        public override void ApplySpecialEffect(EnemyInstance target)
        {
            var poisonBuff = PoisonBuffData.FromDps(target, Dps, Duration);
            poisonBuff.Activate();
        }

        public override string GetUiText() => $"Deals {Dps} damage per second over {Duration} seconds";

        public override void CopyDataToTargetComponent(SpecialComponent targetComponent)
        {
            var target = targetComponent as PoisonComponent;
            if (target == null)
            {
                Debug.LogError("Invalid component");
                return;
            }

            target.Dps = Dps;
            target.Duration = Duration;
        }
    }
}