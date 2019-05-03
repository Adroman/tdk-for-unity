using System.Collections.Generic;
using Scrips.CustomTypes.Prerequisites;
using Scrips.Data;
using Scrips.Towers.Specials;
using UnityEngine;

namespace Scrips.Towers.BaseData
{
    [CreateAssetMenu(menuName = "Tower defense kit/Tower Upgrades/Basic")]
    public class TowerUpgradeNode : ScriptableObject
    {
        public string NewName;

        public FloatIncrease MinAtkIncrease;
        public FloatIncrease MaxAtkIncrease;
        public FloatIncrease RangeIncrease;
        public FloatIncrease FiringSpeedIncrease;

        public List<SpecialUpgrade> SpecialIncreases;
        public List<SpecialType> SpecialUnlocks;

        public List<IntCurrency> Price;

        public List<TowerUpgradeNode> Requirements = new List<TowerUpgradeNode>();
        public AmountRequired RequirementAmount;

        public List<TowerUpgradeNode> Exclusions = new List<TowerUpgradeNode>();
        public AmountRequired ExclusionAmount;

        public GameObject Model;
    }
}