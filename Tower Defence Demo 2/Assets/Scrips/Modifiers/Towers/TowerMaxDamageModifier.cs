using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Tower/Maximum damage modifier")]
    public class TowerMaxDamageModifier : BaseTowerModifier
    {
        public override float GetBaseAmount(TowerInstance tower) => tower.MaxDamage;

        public override float GetModifiedAmount(TowerInstance tower) => tower.ModifiedMaxDamage;

        public override void SetModifiedAmount(TowerInstance tower, float value) => tower.ModifiedMaxDamage = value;

        public override int? GetLastModifiedVersion(TowerInstance tower) => tower.LastModifiedMaxDamageVersion;

        public override void SetLastModifiedVersion(TowerInstance tower, int value) =>
            tower.LastModifiedMaxDamageVersion = value;

        public override float GetBaseAmount(TowerUiData tower) => tower.BaseTowerData.MaxDamage;

        public override float GetModifiedAmount(TowerUiData tower) => tower.ModifiedMaxDamage;

        public override void SetModifiedAmount(TowerUiData tower, float value) => tower.ModifiedMaxDamage = value;

        public override int? GetLastModifiedVersion(TowerUiData tower) => tower.LastModifiedMaxDamageVersion;

        public override void SetLastModifiedVersion(TowerUiData tower, int value) => tower.LastModifiedMaxDamageVersion = value;
    }
}