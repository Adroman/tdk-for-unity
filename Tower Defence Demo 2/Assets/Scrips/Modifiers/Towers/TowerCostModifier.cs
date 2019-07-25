using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.CustomTypes.IncreaseType;
using Scrips.Data;
using Scrips.Modifiers.Currency;
using Scrips.Towers.BaseData;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Tower/Cost modifier")]
    public class TowerCostModifier : BaseModifier
    {
        public List<IntVariable> BlackList;

        [Tooltip("If this collection is not empty, the modifier will be limited to these variables.")]
        public List<IntVariable> WhiteList;

        public void AddToTower(TowerUiData tower, IntVariable variable)
        {
            if ((WhiteList.Count > 0 && !WhiteList.Contains(variable)) || BlackList.Contains(variable)) return;

            foreach (var modifiedCurrency in tower.ModifiedPrice.Where(p => p.Currency.Variable == variable))
            {
                modifiedCurrency.Amount.AddModifier(this);
            }
        }

        public void RemoveFromTower(TowerUiData tower, IntVariable variable)
        {
            foreach (var modifiedCurrency in tower.ModifiedPrice.Where(p => p.Currency.Variable == variable))
            {
                modifiedCurrency.Amount.RemoveModifier(this);
            }
        }
    }
}