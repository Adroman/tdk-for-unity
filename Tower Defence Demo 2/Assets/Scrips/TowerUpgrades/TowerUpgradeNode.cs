using System;
using Scrips.Data;
using UnityEngine;

namespace Scrips.TowerUpgrades
{
    [Serializable]
    public class TowerUpgradeNode
    {
        public FloatIncrease MinAtkIncrease;
        public FloatIncrease MaxAtkIncrease;
        public FloatIncrease RangeIncrease;
        public FloatIncrease FiringSpeedIncrease;

        public TowerUpgradeNode[] NextLineNodes;

        public TowerUpgradeNode[] LineNodesBranches;

        public GameObject ChangeModel;

        public IntCurrency Price;
    }
}