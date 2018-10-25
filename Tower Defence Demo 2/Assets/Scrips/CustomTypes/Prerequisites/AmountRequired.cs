using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.CustomTypes.Prerequisites
{
    public abstract class AmountRequired : ScriptableObject
    {
        public abstract bool MeetsTheRequirement(
            ICollection<TowerUpgradeNode> objectsNeeded,
            ICollection<TowerUpgradeNode> objectsHaving);

        protected ICollection<TowerUpgradeNode> ConvertToReadonlyCollection(ICollection<TowerUpgradeNode> source)
        {
            if (source.Count > 20)
            {
                return new HashSet<TowerUpgradeNode>(source);
            }

            return new List<TowerUpgradeNode>(source.Distinct());
        }
    }
}