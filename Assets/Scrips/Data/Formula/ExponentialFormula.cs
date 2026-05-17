using UnityEngine;

namespace Scrips.Data.Formula
{
    /// <summary>
    /// Formula based on y = BaseValue ^ x + Constant
    /// </summary>
    [CreateAssetMenu(menuName = "Tower defense kit/Formulas/Exponential")]
    public class ExponentialFormula : BaseFormula
    {
        public long BaseValue;
        public long Constant;
        
        public override long GetLevelRequirementLong(int level)
        {
            return (long)Mathf.Pow(BaseValue, level) + Constant;
        }

        /// <summary>
        /// Calculating the next value is pretty easy here, it's just a previous value multiplied by a BaseValue.
        /// </summary>
        /// <param name="previousValue"></param>
        /// <returns></returns>
        public override long GetNextLevelRequirementLong(long previousValue)
        {
            return BaseValue * (previousValue - Constant) + Constant;
        }
    }
}