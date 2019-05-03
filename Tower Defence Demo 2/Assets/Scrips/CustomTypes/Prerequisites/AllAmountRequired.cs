using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.CustomTypes.Prerequisites
{
    [CreateAssetMenu(menuName = "Tower defense kit/Prerequisites/Int/All")]
    public class AllAmountRequired : AmountRequired
    {
        public override bool MeetsTheRequirement(
            ICollection<TowerUpgradeNode> objectsNeeded, 
            ICollection<TowerUpgradeNode> objectsHaving)
        {
            var data = ConvertToReadonlyCollection(objectsHaving);

            return objectsNeeded.All(neededObject => data.Contains(neededObject));
        }
    }
}