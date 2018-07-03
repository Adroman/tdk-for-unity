using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.CustomTypes.Prerequisites;
using Scrips.Data;
using UnityEngine;

namespace Scrips.TowerUpgrades
{
    [CreateAssetMenu(menuName = "Tower Upgrade")]
    public class TowerUpgradeLineNode : ScriptableObject
    {
        public FloatIncrease MinAtkIncrease;
        public FloatIncrease MaxAtkIncrease;
        public FloatIncrease RangeIncrease;
        public FloatIncrease FiringSpeedIncrease;

        public List<TowerSpecialIncrease> SpecialIncreases;

        public List<IntCurrency> Price;

        public List<TowerUpgradeLineNode> Requirements = new List<TowerUpgradeLineNode>();
        public AmountRequired RequirementAmount;
        
        public List<TowerUpgradeLineNode> Exclusions = new List<TowerUpgradeLineNode>();
        public AmountRequired ExclusionAmount;
    }
}