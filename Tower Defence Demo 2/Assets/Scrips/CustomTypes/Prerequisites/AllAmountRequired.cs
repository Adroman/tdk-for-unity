using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.TowerUpgrades;
using UnityEngine;

namespace Scrips.CustomTypes.Prerequisites
{
    [CreateAssetMenu(menuName = "Prerequisites/Int/All")]
    public class AllAmountRequired : AmountRequired
    {
        public override bool MeetsTheRequirement(
            ICollection<TowerUpgradeLineNode> objectsNeeded, 
            ICollection<TowerUpgradeLineNode> objectsHaving)
        {
            var data = ConvertToReadonlyCollection(objectsHaving);

            return objectsNeeded.All(neededObject => data.Contains(neededObject));
        }
    }
}