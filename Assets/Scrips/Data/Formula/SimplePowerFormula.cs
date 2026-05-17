// Formula inspired from: http://howtomakeanrpg.com/a/how-to-make-an-rpg-levels.html

using System;
using UnityEngine;

namespace Scrips.Data.Formula
{
    /// <summary>
    /// Formula based on y = Base * (Multiplier * x) ^ Exponent + Constant
    /// </summary>
    [CreateAssetMenu(menuName = "Tower defense kit/Formulas/Simple power")]
    public class SimplePowerFormula : BaseFormula
    {
        public float Exponent = 2;
        public long BaseValue = 1;
        public long Constant = 0;
        public float Multipier = 1;

        public override long GetLevelRequirementLong(int level)
        {
            return Mathf.FloorToInt(BaseValue * Mathf.Pow( + Multipier * level, Exponent)) + Constant;
        }

        /// <summary>
        /// In order to get a next value from the previous value, we need to calculate x (or level).
        /// Then we plot that x + 1 to calculate next value.
        /// The x in this case is Exponent-th root of the fraction (y - Constant) / Base.
        /// </summary>
        /// <param name="previousValue"></param>
        /// <returns></returns>
        public override long GetNextLevelRequirementLong(long previousValue)
        {
            return Mathf.FloorToInt(
                BaseValue * Mathf.Pow(Mathf.Pow(Multipier + (float)(previousValue - Constant)/BaseValue, 1/Exponent), Exponent) 
                + Constant);
        }
    }
}