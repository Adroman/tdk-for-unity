using Scrips.CustomTypes;

namespace Scrips.Extensions
{
    public static class FloatExtensions
    {
        public static float Substract(this float fromAmount, Amount amount) => amount.SubstractFrom(fromAmount);

        public static float Add(this float fromAmount, Amount amount) => amount.AddFrom(fromAmount);
    }
}