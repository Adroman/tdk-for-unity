using Scrips.Controls;
using Scrips.Modifiers;
using Scrips.Spells;
using UnityEngine;

namespace Scrips.LevelData.ExtraSteps
{
    [RequireComponent(typeof(LevelMouseControls))]
    public class LevelMouseControlsLinker : PostImportComponent
    {
        protected override void PerformPostImport()
        {
            Debug.Log("Linking stuff to Level Mouse Controls.");

            var mouseControls = GetComponent<LevelMouseControls>();
            if (mouseControls == null)
            {
                Debug.LogError("Level Mouse Controls not found.");
                return;
            }

            var camera = FindObjectOfType<Camera>();
            if (camera == null)
            {
                Debug.LogError("Camera not found.");
                return;
            }

            var modifierController = FindObjectOfType<ModifierController>();
            if (modifierController == null)
            {
                Debug.LogError("Modifier Controller not found.");
                return;
            }

            var spellPointer = FindObjectOfType<SpellSpawner>();
            if (spellPointer == null)
            {
                Debug.LogError("Spell Spawner not found.");
                return;
            }

            var spellCircle = spellPointer.GetComponent<CircleRenderer>();
            if (spellCircle == null)
            {
                Debug.LogError("Spell Circle not found.");
                return;
            }

            mouseControls.Camera = camera;
            mouseControls.SpellCircle = spellCircle;
            mouseControls.SpellSpawner = spellPointer;
            mouseControls.ModifierController = modifierController;
        }

        protected override MonoBehaviour GetTargetComponent()
        {
            return GetComponent<LevelMouseControls>();
        }
    }
}