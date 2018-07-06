using System;
using UnityEngine;

namespace Scrips.Towers.Specials
{
    [Serializable]
    public abstract class SpecialUpgrade : ScriptableObject
    {
        public SpecialType SpecialType;

        public abstract void Upgrade(SpecialComponent component);
    }
}