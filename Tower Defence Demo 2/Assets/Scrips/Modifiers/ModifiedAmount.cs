namespace Scrips.Modifiers
{
    public struct ModifiedAmount
    {
        public float FlatAmount;
        public float PercentageAmount;

        public static ModifiedAmount CreateModifiedAmount(float percentageAmount, float flatAmount)
        {
            return new ModifiedAmount()
            {
                PercentageAmount = percentageAmount,
                FlatAmount = flatAmount
            };
        }
    }
}