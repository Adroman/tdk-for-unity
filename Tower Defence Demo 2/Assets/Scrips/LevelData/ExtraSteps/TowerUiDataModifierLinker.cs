using Scrips.Modifiers;
using Scrips.Towers.BaseData;
using UnityEngine;

namespace Scrips.LevelData.ExtraSteps
{
    [RequireComponent(typeof(TowerUiData))]
    public class TowerUiDataModifierLinker : PostImportComponent
    {
        protected override void PerformPostImport()
        {
            Debug.Log($"Linking Modifier controller to Tower UI Data.");

            var uiData = GetComponent<TowerUiData>();

            var modifierController = FindObjectOfType<ModifierController>();

            if (modifierController == null)
            {
                Debug.LogError("ModifierController not found.");
                return;
            }

            uiData.ModifierController = modifierController;
        }

        protected override MonoBehaviour GetTargetComponent()
        {
            return GetComponent<TowerUiData>();
        }
    }
}