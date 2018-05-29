using Scrips.CustomTypes;
using Scrips.EnemyData.Instances;

namespace Scrips.BuffData.BuffComponentInfo
{
    public class SlownessBuffComponent : BaseBuffComponent
    {
        public float SpeedAmount;

        public bool PercentageAmount;

        public override BaseBuffData CreateBuff(EnemyInstance target)
        {
            return new SlownessBuffData(
                target,
                InfiniteDuration ? CustomTypes.Duration.UntilDeath(target) : CustomTypes.Duration.FromFixedTime(Duration),
                PercentageAmount ? Amount.Percenatage(SpeedAmount) : Amount.Flat(SpeedAmount));
        }
    }
}