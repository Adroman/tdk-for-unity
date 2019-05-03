using System;
using Scrips.Data;

namespace Scrips.Modifiers.Currency
{
    [System.Serializable]
    public class ModifiedCurrency
    {
        public IntCurrency Currency;

        public int? LastModifiedVersion;

        public int ModifiedAmount;

        public int GetModifiedValue(Func<int> calculation)
        {
            return calculation();
        }
    }
}