using Scrips.Towers.BaseData;

namespace Scrips.Modifiers.Towers
{
    public class TowerNumberOfTargetsModifier : BaseTowerModifier
    {
        public override float GetBaseAmount(TowerInstance tower) => tower.NumberOfTargets;

        public override float GetModifiedAmount(TowerInstance tower) => tower.ModifiedNumberOfTargets;

        public override void SetModifiedAmount(TowerInstance tower, float value) => tower.ModifiedNumberOfTargets = (int) value;

        public override int? GetLastModifiedVersion(TowerInstance tower) => tower.LastModifiedNumberOfTargetsVersion;

        public override void SetLastModifiedVersion(TowerInstance tower, int value) =>
            tower.LastModifiedNumberOfTargetsVersion = value;
    }
}