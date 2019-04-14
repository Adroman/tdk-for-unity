using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Modifiers/Tower/Range modifier")]
    public class TowerRangeModifier : BaseTowerModifier
    {
        public override float GetBaseAmount(TowerInstance tower) => tower.Range;

        public override float GetModifiedAmount(TowerInstance tower) => tower.ModifiedRange;

        public override void SetModifiedAmount(TowerInstance tower, float value) => tower.ModifiedRange = value;

        public override int? GetLastModifiedVersion(TowerInstance tower) => tower.LastModifiedRangeVersion;

        public override void SetLastModifiedVersion(TowerInstance tower, int value) =>
            tower.LastModifiedRangeVersion = value;
    }
}