using System.Collections.Generic;
using Scrips;
using Scrips.Data;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(WaypointType))]
    public class WaypointTypeCustomInspector : UnityEditor.Editor
    {
        WaypointType _waypointType;


        public override void OnInspectorGUI()
        {
            _waypointType = (WaypointType)target;
            DrawDirections();
            DrawNewDirectionButton();
        }

        private void DrawDirections()
        {
            var dirs = _waypointType.PossibleDirections;
            if (dirs == null) dirs = new List<Direction>();
            for (int i = 0; i < dirs.Count; i++)
            {
                EditorGUILayout.LabelField("Direction: ", EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Right: ", GUILayout.Width(70));
                    int newRight = EditorGUILayout.IntField(dirs[i].x);
                    EditorGUILayout.LabelField("Up: ", GUILayout.Width(70));
                    int newUp = EditorGUILayout.IntField(dirs[i].y);

                    // if data are changed
                    if (newRight != dirs[i].x || newUp != dirs[i].y)
                    {
                        // first check, if those data are somewhere stored
                        //if (_waypointType.directions.Count(d => d.direction.x == newRight && d.direction.y == newUp) == 0)
                        //{
                        // then change the data
                        dirs[i].x = newRight;
                        dirs[i].y = newUp;
                        //}
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField("Distance: ", GUILayout.Width(70));
                    float newDistance = EditorGUILayout.FloatField(dirs[i].distance);

                    if (newDistance > 0)
                    {
                        dirs[i].distance = newDistance;
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Remove direction"))
                {
                    Undo.RecordObject(_waypointType, "Delete direction");
                    _waypointType.PossibleDirections.RemoveAt(i);
                    EditorUtility.SetDirty(_waypointType);
                }
            }
        }

        private void DrawNewDirectionButton()
        {
            if(GUILayout.Button("Add new direction", GUILayout.Height(20)))
            {
                Undo.RecordObject(_waypointType, "Add direction");
                _waypointType.PossibleDirections.Add(new Direction { x = 0, y = 0, distance = 0f });
                EditorUtility.SetDirty(_waypointType);
            }
        }
    }
}
