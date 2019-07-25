using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Tower/Range modifier")]
    public class TowerRangeModifier : BaseTowerModifier
    {
        public override void AddToTower(TowerUiData tower)
        {
            tower.Range.AddModifier(this);
        }

        public override void RemoveFromTower(TowerUiData tower)
        {
            tower.Range.RemoveModifier(this);
        }

        public override void AddToTower(TowerInstance tower)
        {
            tower.Range.AddModifier(this);
        }

        public override void RemoveFromTower(TowerInstance tower)
        {
            tower.Range.RemoveModifier(this);
        }
    }
}