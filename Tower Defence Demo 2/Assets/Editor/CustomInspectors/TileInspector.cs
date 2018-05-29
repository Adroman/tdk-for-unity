using Data;
using Scrips;
using Scrips.Data;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(TdTile))]
    public class TileInspector : UnityEditor.Editor
    {
        TdTile _tdTile;

        public override void OnInspectorGUI()
        {
            _tdTile = (TdTile)target;

            base.OnInspectorGUI();

            DrawWaypointTypes();
            DrawAddButton();
        }

        public void DrawWaypointTypes()
        {
            int waypointTypesCount = _tdTile.WaypointTypesSupported.Count;
            int tileTypesCount = _tdTile.WaypointTypes.Count;
            while (waypointTypesCount < tileTypesCount)
            {
                _tdTile.WaypointTypesSupported.Add(null);
                waypointTypesCount++;
            }
            while (tileTypesCount < waypointTypesCount)
            {
                _tdTile.WaypointTypes.Add(TileType.Normal);
                tileTypesCount++;
            }
            EditorGUILayout.LabelField("Waypoints", EditorStyles.boldLabel);
            for(int i = 0; i < tileTypesCount; i++)
            {
                DrawWaypointType(i);
            }
        }

        public void DrawWaypointType(int i)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(10);
                EditorGUILayout.LabelField("Waypoint type: ");

                var obj = (GameObject)EditorGUILayout.ObjectField(_tdTile.WaypointTypesSupported[i], typeof(GameObject), false);
                if (obj != null && obj.GetComponent<WaypointType>() != null)
                {
                    _tdTile.WaypointTypesSupported[i] = obj;
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Tile type: ");

                var tileType = (TileType)EditorGUILayout.EnumPopup(_tdTile.WaypointTypes[i]);
                _tdTile.WaypointTypes[i] = tileType;
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Delete"))
            {
                _tdTile.WaypointTypesSupported.RemoveAt(i);
                _tdTile.WaypointTypes.RemoveAt(i);
            }
        }

        public void DrawAddButton()
        {
            GUILayout.Space(15);
            if (GUILayout.Button("Add waypoint type"))
            {
                _tdTile.WaypointTypesSupported.Add(null);
                _tdTile.WaypointTypes.Add(TileType.Normal);
            }
        }
    }
}