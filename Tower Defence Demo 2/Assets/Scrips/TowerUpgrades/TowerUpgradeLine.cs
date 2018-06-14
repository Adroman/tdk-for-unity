using System;
using System.Collections.Generic;

namespace Scrips.TowerUpgrades
{
    [Serializable]
    public class TowerUpgradeLine
    {
        public bool UseInfinite;

        public TowerUpgradeLineNode InfiniteUpgradeNode;

        public List<TowerUpgradeLineNode> UpgradeLineNodes;
    }
}