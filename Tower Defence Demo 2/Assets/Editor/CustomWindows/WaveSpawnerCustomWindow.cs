using Scrips.Waves;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomWindows
{
    class WaveSpawnerCustomWindow : EditorWindow
    {
        //[MenuItem("Editors/Wave Editor")]
        public static WaveSpawnerCustomWindow Init()
        {
            return EditorWindow.GetWindow<WaveSpawnerCustomWindow>(false, "Wave editor");
        }

        private WaveSpawner _spawner;

        public static void PopUp(WaveSpawner spawner)
        {
            var window = Init();
            window._spawner = spawner;
        }
        private void OnGUI()
        {
            DrawBasicProperties();
            DrawSpawnPoints();
            DrawSpawns();
        }

        private void DrawBasicProperties()
        {
            GUILayout.BeginHorizontal();

            GUILayout.Label("Countdown: ");
            float newCountDown = EditorGUILayout.FloatField(_spawner.Countdown);
            if (newCountDown < 0) newCountDown = 0;
            _spawner.Countdown = newCountDown;

            GUILayout.EndHorizontal();

        }

        private void DrawSpawnPoints()
        {
            GUILayout.Space(10);
            GUILayout.Label("Spawn points:", EditorStyles.boldLabel);

            for (int i = 0; i < _spawner.SpawnPoints.Count; i++)
            {
                DrawSpawnPoint(i);
            }

            DrawSpawnpointButton();
        }

        private void DrawSpawnPoint(int index)
        {
            if (index < 0 || index > _spawner.SpawnPoints.Count)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Spawnpoint Prefab:");
            var newPrefab = EditorGUILayout.ObjectField(_spawner.SpawnPoints[index], typeof(GameObject), false);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_spawner, "Modify spawn point");

                if (newPrefab.GetType() == typeof(GameObject)) // We need to somehow recognize type of this prefab
                {
                    _spawner.SpawnPoints[index] = (GameObject)newPrefab;
                }
                else
                {
                    //_spawner.Spawns[index].Prefab = null;
                }

                EditorUtility.SetDirty(_spawner);
            }

            if (GUILayout.Button("Remove"))
            {
                Undo.RecordObject(_spawner, "Delete spawn point");
                _spawner.SpawnPoints.RemoveAt(index);
                EditorUtility.SetDirty(_spawner);
            }

            GUILayout.EndHorizontal();
        }

        private void DrawSpawnpointButton()
        {
            if (GUILayout.Button("Add spawn point", GUILayout.Height(30)))
            {
                Undo.RecordObject(_spawner, "Add spawnpoint");
                _spawner.SpawnPoints.Add(null);
                EditorUtility.SetDirty(_spawner);
            }
        }

        private void DrawSpawns()
        {
            GUILayout.Space(10);
            GUILayout.Label("Enemies to spawn: ", EditorStyles.boldLabel);

            for (int i = 0; i < _spawner.Spawns.Count; i++)
            {
                DrawSpawn(i);
            }

            DrawSpawnButton();
        }

        private void DrawSpawn(int index)
        {
            if (index < 0 || index > _spawner.Spawns.Count)
            {
                return;
            }

            EditorGUI.BeginChangeCheck();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Enemy Prefab:");
            var newPrefab = EditorGUILayout.ObjectField(_spawner.Spawns[index].Prefab, typeof(GameObject), false);

            GUILayout.Label("Amount:");
            int newAmount = EditorGUILayout.IntField(_spawner.Spawns[index].Amount);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            GUILayout.Label("Initial Countdown:");
            float newInitCountDown = EditorGUILayout.FloatField(_spawner.Spawns[index].InitialCountDown);

            GUILayout.Label("Countdown between two spawns:");
            float newBetweenCountDown = EditorGUILayout.FloatField(_spawner.Spawns[index].Interval);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_spawner, "Modify object");

                _spawner.Spawns[index].Amount = newAmount;
                _spawner.Spawns[index].InitialCountDown = newInitCountDown;
                _spawner.Spawns[index].Interval = newBetweenCountDown;
                if (newPrefab.GetType() == typeof(GameObject) && ((GameObject)newPrefab).GetComponent<Scrips.EnemyData.Instances.EnemyInstance>() != null)
                {
                    _spawner.Spawns[index].Prefab = (GameObject)newPrefab;
                }
                else
                {
                    //_spawner.Spawns[index].Prefab = null;
                }

                EditorUtility.SetDirty(_spawner);
            }

            //bool newValue = _spawner.Spawns[index].SpawnWithPreviousCluster;
            //EditorGUILayout.Toggle(newValue, "Spawn with previous cluster");

            if (GUILayout.Button("Remove cluster"))
            {
                Undo.RecordObject(_spawner, "Delete object");
                _spawner.Spawns.RemoveAt(index);
                EditorUtility.SetDirty(_spawner);
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(5);
        }

        private void DrawSpawnButton()
        {
            if (GUILayout.Button("Add new cluster", GUILayout.Height(30)))
            {
                Undo.RecordObject(_spawner, "Add spawn");
                _spawner.Spawns.Add(new WaveCluster());
                EditorUtility.SetDirty(_spawner);
            }
        }

        private void DrawLeftPanel()
        {
            EditorGUILayout.BeginScrollView(new Vector2(0, 0), GUILayout.Width(200));

            var targets = LoadWaves();
            foreach(var target in targets)
            {
                EditorGUILayout.LabelField(target.name);
            }

            EditorGUILayout.EndScrollView();

        }

        private WaveSpawner[] LoadWaves()
        {
            return GameObject.FindObjectOfType<WaveController>().GetComponentsInChildren<WaveSpawner>();
        }
    }
}

