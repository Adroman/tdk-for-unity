﻿using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.CustomTypes.Prerequisites
{
    [CreateAssetMenu(menuName = "Tower defense kit/Prerequisites/Int/Set amount")]
    public class SetAmountRequired : AmountRequired
    {
        public int RequiredAmount;

        public override bool MeetsTheRequirement(
            ICollection<TowerUpgradeNode> objectsNeeded,
            ICollection<TowerUpgradeNode> objectsHaving)
        {
            var data = ConvertToReadonlyCollection(objectsHaving);

            int foundAmount = 0;
            
            foreach (var neededObject in objectsNeeded)
            {
                if (data.Contains(neededObject))
                {
                    foundAmount++;
                    if (foundAmount >= RequiredAmount) return true;
                    data.Remove(neededObject);
                }
            }

            return false;
        }
    }
}