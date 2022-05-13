using Scrips.UI;
using UnityEngine;

namespace Scrips.LevelData.ExtraSteps
{
    [RequireComponent(typeof(ScoreManager))]
    public class UILinker : PostImportComponent
    {
        protected override void PerformPostImport()
        {
            Debug.Log("Linking UI to Score Manager");
            
            var scoreManager = GetComponent<ScoreManager>();

            if (scoreManager == null)
            {
                Debug.LogError("Score Manager not found.");
                return;
            }
            
            var uiController = FindObjectOfType<UiController>();

            if (uiController == null)
            {
                Debug.LogError("UI Controller not found.");
                return;
            }

            scoreManager.Ui = uiController;
        }

        protected override MonoBehaviour GetTargetComponent()
        {
            return GetComponent<ScoreManager>();
        }
    }
}