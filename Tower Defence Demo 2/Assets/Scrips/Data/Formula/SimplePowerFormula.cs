// Formula inspired from: http://howtomakeanrpg.com/a/how-to-make-an-rpg-levels.html

using UnityEngine;

namespace Scrips.Data.Formula
{
    [CreateAssetMenu(menuName = "Tower defense kit/Formulas/Simple power")]
    public class SimplePowerFormula : BaseFormula
    {
        public float Exponent;
        public int BaseValue;
        public long Constant;

        public override int GetLevelRequirement(int level)
        {
            return (int) GetLevelRequirementLong(level);
        }

        public override long GetLevelRequirementLong(int level)
        {
            return Mathf.FloorToInt(BaseValue * Mathf.Pow(level, Exponent)) + Constant;
        }
    }
}