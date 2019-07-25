using System;
using Scrips.CustomTypes.IncreaseType;
using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.Modifiers.Towers
{
    public abstract class BaseTowerModifier : BaseModifier
    {
        public abstract void AddToTower(TowerUiData tower);
        
        public abstract void RemoveFromTower(TowerUiData tower);
        
        public abstract void AddToTower(TowerInstance tower);
        
        public abstract void RemoveFromTower(TowerInstance tower);
    }
}