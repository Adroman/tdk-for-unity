using Scrips;
using UnityEditor;

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
            _tdTile.Buildable = EditorGUILayout.Toggle("Buildable", _tdTile.Buildable);
            _tdTile.Walkable = EditorGUILayout.Toggle("Walkable", _tdTile.Walkable);
            _tdTile.IsSpawnpoint = EditorGUILayout.Toggle("Spawnpoint", _tdTile.IsSpawnpoint);
            _tdTile.IsGoal = EditorGUILayout.Toggle("Goal", _tdTile.IsGoal);
        }
    }
}