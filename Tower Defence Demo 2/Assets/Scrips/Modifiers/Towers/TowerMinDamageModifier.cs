using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Tower/Minimum damage modifier")]
    public class TowerMinDamageModifier : BaseTowerModifier
    {
        public override void AddToTower(TowerUiData tower)
        {
            tower.MinDamage.AddModifier(this);
        }

        public override void RemoveFromTower(TowerUiData tower)
        {
            tower.MinDamage.RemoveModifier(this);
        }

        public override void AddToTower(TowerInstance tower)
        {
            tower.MinDamage.AddModifier(this);
        }

        public override void RemoveFromTower(TowerInstance tower)
        {
            tower.MinDamage.RemoveModifier(this);
        }
    }
}