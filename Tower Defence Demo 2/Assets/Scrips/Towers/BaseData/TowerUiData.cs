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

        public ModifiedCurrency[] ModifiedPrice;

        public int GetModifiedPrice(IntVariable currency)
        {
            return ModifiedPrice.First(p => p.Currency.Variable == currency).Amount.Value;
        }

        public IEnumerable<IntCurrency> GetModifiedPrice() => ModifiedPrice.Select(mp =>
            new IntCurrency()
            {
                Amount = mp.Amount.Value,
                Variable = mp.Currency.Variable
            });

        private void Start()
        {
            SetUpModifiedPrice();
            ModifierController.ImportModifiers(this);
            MinDamage.Value = BaseTowerData.MinDamage;
            MaxDamage.Value = BaseTowerData.MaxDamage;
            FiringSpeed.Value = BaseTowerData.FiringSpeed;
            Range.Value = BaseTowerData.Range;
            NumberOfTargets.Value = BaseTowerData.NumberOfTargets;
        }

        private void SetUpModifiedPrice()
        {
            ModifiedPrice = new ModifiedCurrency[BaseTowerData.Price.Count];
            for (int i = 0; i < BaseTowerData.Price.Count; i++)
            {
                var newModified = new ModifiedCurrency
                {
                    Currency = BaseTowerData.Price[i],
                    Amount = {Value = BaseTowerData.Price[i].Amount},
                };
                ModifiedPrice[i] = newModified;
            }
        }
    }
}