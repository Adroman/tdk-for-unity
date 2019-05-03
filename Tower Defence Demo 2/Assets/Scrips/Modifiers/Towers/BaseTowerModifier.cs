using System;
using Scrips.CustomTypes.IncreaseType;
using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    public abstract class BaseTowerModifier : BaseModifier
    {
        public abstract float GetBaseAmount(TowerInstance tower);

        public abstract float GetModifiedAmount(TowerInstance tower);

        public abstract void SetModifiedAmount(TowerInstance tower, float value);

        public abstract int? GetLastModifiedVersion(TowerInstance tower);

        public abstract void SetLastModifiedVersion(TowerInstance tower, int value);

        public abstract float GetBaseAmount(TowerUiData tower);

        public abstract float GetModifiedAmount(TowerUiData tower);

        public abstract void SetModifiedAmount(TowerUiData tower, float value);

        public abstract int? GetLastModifiedVersion(TowerUiData tower);

        public abstract void SetLastModifiedVersion(TowerUiData tower, int value);

        public static float CalculateModifiedValue<TModifier>(TowerInstance tower, ModifierController modifierController)
            where TModifier : BaseTowerModifier
            => CalculateModifiedValue(tower, modifierController, CreateInstance<TModifier>());

        public static float CalculateModifiedValue<TModifier>(
            TowerInstance tower, ModifierController modifierController, TModifier dummyModifier)
            where TModifier : BaseTowerModifier
        {
            if (modifierController == null) return dummyModifier.GetBaseAmount(tower);

            if (dummyModifier.GetLastModifiedVersion(tower) == modifierController.Version)
                return dummyModifier.GetModifiedAmount(tower);

            var modifiedAmounts = CalculateAmount<TModifier>(modifierController);

            float modifiedAmount = dummyModifier.GetBaseAmount(tower) * (1 + modifiedAmounts.PercentageAmount) + modifiedAmounts.FlatAmount;
            dummyModifier.SetModifiedAmount(tower, modifiedAmount);
            dummyModifier.SetLastModifiedVersion(tower, modifierController.Version);

            return modifiedAmount;
        }

        public static float CalculateModifiedValue<TModifier>(TowerUiData tower, ModifierController modifierController)
            where TModifier : BaseTowerModifier
            => CalculateModifiedValue(tower, modifierController, CreateInstance<TModifier>());

        public static float CalculateModifiedValue<TModifier>(
            TowerUiData tower, ModifierController modifierController, TModifier dummyModifier)
            where TModifier : BaseTowerModifier
        {
            if (modifierController == null) return dummyModifier.GetBaseAmount(tower);

            if (dummyModifier.GetLastModifiedVersion(tower) == modifierController.Version)
                return dummyModifier.GetModifiedAmount(tower);

            var modifiedAmounts = CalculateAmount<TModifier>(modifierController);

            float modifiedAmount = dummyModifier.GetBaseAmount(tower) * (1 + modifiedAmounts.PercentageAmount) + modifiedAmounts.FlatAmount;
            dummyModifier.SetModifiedAmount(tower, modifiedAmount);
            dummyModifier.SetLastModifiedVersion(tower, modifierController.Version);

            return modifiedAmount;
        }

        private static ModifiedAmount CalculateAmount<TModifier>(ModifierController modifierController) where TModifier : BaseTowerModifier
        {
            float percentageAmount = 0;
            float flatAmount = 0;

            foreach (var modifier in modifierController.Modifiers)
            {
                var desiredModifier = modifier as TModifier;
                if (desiredModifier != null)
                {
                    if (desiredModifier.IncreaseType is MultiplicativeIncreaseType)
                        percentageAmount += desiredModifier.Amount;
                    else if (desiredModifier.IncreaseType is AdditiveIncreaseType)
                        flatAmount += desiredModifier.Amount;
                }
            }

            percentageAmount = Math.Max(-1, percentageAmount);    // if we get -150% modifier, we just zero it out to -100%

            return ModifiedAmount.CreateModifiedAmount(percentageAmount, flatAmount);
        }
    }
}