using System;
using Scrips;
using UnityEditor;
using UnityEngine;

namespace Editor.EditorToolsMenu
{
    [InitializeOnLoad]
    public class SquareHandle : UnityEditor.Editor
    {
        public static Vector3 CurrentHandlePosition = Vector3.zero;
        public static bool IsMouseInValidArea = false;

        public static Level LevelComponent
        {
            get
            {
                if (_level == null)
                    _level = GameObject.Find("Level").GetComponent<Level>();

                return _level;
            }
        }

        private static Level _level;

        static Vector3 _oldHandlePosition = Vector3.zero;

        static SquareHandle()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
            SceneView.duringSceneGui += OnSceneGUI;
        }

        void OnDestroy()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        static void OnSceneGUI(SceneView sceneView)
        {
            bool isLevelEditorEnabled = EditorPrefs.GetBool("IsLevelEditorEnabled", true);

            if (isLevelEditorEnabled == false)
            {
                return;
            }

            UpdateHandlePosition();
            UpdateIsMouseInValidArea(sceneView.position);
            UpdateRepaint();

            DrawSquareDrawPreview();
        }

        public static bool IsMouseInGameArea()
        {
            return IsMouseInValidArea && !IsOnEdge();
        }

        static bool IsOnEdge()
        {
            return Math.Abs(CurrentHandlePosition.x) >= LevelComponent.Width / 2f ||
                   Math.Abs(CurrentHandlePosition.y) >= LevelComponent.Height / 2f;
        }

        static void UpdateIsMouseInValidArea(Rect sceneViewRect)
        {
            bool isInValidArea = Event.current.mousePosition.y < sceneViewRect.height - 35
                                 && Event.current.mousePosition.x > 80;

            if (isInValidArea != IsMouseInValidArea)
            {
                IsMouseInValidArea = isInValidArea;
                SceneView.RepaintAll();
            }
        }

        static void UpdateHandlePosition()
        {
            if (Event.current == null)
            {
                return;
            }

            Vector2 mousePosition = new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y);

            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("MapEditor")) == true)
            {
                Vector3 offset = Vector3.zero;

                if (EditorPrefs.GetBool("SelectBlockNextToMousePosition", true))
                {
                    offset = hit.normal;
                }

                CurrentHandlePosition.x = Mathf.Floor(hit.point.x - hit.normal.x * 0.001f + offset.x);
                CurrentHandlePosition.y = Mathf.Floor(hit.point.y - hit.normal.y * 0.001f + offset.y);
                CurrentHandlePosition.z = Mathf.Floor(hit.point.z - hit.normal.z * 0.001f + offset.z);

                CurrentHandlePosition += new Vector3(0.5f, 0.5f, 0.5f);
            }
        }

        static void UpdateRepaint()
        {
            if (CurrentHandlePosition != _oldHandlePosition)
            {
                SceneView.RepaintAll();
                _oldHandlePosition = CurrentHandlePosition;
            }
        }

        static void DrawSquareDrawPreview()
        {
            if (!IsMouseInGameArea())
            {
                return;
            }

            Handles.color = new Color(EditorPrefs.GetFloat("CubeHandleColorR", 1f), EditorPrefs.GetFloat("CubeHandleColorG", 1f), EditorPrefs.GetFloat("CubeHandleColorB", 0f));

            DrawHandlesSquare(CurrentHandlePosition);
        }

        static void DrawHandlesSquare(Vector3 center)
        {
            Vector3 p1 = center + Vector3.up * 0.5f + Vector3.right * 0.5f - Vector3.forward * 0.5f;
            Vector3 p2 = center + Vector3.up * 0.5f - Vector3.right * 0.5f - Vector3.forward * 0.5f;
            Vector3 p3 = center - Vector3.up * 0.5f + Vector3.right * 0.5f - Vector3.forward * 0.5f;
            Vector3 p4 = center - Vector3.up * 0.5f - Vector3.right * 0.5f - Vector3.forward * 0.5f;

            Handles.DrawLine(p2, p1);
            Handles.DrawLine(p4, p3);
            Handles.DrawLine(p1, p3);
            Handles.DrawLine(p2, p4);
        }
    }
}

