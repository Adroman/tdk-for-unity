using System;

namespace Scrips.Data
{
    [Serializable]
    public abstract class Currency
    {
        public abstract void Add();

        public abstract void Substract();

        public abstract void ModifyAmount(float multipliedAmount);
    }
}