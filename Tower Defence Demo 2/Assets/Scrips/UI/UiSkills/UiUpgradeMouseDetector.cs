using System.Text;

namespace Scrips.UI.UiSkills
{
    public class UiUpgradeMouseDetector : UiMouseDetector
    {
        private readonly StringBuilder _builder = new StringBuilder();

        protected override string BuildErrorText()
        {
            var player = PlayerData.ActivePlayer;
            int skillPointsRequired = Skill.SkillToUse.LevelCostFormula.GetLevelRequirement(Skill.PlayerSkill.Level + 1);

            _builder.Clear();

            if (player.PlayerLevel < Skill.SkillToUse.PlayerLevelRequirement)
            {
                // player level not met
                _builder.AppendLine($"Level {Skill.SkillToUse.PlayerLevelRequirement} required.");
            }
            else if (!Skill.SkillToUse.MeetsTheRequirements(player))
            {
                // skill requirements are not met
                _builder.AppendLine("Skills required:");
                foreach (var requirement in Skill.SkillToUse.GetTheRequirements(player))
                {
                    _builder.AppendLine($"{requirement.RequiredSkill.Name}: {requirement.RequiredLevel}");
                }
            }
            else if (Skill.PlayerSkill.Level == Skill.SkillToUse.MaxLevel)
            {
                // maximum level
                _builder.Append("Maximum level reached");
            }
            else if (player.SkillPoints.Amount < skillPointsRequired)
            {
                // not enough skill points
                _builder.Append($"Not enough skill points. {skillPointsRequired} required.");
            }

            return _builder.ToString();
        }
    }
}