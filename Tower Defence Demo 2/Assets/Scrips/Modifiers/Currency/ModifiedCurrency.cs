using System;
using Scrips.Data;
using Scrips.Modifiers.Stats;

namespace Scrips.Modifiers.Currency
{
    [Serializable]
    public class ModifiedCurrency
    {
        public IntCurrency Currency;

        public IntModifiableStat Amount;

        public ModifiedCurrency()
        {
            Amount = new IntModifiableStat();
        }

        public void Add()
        {
            Currency.Variable.Value += Amount.Value;
        }

        public void Subtract()
        {
            Currency.Variable.Value -= Amount.Value;
        }

        public bool HasEnough()
        {
            return Currency.Variable.Value >= Amount.Value;
        }
    }
}