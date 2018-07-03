using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.TowerUpgrades;
using UnityEngine;

namespace Scrips.CustomTypes.Prerequisites
{
    [CreateAssetMenu(menuName = "Prerequisites/Int/Any")]
    public class AnyAmountRequired : AmountRequired
    {
        public override bool MeetsTheRequirement(
            ICollection<TowerUpgradeLineNode> objectsNeeded,
            ICollection<TowerUpgradeLineNode> objectsHaving)
        {
            var data = ConvertToReadonlyCollection(objectsHaving);

            return objectsNeeded.Any(neededObject => data.Contains(neededObject));
        }
    }
}