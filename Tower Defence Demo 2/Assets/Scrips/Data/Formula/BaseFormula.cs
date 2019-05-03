using UnityEngine;

namespace Scrips.Data.Formula
{
    public abstract class BaseFormula : ScriptableObject
    {
        public abstract int GetLevelRequirement(int level);

        public abstract long GetLevelRequirementLong(int level);
    }
}