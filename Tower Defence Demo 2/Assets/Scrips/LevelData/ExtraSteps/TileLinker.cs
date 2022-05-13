using UnityEngine;

namespace Scrips.LevelData.ExtraSteps
{
    [RequireComponent(typeof(Level))]
    public class TileLinker : PostImportComponent
    {
        protected override void PerformPostImport()
        {
            Debug.Log("Linking Tiles to Level");
            
            var level = GetComponent<Level>();

            if (level == null)
            {
                Debug.LogError("Level not found.");
                return;
            }

            level.ReAssignTiles();
        }

        protected override MonoBehaviour GetTargetComponent()
        {
            return GetComponent<Level>();
        }
    }
}