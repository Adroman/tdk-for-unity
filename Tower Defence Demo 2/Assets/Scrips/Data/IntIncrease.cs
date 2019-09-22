using System;
using Scrips.CustomTypes.IncreaseType;
using UnityEngine;

namespace Scrips.Data
{
    [Serializable]
    public struct IntIncrease
    {
        public BaseIncreaseType Type;
        public float Amount;

        public int Apply(int from) => Type.Increase(from, Amount);
    }
}