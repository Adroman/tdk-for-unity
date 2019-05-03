using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.CustomTypes.Prerequisites
{
    [CreateAssetMenu(menuName = "Tower defense kit/Prerequisites/Int/Any")]
    public class AnyAmountRequired : AmountRequired
    {
        public override bool MeetsTheRequirement(
            ICollection<TowerUpgradeNode> objectsNeeded,
            ICollection<TowerUpgradeNode> objectsHaving)
        {
            var data = ConvertToReadonlyCollection(objectsHaving);

            return objectsNeeded.Any(neededObject => data.Contains(neededObject));
        }
    }
}