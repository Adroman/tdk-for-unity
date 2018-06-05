using Scrips;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(TdTile))]
    public class TileInspector : UnityEditor.Editor
    {
        private TdTile _tdTile;

        public override void OnInspectorGUI()
        {
            _tdTile = (TdTile)target;

            base.OnInspectorGUI();
            DrawToggles();
        }

        private void DrawToggles()
        {
            bool oldBuildable = _tdTile.Buildable;
            bool oldWalkable = _tdTile.Walkable;
            bool oldIsSpawnpoint = _tdTile.IsSpawnpoint;
            bool oldIsGoal = _tdTile.IsGoal;

            bool newBuildable = EditorGUILayout.Toggle("Buildable", oldBuildable);
            bool newWalkable = EditorGUILayout.Toggle("Walkable", oldWalkable);
            bool newIsSpawnpoint = EditorGUILayout.Toggle("Spawnpoint", oldIsSpawnpoint);
            bool newIsGoal = EditorGUILayout.Toggle("Goal", oldIsGoal);

            if (oldBuildable != newBuildable)
            {
                Undo.RecordObject(_tdTile, "Change Tile properties");
                _tdTile.Buildable = newBuildable;
                EditorUtility.SetDirty(_tdTile);
            }

            if (oldWalkable != newWalkable)
            {
                Undo.RecordObject(_tdTile, "Change Tile properties");
                _tdTile.Walkable = newWalkable;
                EditorUtility.SetDirty(_tdTile);
            }

            if (oldIsSpawnpoint != newIsSpawnpoint)
            {
                Undo.RecordObject(_tdTile, "Change Tile properties");
                _tdTile.IsSpawnpoint = newIsSpawnpoint;
                EditorUtility.SetDirty(_tdTile);
            }

            if (oldIsGoal != newIsGoal)
            {
                Undo.RecordObject(_tdTile, "Change Tile properties");
                _tdTile.IsGoal = newIsGoal;
                EditorUtility.SetDirty(_tdTile);
            }
        }
    }
}