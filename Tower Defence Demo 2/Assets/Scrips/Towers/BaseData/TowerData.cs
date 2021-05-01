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
    [CreateAssetMenu(menuName = "Tower defense kit/Towers/Tower data")]
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

        public TowerInstance BuildTower(
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            TowerUiData data)
        {
            var modifiedPrice = data.GetModifiedPrice().ToList();

            if (!modifiedPrice.All(p => p.HasEnough()))
            {
                // not enough resources
                return null;
            }

            modifiedPrice.ForEach(p => p.Subtract());

            var tower = Instantiate(Prefab, position, rotation, parent);
            tower.Name = TowerName;
            tower.ModifierController = data.ModifierController;
            foreach (var special in Specials)
            {
                special.GetOrCreateSpecialComponent(tower.gameObject);
            }
            tower.ModifierController.ImportModifiers(tower);
            tower.MinDamage.Value = MinDamage;
            tower.MaxDamage.Value = MaxDamage;
            tower.ActualFiringSpeed = FiringSpeed;
            tower.ActualRange = Range;
            tower.NumberOfTargets.Value = NumberOfTargets;
            tower.Upgrades = Upgrades.ToList();
            
            if (OnTowerBuilt != null) OnTowerBuilt.Invoke(tower);
            return tower;
        }
    }
}