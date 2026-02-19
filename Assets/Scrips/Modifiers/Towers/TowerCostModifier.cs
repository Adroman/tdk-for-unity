using System;
using System.Collections.Generic;
using Scrips.Towers.BaseData;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    public abstract class TowerCostModifier : BaseModifier
    {
        [Tooltip("The list of currencies which should be excluded from the modifier.")]
        public List<IntVariable> BlackList;

        [Tooltip("The list of currencies which should be included from the modifier./n" + 
                 "If this collection is not empty, the modifier will be limited to these variables.")]
        public List<IntVariable> WhiteList;

        public abstract void AddToTower(TowerData tower, IntVariable variable);
        
        public abstract void RemoveFromTower(TowerData tower, IntVariable variable);
    }
}