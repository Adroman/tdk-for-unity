using Scrips.CustomTypes;
using Scrips.CustomTypes.IncreaseType;
using Scrips.EnemyData.Instances;

namespace Scrips.BuffData.BuffComponentInfo
{
    public class SlownessBuffComponent : BaseBuffComponent
    {
        public float SpeedAmount;

        public BaseIncreaseType AmountType;

        public override BaseBuffData CreateBuff(EnemyInstance target)
        {
            return new SlownessBuffData(
                target,
                SpeedAmount,
                AmountType,
                InfiniteDuration ? float.PositiveInfinity : Duration);
        }
    }
}