using UnityEditor;
using UnityEngine;

namespace Utils
{
    public static class BulkRevertToPrefab
    {
        [MenuItem("Tools/Revert to Prefab")]
        public static void Revert()
        {
            var selection = Selection.gameObjects;

            if (selection.Length > 0)
            {
                foreach (var t in selection)
                {
                    PrefabUtility.RevertPrefabInstance(t, InteractionMode.UserAction);
                }
            }
            else
            {
                Debug.Log("Cannot revert to prefab - nothing selected");
            }
        }
    }
}