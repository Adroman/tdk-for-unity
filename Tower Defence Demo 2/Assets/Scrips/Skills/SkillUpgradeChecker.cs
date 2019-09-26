using Scrips.UI.UiSkills;
using UnityEngine;

namespace Scrips.Skills
{
    public class SkillUpgradeChecker : MonoBehaviour
    {
        public void RefreshRequirements(PlayerData p)
        {
            foreach (var skill in GetComponentsInChildren<UiSkill>())
            {
                skill.CheckUpgradeButton(p);
                skill.CheckDowngradeButton();
                skill.UpdateTexts();
            }
        }
    }
}