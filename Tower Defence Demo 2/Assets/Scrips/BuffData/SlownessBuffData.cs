using System.Collections;
using System.Linq;
using Scrips.CustomTypes;
using Scrips.CustomTypes.IncreaseType;
using Scrips.EnemyData.Instances;
using Scrips.Extensions;
using UnityEngine;

namespace Scrips.BuffData
{
    public class SlownessBuffData : StatChangeBuffData
    {
        public SlownessBuffData(EnemyInstance target, float amount, BaseIncreaseType amountType, float duration)
            : base(target, amount, amountType, duration)
        {
        }

        protected override float StatToChange
        {
            get => Target.InitialSpeed.Value;
            set => Target.Speed = value;
        }
    }
}