using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Tower/Firing speed modifier")]
    public class TowerFiringSpeedModifier : BaseTowerModifier
    {
        public override void AddToTower(TowerUiData tower)
        {
            tower.FiringSpeed.AddModifier(this);
        }

        public override void RemoveFromTower(TowerUiData tower)
        {
            tower.FiringSpeed.RemoveModifier(this);
        }

        public override void AddToTower(TowerInstance tower)
        {
            tower.FiringSpeed.AddModifier(this);
        }

        public override void RemoveFromTower(TowerInstance tower)
        {
            tower.FiringSpeed.RemoveModifier(this);
        }
    }
}