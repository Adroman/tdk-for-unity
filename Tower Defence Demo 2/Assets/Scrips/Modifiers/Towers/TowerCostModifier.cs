using System;
using System.Linq;
using Scrips.CustomTypes.IncreaseType;
using Scrips.Data;
using Scrips.Towers.BaseData;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Tower/Cost modifier")]
    public class TowerCostModifier : BaseModifier
    {
        public int GetBaseAmount(TowerUiData tower, IntVariable currency)
            => tower.BaseTowerData.Price.FirstOrDefault(p => p.Variable == currency)?.Amount ?? 0;

        public int GetModifiedAmount(TowerUiData tower, IntVariable currency)
            => tower.ModifiedPrice.FirstOrDefault(p => p.Currency.Variable == currency)?.ModifiedAmount ?? 0;

        public void SetModifiedAmount(TowerUiData tower, IntVariable currency, int value)
        {
            var desiredPrice = tower.ModifiedPrice.FirstOrDefault(p => p.Currency.Variable == currency);
            if (desiredPrice != null) desiredPrice.ModifiedAmount = value;
        }

        public int? GetLastModifiedVersion(TowerUiData tower, IntVariable currency)
            => tower.ModifiedPrice.FirstOrDefault(p => p.Currency.Variable == currency)?.LastModifiedVersion;

        public void SetLastModifiedVersion(TowerUiData tower, IntVariable currency, int value)
        {
            var desiredPrice = tower.ModifiedPrice.FirstOrDefault(p => p.Currency.Variable == currency);
            if (desiredPrice != null) desiredPrice.LastModifiedVersion = value;
        }

        public static int CalculateModifiedValue<TModifier>(
            TowerUiData tower, ModifierController modifierController, IntVariable currency)
            where TModifier : TowerCostModifier
            => CalculateModifiedValue(tower, modifierController, currency, CreateInstance<TModifier>());

        public static int CalculateModifiedValue<TModifier>(
            TowerUiData tower, ModifierController modifierController, IntVariable currency, TModifier dummyModifier)
            where TModifier : TowerCostModifier
        {
            if (modifierController == null) return dummyModifier.GetBaseAmount(tower, currency);

            if (dummyModifier.GetLastModifiedVersion(tower, currency) == modifierController.Version)
                return dummyModifier.GetModifiedAmount(tower, currency);

            var modifiedAmounts = CalculateAmount<TModifier>(modifierController);

            int modifiedAmount = Mathf.RoundToInt((dummyModifier.GetBaseAmount(tower, currency) * (1 + modifiedAmounts.PercentageAmount) + modifiedAmounts.FlatAmount));
            dummyModifier.SetModifiedAmount(tower, currency, (int) modifiedAmount);
            dummyModifier.SetLastModifiedVersion(tower, currency, modifierController.Version);

            return modifiedAmount;
        }

        private static ModifiedAmount CalculateAmount<TModifier>(ModifierController modifierController) where TModifier : TowerCostModifier
        {
            float percentageAmount = 0;
            float flatAmount = 0;

            foreach (var modifier in modifierController.Modifiers)
            {
                var desiredModifier = modifier as TModifier;
                if (desiredModifier != null)
                {
                    if (desiredModifier.IncreaseType is MultiplicativeIncreaseType)
                        percentageAmount += desiredModifier.Amount * desiredModifier.Level;
                    else if (desiredModifier.IncreaseType is AdditiveIncreaseType)
                        flatAmount += desiredModifier.Amount * desiredModifier.Level;
                }
            }

            percentageAmount = Math.Max(-1, percentageAmount);    // if we get -150% modifier, we just zero it out to -100%

            return ModifiedAmount.CreateModifiedAmount(percentageAmount, flatAmount);
        }
    }
}