using System.Collections.Generic;
using System.Linq;
using Scrips.EnemyData.Instances;
using Scrips.Modifiers.Currency;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Modifiers.Enemies
{
    public abstract class BaseEnemyCurrencyModifier : BaseModifier
    {
        public List<IntVariable> BlackList;

        [Tooltip("If this collection is not empty, the modifier will be limited to these variables.")]
        public List<IntVariable> WhiteList;

        public void AddToEnemy(EnemyInstance enemyInstance, IntVariable variable = null)
        {
            bool everyVariable = variable == null;

            foreach (var modifiedCurrency in GetDesiredCollection(enemyInstance).Where(l => everyVariable || l.Currency.Variable == variable))
            {
                if (!IsValidVariable(modifiedCurrency.Currency.Variable)) continue;

                modifiedCurrency.Amount.AddModifier(this);
            }
        }

        public void RemoveFromEnemy(EnemyInstance enemyInstance, IntVariable variable = null)
        {
            bool everyVariable = variable == null;

            foreach (var modifiedCurrency in GetDesiredCollection(enemyInstance).Where(l => everyVariable || l.Currency.Variable == variable))
            {
                modifiedCurrency.Amount.RemoveModifier(this);
            }
        }

        protected abstract List<ModifiedCurrency> GetDesiredCollection(EnemyInstance enemyInstance);

        private bool IsValidVariable(IntVariable variable)
            => !BlackList.Contains(variable) && (WhiteList.Count == 0 || WhiteList.Contains(variable));
    }
}