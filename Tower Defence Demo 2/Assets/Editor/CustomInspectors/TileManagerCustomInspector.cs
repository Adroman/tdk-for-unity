using Scrips;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(TileManager))]
    public class TileManagerCustomInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var manager = (TileManager)target;

            if (GUILayout.Button("Validate tiles"))
            {
                manager.ValidateTiles();
                Debug.Log("Tile validation completed.");
            }

            if (GUILayout.Button("Fill empty tiles"))
            {
                var filledTiles = manager.FillTiles();
                Debug.Log($"{filledTiles} tile(s) have been filled with the default tile.");
                if (filledTiles > 0)
                {
                    EditorSceneManager.MarkSceneDirty(manager.gameObject.scene);
                    Undo.RecordObject(manager, "Fill empty tiles");
                }
            }

            if (GUILayout.Button("Trim tiles"))
            {
                var trimmedTiles = manager.TrimTiles();
                Debug.Log($"{trimmedTiles} tile(s) have been trimmed.");
                if (trimmedTiles > 0)
                {
                    EditorSceneManager.MarkSceneDirty(manager.gameObject.scene);
                    Undo.RecordObject(manager, "Trim excess tiles");
                }
            }
        }
    }
}