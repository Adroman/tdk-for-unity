namespace Scrips.UI.UiSkills
{
    public class UiDowngradeMouseDetector : UiMouseDetector
    {
        protected override string BuildErrorText()
        {
            return Skill.PlayerSkill.Level == 0 ? "You cannot go below level 0." : "";
        }
    }
}