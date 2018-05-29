using System;
using JetBrains.Annotations;
using Scrips.Variables;

namespace Scrips.Data
{
    [PublicAPI]
    [Serializable]
    public class IntCurrency
    {
        public int Amount;
        public IntVariable Variable;

        public IntCurrency()
        {}

        public IntCurrency(IntCurrency original)
        {
            Variable = original.Variable;
            Amount = original.Amount;
        }

        public void Add()
        {
            Variable.Value += Amount;
        }

        public void Substract()
        {
            Variable.Value -= Amount;
        }

        public void ModifyAmount(float multipliedAmount)
        {
            Amount = (int)(Amount * multipliedAmount);
        }

        public bool HasEnough()
        {
            return Variable.Value >= Amount;
        }
    }
}