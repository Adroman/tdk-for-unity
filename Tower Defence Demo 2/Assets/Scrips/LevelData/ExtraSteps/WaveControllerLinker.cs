using Scrips.Modifiers;
using Scrips.UI;
using Scrips.Waves;
using UnityEngine;

namespace Scrips.LevelData.ExtraSteps
{
    [RequireComponent(typeof(WaveController))]
    public class WaveControllerLinker : PostImportComponent
    {
        protected override void PerformPostImport()
        {
            Debug.Log("Linking Modifier controller and UI Wave Queue to Wave controller");
            
            var waveController = GetComponent<WaveController>();

            if (waveController == null)
            {
                Debug.LogError("Wave controller not found.");
                return;
            }
            
            var modifierController = FindObjectOfType<ModifierController>();

            if (modifierController == null)
            {
                Debug.LogError("Modifier controller not found.");
                return;
            }

            var uiWaves = FindObjectOfType<UiWaveQueue>();

            if (uiWaves == null)
            {
                Debug.LogError("UI Wave Queue not found.");
                return;
            }

            waveController.UiWaves = uiWaves;
            waveController.ModifierController = modifierController;
        }

        protected override MonoBehaviour GetTargetComponent()
        {
            return GetComponent<WaveController>();
        }
    }
}