using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Modifiers/Tower/Maximum damage modifier")]
    public class TowerMaxDamageModifier : BaseTowerModifier
    {
        public override float GetBaseAmount(TowerInstance tower) => tower.MaxDamage;

        public override float GetModifiedAmount(TowerInstance tower) => tower.ModifiedMaxDamage;

        public override void SetModifiedAmount(TowerInstance tower, float value) => tower.ModifiedMaxDamage = value;

        public override int? GetLastModifiedVersion(TowerInstance tower) => tower.LastModifiedMaxDamageVersion;

        public override void SetLastModifiedVersion(TowerInstance tower, int value) =>
            tower.LastModifiedMaxDamageVersion = value;
    }
}