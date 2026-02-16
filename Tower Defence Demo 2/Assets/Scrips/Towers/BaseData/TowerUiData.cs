using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Data;
using Scrips.Modifiers;
using Scrips.Modifiers.Currency;
using Scrips.Modifiers.Stats;
using Scrips.Modifiers.Towers;
using Scrips.Variables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scrips.Towers.BaseData
{
    public class TowerUiData : MonoBehaviour
    {
        public TowerData BaseTowerData;
        public ModifierController ModifierController;

        public FloatModifiableStat MinDamage;
        public FloatModifiableStat MaxDamage;
        public FloatModifiableStat FiringSpeed;
        public FloatModifiableStat Range;
        public IntModifiableStat NumberOfTargets;

        // Price for building Tower
        [FormerlySerializedAs("ModifiedPrice")]
        public ModifiedCurrency[] ModifiedPurchasePrice;
        
        // Price for selling Tower - could be used as a limitation against juggling
        public ModifiedCurrency[] ModifiedSellingPrice;
        
        // Amount of resources returned to player when selling Tower
        public ModifiedCurrency[] ModifiedSellingRefund;

        public IEnumerable<IntCurrency> GetModifiedPrice() => ModifiedPurchasePrice.Select(mp =>
            new IntCurrency
            {
                Amount = mp.Amount.Value,
                Variable = mp.Currency.Variable
            });

        private void OnEnable()
        {
            SetUpModifiedPrice();
            MinDamage.Value = BaseTowerData.MinDamage;
            MaxDamage.Value = BaseTowerData.MaxDamage;
            FiringSpeed.Value = BaseTowerData.FiringSpeed;
            Range.Value = BaseTowerData.Range;
            NumberOfTargets.Value = BaseTowerData.NumberOfTargets;
        }

        private void Start()
        {
            ModifierController.ImportModifiers(this);
        }

        private void SetUpModifiedPrice()
        {
            ModifiedPurchasePrice = TowerData.BuildModifiedPrice(BaseTowerData.BuildPrice);
            
            // ModifiedPurchasePrice = new ModifiedCurrency[BaseTowerData.Price.Count];
            // for (int i = 0; i < BaseTowerData.Price.Count; i++)
            // {
            //     var newModified = new ModifiedCurrency
            //     {
            //         Currency = BaseTowerData.Price[i],
            //         Amount = {Value = BaseTowerData.Price[i].Amount},
            //     };
            //     ModifiedPurchasePrice[i] = newModified;
            // }
        }
    }
}