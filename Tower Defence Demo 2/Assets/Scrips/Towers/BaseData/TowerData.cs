using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scrips.Data;
using Scrips.Events.Towers;
using Scrips.Modifiers;
using Scrips.Towers.Specials;
using UnityEngine;

namespace Scrips.Towers.BaseData
{
    [CreateAssetMenu(menuName = "Towers/Tower data")]
    public class TowerData : ScriptableObject
    {
        public string TowerName;
        public TowerInstance Prefab;

        public float MinDamage;
        public float MaxDamage;
        public float FiringSpeed;
        public float Range;
        public int NumberOfTargets;

        public List<SpecialType> Specials;

        public List<TowerUpgradeNode> Upgrades;

        public List<IntCurrency> Price;

        public Sprite PreviewSprite;

        public TowerEvent OnTowerBuilt;

        public TowerInstance BuildTower(Vector3 position, Quaternion rotation, Transform parent, ModifierController modifierController)
        {
            if (!Price.All(p => p.HasEnough()))
            {
                // not enough resources
                return null;
            }

            Price.ForEach(p => p.Subtract());

            var tower = Instantiate(Prefab, position, rotation, parent);
            tower.Name = TowerName;
            tower.ModifierController = modifierController;
            tower.ActualMinDamage = MinDamage;
            tower.ActualMaxDamage = MaxDamage;
            tower.ActualFiringSpeed = FiringSpeed;
            tower.ActualRange = Range;
            tower.ActualNumberOfTargets = NumberOfTargets;
            tower.Upgrades = Upgrades.ToList();
            foreach (var special in Specials)
            {
                special.GetOrCreateSpecialComponent(tower.gameObject);
            }

            if (OnTowerBuilt != null) OnTowerBuilt.Invoke(tower);
            return tower;
        }
    }
}