using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scrips.Data;
using Scrips.Events.Alerts;
using Scrips.Events.Towers;
using Scrips.Modifiers;
using Scrips.Modifiers.Currency;
using Scrips.Towers.Specials;
using UnityEngine;
using UnityEngine.Serialization;

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

        [FormerlySerializedAs("Price")]
        [Tooltip("The price for building the tower")]
        public List<IntCurrency> BuildPrice;
        
        [Tooltip("The price for selling/destroying the tower")]
        public List<IntCurrency> SellPrice;
        
        [Tooltip("The refund for selling/destroying the tower")]
        public List<IntCurrency> SellRefund;

        public Sprite PreviewSprite;

        public TowerEvent OnTowerBuilt;

        public TowerInstance BuildTower(
            Vector3 position,
            Quaternion rotation,
            Transform parent,
            TowerUiData data,
            AlertEvent userErrorAlert)
        {
            var modifiedPrice = data.GetModifiedPrice().ToList();

            if (!modifiedPrice.All(p => p.HasEnough()))
            {
                Debug.LogWarning("Not enough resources to build the tower.");
                if (userErrorAlert != null) 
                    userErrorAlert.Invoke("Error building tower", "Not enough resources to build the tower.");
                
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
            tower.SellPrice = BuildModifiedPrice(SellPrice);
            tower.SellRefund = BuildModifiedPrice(SellRefund);
            
            if (OnTowerBuilt != null) OnTowerBuilt.Invoke(tower);
            return tower;
        }
        
        public static ModifiedCurrency[] BuildModifiedPrice(IList<IntCurrency> priceToBuildFrom)
        {
            var priceToUse = new ModifiedCurrency[priceToBuildFrom.Count];

            for (int i = 0; i < priceToBuildFrom.Count; i++)
            {
                var newModified = new ModifiedCurrency
                {
                    Currency = priceToBuildFrom[i],
                    Amount = { Value = priceToBuildFrom[i].Amount }
                };
                priceToUse[i] = newModified;
            }

            return priceToUse;
        }
    }
}