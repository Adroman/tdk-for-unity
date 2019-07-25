using UnityEngine;

namespace Scrips.Data.Formula
{
    [CreateAssetMenu(menuName = "Tower defense kit/Formulas/Linear")]
    public class LinearFormula : BaseFormula
    {
        public long Base;
        public long Constant;
        
        public override int GetLevelRequirement(int level)
        {
            return (int) GetLevelRequirementLong(level);
        }

        public override long GetLevelRequirementLong(int level)
        {
            return Base * level + Constant;
        }
    }
}