using System.Collections.Generic;
using Scrips.CustomTypes.Prerequisites;
using Scrips.Data;
using Scrips.Towers.Specials;
using UnityEngine;

namespace Scrips.Towers.BaseData
{
    [CreateAssetMenu(menuName = "Tower Upgrades/Basic")]
    public class TowerUpgradeLineNode : ScriptableObject
    {
        public FloatIncrease MinAtkIncrease;
        public FloatIncrease MaxAtkIncrease;
        public FloatIncrease RangeIncrease;
        public FloatIncrease FiringSpeedIncrease;

        public List<SpecialUpgrade> SpecialIncreases;
        public List<SpecialType> SpecialUnlocks;

        public List<IntCurrency> Price;

        public List<TowerUpgradeLineNode> Requirements = new List<TowerUpgradeLineNode>();
        public AmountRequired RequirementAmount;

        public List<TowerUpgradeLineNode> Exclusions = new List<TowerUpgradeLineNode>();
        public AmountRequired ExclusionAmount;

        public GameObject Model;
    }
}