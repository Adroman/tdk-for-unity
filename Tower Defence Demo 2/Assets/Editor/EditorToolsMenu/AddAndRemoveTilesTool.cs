using System;
using System.Linq;
using Data;
using Scrips;
using Scrips.Data;
using UnityEditor;
using UnityEngine;

namespace Editor.EditorToolsMenu
{
    [InitializeOnLoad]
    public class AddAndRemoveTilesTool : UnityEditor.Editor
    {
        static GameObject _level;

        static TileDatabase _tileDatabase;

        static Vector2 _scrollPosition;

        public static GameObject LevelProperty
        {
            get
            {
                if (_level == null)
                {
                    var level = GameObject.Find("Level");

                    if (level != null)
                    {
                        _level = level;
                    }
                }

                return _level;
            }
        }

        public static int SelectedTile
        {
            get
            {
                return EditorPrefs.GetInt("EditorSelectedTile", 0);
            }
            set
            {
                EditorPrefs.SetInt("EditorSelectedTile", value);
            }
        }

        private static TileDatabase TileDatabase
        {
            get
            {
                if (_tileDatabase == null)
                {
                    _tileDatabase = AssetDatabase.LoadAssetAtPath<TileDatabase>("Assets/Database/TileDatabase.asset");
                }

                return _tileDatabase;
            }
        }

        static AddAndRemoveTilesTool()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
            SceneView.onSceneGUIDelegate += OnSceneGUI;
        }

        void OnDestroy()
        {
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
        }

        static void OnSceneGUI(SceneView sceneView)
        {

            if (ToolsMenu.SelectedTool == 0)
            {
                return;
            }

            int controlId = GUIUtility.GetControlID(FocusType.Passive);

            if (Event.current.type == EventType.MouseDown &&
                Event.current.button == 0 &&
                Event.current.alt == false &&
                Event.current.shift == false &&
                Event.current.control == false)
            {
                if (SquareHandle.IsMouseInGameArea())
                {
                    if (ToolsMenu.SelectedTool == 1)
                    {
                        RemoveTile(SquareHandle.CurrentHandlePosition);
                    }

                    if (ToolsMenu.SelectedTool == 2)
                    {
                        AddTile(SquareHandle.CurrentHandlePosition);
                    }
                }
            }

            if (Event.current.type == EventType.KeyDown &&
                Event.current.keyCode == KeyCode.Escape)
            {
                ToolsMenu.SelectedTool = 0;
            }

            HandleUtility.AddDefaultControl(controlId);
            DrawTileButtons(sceneView);
        }

        private static void DrawTileButtons(SceneView sceneView)
        {
            Handles.BeginGUI();

            GUI.Box(new Rect(0, 0, 80, sceneView.position.height - 35), GUIContent.none, EditorStyles.textArea);

            if (TileDatabase?.Tiles != null)
            {
                _scrollPosition = GUI.BeginScrollView(new Rect(0, 0, 95, sceneView.position.height - 35), _scrollPosition,
                    new Rect(0, 0, 80, TileDatabase.Tiles.Count * 100));
                for (int i = 0; i < TileDatabase.Tiles.Count; ++i)
                {
                    DrawButton(i, sceneView.position);
                }

                GUI.EndScrollView();
            }

            Handles.EndGUI();
        }

        private static void DrawButton(int index, Rect rect)
        {
            bool isActive = ToolsMenu.SelectedTool == 2 && index == SelectedTile;

            var image = AssetPreview.GetAssetPreview(TileDatabase.Tiles[index].Prefab);
            var content = new GUIContent(image);

            bool isPressed = GUI.Toggle(new Rect(5, index * 100 + 5, 70, 70), isActive, content, GUI.skin.button);
            var oldColor = GUI.color;
            GUI.color = Color.black;
            GUI.Label(new Rect(5, index * 100 + 75, 70, 30), TileDatabase.Tiles[index].Name);
            GUI.color = oldColor;

            if (isPressed == true && isActive == false)
            {
                SelectedTile = index;
                ToolsMenu.SelectedTool = 2;
                LevelProperty.GetComponent<Level>().TilePrefab = TileDatabase.Tiles[index].Prefab;
            }
        }

