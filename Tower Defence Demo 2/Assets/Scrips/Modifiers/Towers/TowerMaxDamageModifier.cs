using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Tower/Maximum damage modifier")]
    public class TowerMaxDamageModifier : BaseTowerModifier
    {
        public override void AddToTower(TowerUiData tower)
        {
            tower.MaxDamage.AddModifier(this);
        }

        public override void RemoveFromTower(TowerUiData tower)
        {
            tower.MaxDamage.RemoveModifier(this);
        }

        public override void AddToTower(TowerInstance tower)
        {
            tower.MaxDamage.AddModifier(this);
        }

        public override void RemoveFromTower(TowerInstance tower)
        {
            tower.MaxDamage.RemoveModifier(this);
        }
    }
}