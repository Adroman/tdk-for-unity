using UnityEditor;
using UnityEngine;

namespace Editor.EditorToolsMenu
{
    [InitializeOnLoad]
    public class ToolsMenu : UnityEditor.Editor
    {


        public static int SelectedTool
        {
            get
            {
                return EditorPrefs.GetInt("SelectedEditorTool", 0);
            }
            set
            {
                if (value == SelectedTool) return;

                EditorPrefs.SetInt("SelectedEditorTool", value);

                switch (value)
                {
                    case 0:
                        EditorPrefs.SetBool("IsLevelEditorEnabled", false);

                        Tools.hidden = false;
                        break;
                    case 1:
                        EditorPrefs.SetBool("IsLevelEditorEnabled", true);

                        Tools.hidden = false;
                        break;
                    case 2:
                        EditorPrefs.SetBool("IsLevelEditorEnabled", true);

                        Tools.hidden = false;
                        break;
                }
            }
        }

        static ToolsMenu()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            SceneView.onSceneGUIDelegate += OnSceneGUI;

            EditorApplication.hierarchyChanged -= OnSceneChanged;
            EditorApplication.hierarchyChanged += OnSceneChanged;
        }

        void Destroy()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;

            EditorApplication.hierarchyChanged -= OnSceneChanged;
        }

        static void OnSceneChanged()
        {
            Tools.hidden = ToolsMenu.SelectedTool != 0;

        }

        static void OnSceneGUI(SceneView sceneView)
        {
            DrawToolsMenu(sceneView.position);
        }

        static void DrawToolsMenu(Rect position)
        {
            Handles.BeginGUI();

            GUILayout.BeginArea(new Rect(0, position.height - 35, position.width, 20), EditorStyles.toolbar);
            {
                string[] buttonLabels = new string[] { "None", "Delete Tile", "New Tile" };

                SelectedTool = GUILayout.SelectionGrid(
                    SelectedTool,
                    buttonLabels,
                    3,
                    EditorStyles.toolbarButton,
                    GUILayout.Width(300));
            }
            GUILayout.EndArea();

            Handles.EndGUI();
        }
    }
}

