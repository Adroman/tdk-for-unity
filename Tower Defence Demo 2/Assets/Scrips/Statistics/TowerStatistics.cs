using Scrips.Towers.BaseData;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Statistics
{
    public class TowerStatistics : MonoBehaviour
    {
        public IntVariable TowersBuilt;

        public void UpdateStatistics(TowerInstance target)
        {
            if (TowersBuilt != null) TowersBuilt.Value++;
        }
    }
}