using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Tower/Minimum damage modifier")]
    public class TowerMinDamageModifier : BaseTowerModifier
    {
        public override float GetBaseAmount(TowerInstance tower) => tower.MinDamage;

        public override float GetModifiedAmount(TowerInstance tower) => tower.ModifiedMinDamage;

        public override void SetModifiedAmount(TowerInstance tower, float value) => tower.ModifiedMinDamage = value;

        public override int? GetLastModifiedVersion(TowerInstance tower) => tower.LastModifiedMinDamageVersion;

        public override void SetLastModifiedVersion(TowerInstance tower, int value) =>
            tower.LastModifiedMinDamageVersion = value;

        public override float GetBaseAmount(TowerUiData tower) => tower.BaseTowerData.MinDamage;

        public override float GetModifiedAmount(TowerUiData tower) => tower.ModifiedMinDamage;

        public override void SetModifiedAmount(TowerUiData tower, float value) => tower.ModifiedMinDamage = value;

        public override int? GetLastModifiedVersion(TowerUiData tower) => tower.LastModifiedMinDamageVersion;

        public override void SetLastModifiedVersion(TowerUiData tower, int value) => tower.LastModifiedMinDamageVersion = value;
    }
}