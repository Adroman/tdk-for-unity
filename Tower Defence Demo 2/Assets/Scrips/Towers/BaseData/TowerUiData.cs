using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Data;
using Scrips.Modifiers;
using Scrips.Modifiers.Currency;
using Scrips.Modifiers.Towers;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Towers.BaseData
{
    public class TowerUiData : MonoBehaviour
    {
        public TowerData BaseTowerData;
        public ModifierController ModifierController;

        public float ActualMinDamage => BaseTowerModifier.CalculateModifiedValue<TowerMinDamageModifier>(this, ModifierController);

        public float ActualMaxDamage => BaseTowerModifier.CalculateModifiedValue<TowerMaxDamageModifier>(this, ModifierController);

        public float ActualFiringSpeed => BaseTowerModifier.CalculateModifiedValue<TowerFiringSpeedModifier>(this, ModifierController);

        public float ActualRange => BaseTowerModifier.CalculateModifiedValue<TowerRangeModifier>(this, ModifierController);

        public float ActualNumberOfTargets => BaseTowerModifier.CalculateModifiedValue<TowerNumberOfTargetsModifier>(this, ModifierController);

        public float ModifiedMinDamage;
        public float ModifiedMaxDamage;
        public float ModifiedFiringSpeed;
        public float ModifiedRange;
        public int ModifiedNumberOfTargets;

        public int? LastModifiedMinDamageVersion;
        public int? LastModifiedMaxDamageVersion;
        public int? LastModifiedFiringSpeedVersion;
        public int? LastModifiedRangeVersion;
        public int? LastModifiedNumberOfTargetsVersion;

        public ModifiedCurrency[] ModifiedPrice;

        public int GetModifiedPrice(IntVariable currency)
        {
            return ModifiedPrice.First(p => p.Currency.Variable == currency)
                .GetModifiedValue(GetModifiedPriceDelegate(currency));
        }

        public IEnumerable<IntCurrency> GetModifiedPrice() => ModifiedPrice.Select(mp =>
            new IntCurrency()
            {
                Amount = mp.GetModifiedValue(GetModifiedPriceDelegate(mp.Currency.Variable)),
                Variable = mp.Currency.Variable
            });

        private void Start()
        {
            SetUpModifiedPrice();
        }
        
        private void SetUpModifiedPrice()
        {
            ModifiedPrice = new ModifiedCurrency[BaseTowerData.Price.Count];
            for (int i = 0; i < BaseTowerData.Price.Count; i++)
            {
                var newModified = new ModifiedCurrency
                {
                    Currency = BaseTowerData.Price[i],
                    LastModifiedVersion = null
                };
                ModifiedPrice[i] = newModified;
                newModified.GetModifiedValue(GetModifiedPriceDelegate(newModified.Currency.Variable));
            }
        }

        private Func<int> GetModifiedPriceDelegate(IntVariable currency)
            => () => TowerCostModifier.CalculateModifiedValue<TowerCostModifier>(this, ModifierController, currency);
    }
}