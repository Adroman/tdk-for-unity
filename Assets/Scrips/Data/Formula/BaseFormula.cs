using UnityEngine;

namespace Scrips.Data.Formula
{
    public abstract class BaseFormula : ScriptableObject
    {
        public virtual int GetLevelRequirement(int level) => (int)GetLevelRequirementLong(level);

        public abstract long GetLevelRequirementLong(int level);
        
        public virtual int GetNextLevelRequirement(long previousValue) => (int)GetNextLevelRequirementLong(previousValue);
        
        public abstract long GetNextLevelRequirementLong(long previousValue);
    }
}