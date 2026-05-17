using UnityEngine;

namespace Scrips.Data.Formula
{
    /// <summary>
    /// Formula based on y = Base * x + Constant
    /// </summary>
    [CreateAssetMenu(menuName = "Tower defense kit/Formulas/Linear")]
    public class LinearFormula : BaseFormula
    {
        public long Base;
        public long Constant;

        public override long GetLevelRequirementLong(int level)
        {
            return Base * level + Constant;
        }

        public override long GetNextLevelRequirementLong(long previousValue)
        {
            return (previousValue - Constant) * Base + Constant;
        }
    }
}