        public static void AddTile(Vector2 position)
        {
            var levelComponent = LevelProperty.GetComponent<Level>();

            float minX = - (levelComponent.Width - 1) / 2f;
            float minY = - (levelComponent.Height - 1) / 2f;

            int actualX = Mathf.FloorToInt(position.x - minX);
            int actualY = Mathf.FloorToInt(position.y - minY);

            var newTile = (GameObject)PrefabUtility.InstantiatePrefab(LevelProperty.GetComponent<Level>().TilePrefab);
            newTile.transform.parent = LevelProperty.transform.Find("Tiles");

            newTile.transform.position = new Vector3(position.x, position.y, 1);
            //newTile.transform.localScale = new Vector3(7, 7, 1);

            if (levelComponent[actualX, actualY] != null)
            {
                RemoveTile(levelComponent[actualX, actualY].transform.position);
            }

            levelComponent[actualX, actualY] = newTile;

            CreateSpecialPoints(newTile);

            Undo.RegisterCreatedObjectUndo(newTile, "Add Tile");
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }

        private static void CreateSpecialPoints(GameObject tile)
        {
            var t = tile.GetComponent<TdTile>();
            if (t == null) return;

            for(int i = 0; i < t.WaypointTypesSupported.Count; i++)
            {
                if(t.WaypointTypes[i] == TileType.SpawnPoint)
                {
                    var sp = new GameObject(t.WaypointTypesSupported[i].name + " SpawnPoint");
                    sp.transform.position = t.transform.position;
                    var s = sp.AddComponent<Spawnpoint>();
                    s.WaypointType = t.WaypointTypesSupported[i];
                    sp.transform.parent = GameObject.Find("SpawnPoints").transform;
                }
                else if(t.WaypointTypes[i] == TileType.Goal)
                {
                    var goal = new GameObject(t.WaypointTypesSupported[i].name + " Goal");
                    goal.transform.position = t.transform.position;
                    var g = goal.AddComponent<Goal>();
                    g.WaypointType = t.WaypointTypesSupported[i];
                    goal.transform.parent = GameObject.Find("Goals").transform;
                }
            }
        }

        public static void RemoveTile(Vector2 position)
        {
            var levelComponent = LevelProperty.GetComponent<Level>();

            var minX = -(levelComponent.Width - 1) / 2f;
            var minY = -(levelComponent.Height - 1) / 2f;

            var actualX = Mathf.FloorToInt(position.x - minX);
            var actualY = Mathf.FloorToInt(position.y - minY);

            if (levelComponent[actualX, actualY] != null)
            {

                var objectToDestroy = levelComponent[actualX, actualY];
                levelComponent[actualX, actualY] = null;


                if (objectToDestroy.GetComponent<TdTile>().WaypointTypes.Any(wt => wt == TileType.SpawnPoint))
                {
                    // Destroy all spawnpoints on that position
                    var spawnpoints = GameObject.Find("SpawnPoints").transform;
                    for (int i = 0; i < spawnpoints.childCount; i++)
                    {
                        var sp = spawnpoints.GetChild(i);
                        if (Math.Abs(sp.position.x - objectToDestroy.transform.position.x) < 0.01f &&
                            Math.Abs(sp.position.y - objectToDestroy.transform.position.y) < 0.01f)
                        {
                            DestroyImmediate(sp.gameObject);
                            i--;
                        }
                    }
                }

                if (objectToDestroy.GetComponent<TdTile>().WaypointTypes.Any(wt => wt == TileType.Goal))
                {
                    // Destroy all spawnpoints on that position
                    var goals = GameObject.Find("Goals").transform;
                    for (int i = 0; i < goals.childCount; i++)
                    {
                        var g = goals.GetChild(i);
                        if (Math.Abs(g.position.x - objectToDestroy.transform.position.x) < 0.01f &&
                            Math.Abs(g.position.y - objectToDestroy.transform.position.y) < 0.01f)
                        {
                            DestroyImmediate(g.gameObject);
                            i--;
                        }
                    }
                }

                Undo.DestroyObjectImmediate(objectToDestroy);
                DestroyImmediate(objectToDestroy);
                UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
            }
        }
    }
}

