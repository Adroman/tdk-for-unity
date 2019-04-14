using Scrips.CustomTypes;
using Scrips.Modifiers;
using Scrips.Variables;

namespace Scrips.Objectives
{
    public class IntVariableObjective : BaseObjective
    {
        public IntVariable VariableToCheck;

        public int Threshold;
        public ComparisonType Comparison;
        
        protected override bool IsCompleted()
        {
            switch (Comparison)
            {
                case ComparisonType.Equal:
                    return VariableToCheck.Value == Threshold;
                case ComparisonType.NotEqual:
                    return VariableToCheck.Value != Threshold;
                case ComparisonType.LessThan:
                    return VariableToCheck.Value < Threshold;
                case ComparisonType.MoreThan:
                    return VariableToCheck.Value > Threshold;
                case ComparisonType.LessOrEqual:
                    return VariableToCheck.Value <= Threshold;
                case ComparisonType.MoreOrEqual:
                    return VariableToCheck.Value >= Threshold;
                default:
                    return false;
            }
        }
    }
}