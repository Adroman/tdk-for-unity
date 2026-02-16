using System.Collections.Generic;
using System.Linq;
using Scrips.Towers.BaseData;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    [CreateAssetMenu(menuName = "Tower defense kit/Modifiers/Tower/Cost modifier")]
    public class TowerBuildCostModifier : TowerCostModifier
    {
        public void AddToTower(TowerUiData tower, IntVariable variable)
        {
            if ((WhiteList.Count > 0 && !WhiteList.Contains(variable)) || BlackList.Contains(variable)) return;

            foreach (var modifiedCurrency in 
                     tower.ModifiedPurchasePrice
                         .Where(p => p.Currency.Variable == variable))
            {
                modifiedCurrency.Amount.AddModifier(this);
            }
        }

        public void RemoveFromTower(TowerUiData tower, IntVariable variable)
        {
            foreach (var modifiedCurrency in 
                     tower.ModifiedPurchasePrice
                         .Where(p => p.Currency.Variable == variable))
            {
                modifiedCurrency.Amount.RemoveModifier(this);
            }
        }

        public override void AddToTower(TowerData tower, IntVariable variable)
        {
            throw new System.NotImplementedException();
        }

        public override void RemoveFromTower(TowerData tower, IntVariable variable)
        {
            throw new System.NotImplementedException();
        }
    }
}