using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Data.Formula;
using Scrips.Modifiers;
using UnityEngine;

namespace Scrips.Skills
{
    [CreateAssetMenu(menuName = "Tower defense kit/Skill")]
    public class Skill : ScriptableObject
    {
        public string Name;

        [Multiline]
        public string DescriptionFormat;

        public bool DisplayNegatedAmount;

        public bool DisplayMultipliedBy100;

        public SkillDependency[] SkillRequirements;

        public int PlayerLevelRequirement;

        public AppliedModifier[] Modifiers;

        public BaseFormula LevelCostFormula;

        public int MaxLevel;

        public bool MeetsTheRequirements(PlayerData player)
        {
            return SkillRequirements.All(r => r.MeetsTheRequirement(player));
        }

        public IEnumerable<SkillDependency> GetTheRequirements(PlayerData player)
        {
            return SkillRequirements.Where(r => !r.MeetsTheRequirement(player));
        }
    }

    [Serializable]
    public class SkillDependency
    {
        public Skill RequiredSkill;
        public int RequiredLevel;

        public bool MeetsTheRequirement(PlayerData player)
        {
            return player.Skills.Any(s => s.Skill == RequiredSkill && s.Level >= RequiredLevel);
        }
    }

    [Serializable]
    public class AppliedModifier
    {
        public BaseModifier Modifier;
        public int PerLevelsApplied;
    }
}