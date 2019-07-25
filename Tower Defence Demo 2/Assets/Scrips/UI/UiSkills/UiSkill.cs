using System.Globalization;
using System.Linq;
using Scrips.Skills;
using Scrips.UI.UiTextDisplay;
using UnityEngine;
using UnityEngine.UI;

namespace Scrips.UI.UiSkills
{
    public class UiSkill : MonoBehaviour
    {
        public Skill SkillToUse;

        public BaseUiTextDisplay SkillNameDisplay;

        public BaseUiTextDisplay SkillDescriptionDisplay;

        public Color SkillDescriptionDisplayColor;

        public BaseUiTextDisplay SkillLevelDisplay;

        public Button UpgradeButton;

        public Button DowngradeButton;

        [HideInInspector]
        public PlayerSkill PlayerSkill;

        public void Start()
        {
            var pd = PlayerData.ActivePlayer;

            ImportPlayerSkills(pd);

            UpdateTexts();
            CheckUpgradeButton(pd);
            CheckDowngradeButton();
        }

        public void UpdateTexts()
        {
            SkillNameDisplay.Display(SkillToUse.Name);
            SkillDescriptionDisplay.SetColor(SkillDescriptionDisplayColor);
            SkillDescriptionDisplay.DisplayFormat(SkillToUse.DescriptionFormat,
                SkillToUse.Modifiers
                    .Select(m => (m.Modifier.Amount * (PlayerSkill.Level / m.PerLevelsApplied)
                                  * (SkillToUse.DisplayMultipliedBy100 ? 100 : 1)
                                  * (SkillToUse.DisplayNegatedAmount ? -1 : 1))
                        .ToString(CultureInfo.InvariantCulture))
                    .Cast<object>().ToArray());
            SkillLevelDisplay.Display(PlayerSkill.Level.ToString());
        }

        private void ImportPlayerSkills(PlayerData player)
        {
            PlayerSkill = player.Skills.Single(s => s.Skill == SkillToUse);
        }

        public void CheckUpgradeButton(PlayerData player)
        {
            if (player.PlayerLevel < SkillToUse.PlayerLevelRequirement)
            {
                // player level not met
                UpgradeButton.enabled = false;
            }
            else if (!SkillToUse.MeetsTheRequirements(player))
            {
                // other skills are required
                UpgradeButton.enabled = false;
            }
            else if (PlayerSkill.Level == SkillToUse.MaxLevel)
            {
                // maximum level
                UpgradeButton.enabled = false;
            }
            else if (player.SkillPoints.Amount <
                     SkillToUse.LevelCostFormula.GetLevelRequirement(PlayerSkill.Level + 1))
            {
                // not enough skill points
                UpgradeButton.enabled = false;
            }
            else
            {
                UpgradeButton.enabled = true;
            }
        }

        public void CheckDowngradeButton()
        {
            DowngradeButton.enabled = PlayerSkill.Level != 0;
        }

        public void UpgradeSkill(PlayerData pd)
        {
            pd.SkillPoints.Amount -= PlayerSkill.Skill.LevelCostFormula.GetLevelRequirement(++PlayerSkill.Level);
            CheckUpgradeButton(pd);
            CheckDowngradeButton();
            UpdateTexts();
        }

        public void DowngradeSkill(PlayerData pd)
        {
            pd.SkillPoints.Amount += PlayerSkill.Skill.LevelCostFormula.GetLevelRequirement(PlayerSkill.Level--);
            CheckUpgradeButton(pd);
            CheckDowngradeButton();
            UpdateTexts();
        }
    }
}