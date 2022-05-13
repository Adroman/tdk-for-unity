using Scrips.Modifiers;
using Scrips.UI;
using UnityEngine;

namespace Scrips.LevelData.ExtraSteps
{
    [RequireComponent(typeof(UiSpellButton))]
    public class SpellLinker : PostImportComponent
    {
        protected override void PerformPostImport()
        {
            Debug.Log("Linking Modifier controller to IU Spell Button");
            
            var uiSpellButton = GetComponent<UiSpellButton>();

            if (uiSpellButton == null)
            {
                Debug.LogError("IU Spell Button not found.");
                return;
            }
            
            var modifierController = FindObjectOfType<ModifierController>();

            if (modifierController == null)
            {
                Debug.LogError("Modifier controller not found.");
                return;
            }

            uiSpellButton.ModifierController = modifierController;
        }

        protected override MonoBehaviour GetTargetComponent()
        {
            return GetComponent<UiSpellButton>();
        }
    }
}