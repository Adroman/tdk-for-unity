using System;
using Scrips.CustomTypes.IncreaseType;

namespace Scrips.Data
{
    [Serializable]
    public struct FloatIncrease
    {
        public BaseIncreaseType Type;
        public float Amount;

        public float Apply(float from) => Type.Increase(@from, Amount);
    }
}