using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Modifiers/Tower/Firing speed modifier")]
    public class TowerFiringSpeedModifier : BaseTowerModifier
    {
        public override float GetBaseAmount(TowerInstance tower) => tower.FiringSpeed;

        public override float GetModifiedAmount(TowerInstance tower) => tower.ModifiedFiringSpeed;

        public override void SetModifiedAmount(TowerInstance tower, float value) => tower.ModifiedFiringSpeed = value;

        public override int? GetLastModifiedVersion(TowerInstance tower) => tower.LastModifiedFiringSpeedVersion;

        public override void SetLastModifiedVersion(TowerInstance tower, int value) =>
            tower.LastModifiedFiringSpeedVersion = value;
    }
}