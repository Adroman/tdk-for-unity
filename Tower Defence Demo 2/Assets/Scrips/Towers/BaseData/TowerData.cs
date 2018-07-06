using System.Collections.Generic;
using System.Linq;
using Scrips.Data;
using Scrips.Towers.Specials;
using UnityEngine;

namespace Scrips.Towers.BaseData
{
    public class TowerData : ScriptableObject
    {
        public TowerInstance Prefab;

        public float MinDamage;
        public float MaxDamage;
        public float FiringSpeed;
        public float Range;

        public List<SpecialType> Specials;

        public List<TowerUpgradeLineNode> Upgrades;

        public List<IntCurrency> Price;

        public TowerInstance BuildTower(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (!Price.All(p => p.HasEnough()))
            {
                // not enough resources
                return null;
            }

            Price.ForEach(p => p.Substract());

            var tower = Instantiate(Prefab, position, rotation, parent);
            tower.MinDamage = MinDamage;
            tower.MaxDamage = MaxDamage;
            tower.FiringSpeed = FiringSpeed;
            tower.Range = Range;
            tower.Upgrades = Upgrades.ToList();
            foreach (var special in Specials)
            {
                special.GetOrCreateSpecialComponent(tower.gameObject);
            }

            return tower;
        }
    }
}