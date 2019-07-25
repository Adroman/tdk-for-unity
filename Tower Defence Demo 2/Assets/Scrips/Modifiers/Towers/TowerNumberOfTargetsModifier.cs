using Scrips.Towers.BaseData;

namespace Scrips.Modifiers.Towers
{
    public class TowerNumberOfTargetsModifier : BaseTowerModifier
    {
        public override void AddToTower(TowerUiData tower)
        {
            tower.NumberOfTargets.AddModifier(this);
        }

        public override void RemoveFromTower(TowerUiData tower)
        {
            tower.NumberOfTargets.RemoveModifier(this);
        }

        public override void AddToTower(TowerInstance tower)
        {
            tower.NumberOfTargets.AddModifier(this);
        }

        public override void RemoveFromTower(TowerInstance tower)
        {
            tower.NumberOfTargets.RemoveModifier(this);
        }
    }
}