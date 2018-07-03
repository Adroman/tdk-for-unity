using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.TowerUpgrades;
using UnityEngine;

namespace Scrips.CustomTypes.Prerequisites
{
    public abstract class AmountRequired : ScriptableObject
    {
        public abstract bool MeetsTheRequirement(
            ICollection<TowerUpgradeLineNode> objectsNeeded,
            ICollection<TowerUpgradeLineNode> objectsHaving);

        protected ICollection<TowerUpgradeLineNode> ConvertToReadonlyCollection(ICollection<TowerUpgradeLineNode> source)
        {
            if (source.Count > 20)
            {
                return new HashSet<TowerUpgradeLineNode>(source);
            }

            return new List<TowerUpgradeLineNode>(source.Distinct());
        }
    }
}