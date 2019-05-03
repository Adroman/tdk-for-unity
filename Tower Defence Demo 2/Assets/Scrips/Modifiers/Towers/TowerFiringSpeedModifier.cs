using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Tower/Firing speed modifier")]
    public class TowerFiringSpeedModifier : BaseTowerModifier
    {
        public override float GetBaseAmount(TowerInstance tower) => tower.FiringSpeed;

        public override float GetModifiedAmount(TowerInstance tower) => tower.ModifiedFiringSpeed;

        public override void SetModifiedAmount(TowerInstance tower, float value) => tower.ModifiedFiringSpeed = value;

        public override int? GetLastModifiedVersion(TowerInstance tower) => tower.LastModifiedFiringSpeedVersion;

        public override void SetLastModifiedVersion(TowerInstance tower, int value) =>
            tower.LastModifiedFiringSpeedVersion = value;

        public override float GetBaseAmount(TowerUiData tower) => tower.BaseTowerData.FiringSpeed;

        public override float GetModifiedAmount(TowerUiData tower) => tower.ModifiedFiringSpeed;

        public override void SetModifiedAmount(TowerUiData tower, float value) => tower.ModifiedFiringSpeed = value;

        public override int? GetLastModifiedVersion(TowerUiData tower) => tower.LastModifiedFiringSpeedVersion;

        public override void SetLastModifiedVersion(TowerUiData tower, int value) => tower.LastModifiedFiringSpeedVersion = value;
    }
}