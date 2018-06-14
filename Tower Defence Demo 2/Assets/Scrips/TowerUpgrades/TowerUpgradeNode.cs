using System;
using System.Collections.Generic;
using Scrips.Data;
using UnityEngine;

namespace Scrips.TowerUpgrades
{
    [Serializable]
    public class TowerUpgradeLineNode
    {
        public FloatIncrease MinAtkIncrease;
        public FloatIncrease MaxAtkIncrease;
        public FloatIncrease RangeIncrease;
        public FloatIncrease FiringSpeedIncrease;

        public List<TowerSpecialIncrease> SpecialIncreases;

        public List<IntCurrency> Price;
    }
}