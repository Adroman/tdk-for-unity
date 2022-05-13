using UnityEditor;
using UnityEngine;

namespace Scrips.LevelData.ExtraSteps
{
    public abstract class PostImportComponent : MonoBehaviour
    {
        protected abstract void PerformPostImport();

        protected abstract MonoBehaviour GetTargetComponent();

        public void HandlePostImport()
        {
            PerformPostImport();
            PrefabUtility.RecordPrefabInstancePropertyModifications(GetTargetComponent());
        }
    }
}