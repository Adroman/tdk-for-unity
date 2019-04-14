using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Modifiers/Tower/Minimum damage modifier")]
    public class TowerMinDamageModifier : BaseTowerModifier
    {
        public override float GetBaseAmount(TowerInstance tower) => tower.MinDamage;

        public override float GetModifiedAmount(TowerInstance tower) => tower.ModifiedMinDamage;

        public override void SetModifiedAmount(TowerInstance tower, float value) => tower.ModifiedMinDamage = value;

        public override int? GetLastModifiedVersion(TowerInstance tower) => tower.LastModifiedMinDamageVersion;

        public override void SetLastModifiedVersion(TowerInstance tower, int value) =>
            tower.LastModifiedMinDamageVersion = value;
    }
}