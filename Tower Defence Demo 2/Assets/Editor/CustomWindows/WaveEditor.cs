using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Utils;
using Scrips.EnemyData.AutoGenerateModifers;
using Scrips.EnemyData.Instances;
using Scrips.Waves;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Editor.CustomWindows
{
    public class WaveEditor : EditorWindow
    {
        private WaveController _waveController;
        private Wave _selectedWave = null;
        private List<Vector3> _overrideSpawnpointScrollPositions = new List<Vector3>();

        private Vector2 _waveListScrollPosition;
        private Vector2 _spawnpointListScrollPosition;
        private Vector2 _clusterListScrollPosition;

        private const float MainButtonWidth = 120;
        private const float MainButtonHeight = 50;
        private const float Space = 5;
        private const float MinibuttonSize = 20;
        private const float IconSize = 30;

        private WaveController WaveController
        {
            get
            {
                if (_waveController == null) _waveController = GameObject.Find("Level/Waves").GetComponent<WaveController>();
                return _waveController;
            }
        }

        private WaveGenerator Generator => WaveController.GetComponent<WaveController>().Generator;


        [MenuItem("Tools/Tower defence kit/Wave editor")]
        public static WaveEditor Init()
        {
            return GetWindow<WaveEditor>(false, "Wave editor");
        }

        private void OnGUI()
        {
            minSize = new Vector2(0, 0);
            DrawHeader();
            DrawWaveList(200);
        }

        private void SetDirty(Object obj, string message)
        {
            EditorSceneManager.MarkSceneDirty(_waveController.gameObject.scene);
            Undo.RegisterCompleteObjectUndo(obj, message);
        }

        private void DrawHeader()
        {
            GUI.Label(new Rect(10, Space + 0, 200, 15), "Automatic Wave Generation", EditorStyles.boldLabel);

            GUI.Label(new Rect(10, Space + 20, 150, 15), "Number of waves:");
            GUI.Label(new Rect(10, Space + 40, 150, 15), "Min Clusters:");
            GUI.Label(new Rect(10, Space + 60, 150, 15), "Max Clusters:");
            GUI.Label(new Rect(10, Space + 80, 150, 15), "Base Difficulty:");
            GUI.Label(new Rect(10, Space + 100, 150, 15), "Difficulty increase:");
            GUI.Label(new Rect(10, Space + 120, 150, 15), "Difficulty deviation:");
            GUI.Label(new Rect(10, Space + 140, 150, 15), "Random seed:");

            EditorGUI.BeginChangeCheck();

            int newWaveAmount = EditorGUI.IntField(new Rect(200, Space + 20, 150, 15), Generator.WavesTotal);
            int newMinClusters = EditorGUI.IntField(new Rect(200, Space + 40, 150, 15), Generator.MinClusters);
            int newMaxClusters = EditorGUI.IntField(new Rect(200, Space + 60, 150, 15), Generator.MaxClusters);
            int newBaseDifficulty = EditorGUI.IntField(new Rect(200, Space + 80, 150, 15), Generator.BaseDifficulty);
            float newDifficultyIncrease = EditorGUI.FloatField(new Rect(200, Space + 100, 150, 15), Generator.DifficultyIncrease);
            float newDifficultyDeviation = EditorGUI.Slider(new Rect(200, Space + 120, 150, 15), Generator.DifficultyDeviation, 0, 1);
            string newSeed = EditorGUI.TextField(new Rect(200, Space + 140, 150, 15), Generator.Seed);

            newWaveAmount = Math.Max(1, newWaveAmount);
            newMinClusters = Math.Max(1, newMinClusters);
            newMaxClusters = Math.Max(1, newMaxClusters);
            newBaseDifficulty = Math.Max(1, newBaseDifficulty);
            newDifficultyIncrease = Math.Max(1, newDifficultyIncrease);

            if (EditorGUI.EndChangeCheck())
            {
                Generator.BaseDifficulty = newBaseDifficulty;
                Generator.DifficultyDeviation = newDifficultyDeviation;
                Generator.DifficultyIncrease = newDifficultyIncrease;
                Generator.MinClusters = newMinClusters;
                Generator.MaxClusters = newMaxClusters;
                Generator.Seed = newSeed;
                Generator.WavesTotal = newWaveAmount;
            }

            GameObject go;

            GUI.Label(new Rect(400, 45, 200, 15), "Prefabs to use: ", EditorStyles.boldLabel);

            int prefabToDelete = -1;

            for (int i = 0; i < Generator.Prefabs.Count; i++)
            {
                go = (GameObject)EditorGUI.ObjectField(new Rect(400 + Space + i * 105, 40 + 25, 100, 15), Generator.Prefabs[i].gameObject, typeof(GameObject), false);
                if (CheckPrefab(go))
                {
                    // Replace Prefab
                    Generator.Prefabs[i] = go;
                }
                if (GUI.Button(new Rect(400 + Space + i * 105, 40 + 45, 82, 15), "Delete"))
                {
                    // Delete Prefab
                    prefabToDelete = i;
                }
            }
            go = (GameObject)EditorGUI.ObjectField(new Rect(400 + Space + Generator.Prefabs.Count * 105, 40 + 25, 100, 15), null, typeof(GameObject), false);
            if (CheckPrefab(go))
            {
                // Add Prefab
                Generator.Prefabs.Add(go);
            }
            if (prefabToDelete > -1) Generator.Prefabs.RemoveAt(prefabToDelete);

            if (GUI.Button(new Rect(400 + Space, 40 + 85, 150, 35), "Generate") && Generator.Prefabs.Count > 0)
            {
                GenerateWave();
            }
        }

        private void GenerateWave()
        {
            var wavesParent = WaveController.transform;

            while (wavesParent.childCount > 0)
            {
                DestroyImmediate(wavesParent.GetChild(0).gameObject);
            }

            var newWaves = Generator.GenerateWaves();

            WaveController.Waves = newWaves;

            _selectedWave = null;
        }

        private bool CheckPrefab(GameObject go)
        {
            return go?.GetComponent<EnemyInstance>() != null && go.GetComponent<BaseAutoGenerateModifiers>() != null;
        }

        private void RefreshWaveList()
        {
            WaveController.Waves = WaveController.Waves?.Where(wave => wave != null)?.ToList() ?? new List<Wave>();
        }

        private void DrawWaveList(float baseY)
        {
            RefreshWaveList();

            if (WaveController.Waves == null)
            {
                Debug.LogWarning("Waves not found.");
                WaveController.Waves = new List<Wave>();
            }
            var waves = WaveController.Waves;

            var innerRect = new Rect(0, baseY, ListUtils.GetWaveListWidth(0, MainButtonWidth, MinibuttonSize, Space, 20, false), ListUtils.GetScrollHeight(waves.Count + 1, Space, MainButtonHeight));
            var scrollRect = new Rect(0, baseY, ListUtils.GetWaveListWidth(0, MainButtonWidth, MinibuttonSize, Space, 20, true), position.height - baseY);

            using (new GuiColor(new Color(0.7f, 0.7f, 0.7f))) GUI.Box(new Rect(innerRect.x, innerRect.y, innerRect.width, scrollRect.height), GUIContent.none);

            _waveListScrollPosition = GUI.BeginScrollView(scrollRect, _waveListScrollPosition, innerRect);
            for (int i = 0; i < waves.Count; i++)
            {
                var wave = waves[i];
                DrawWaveHeader(i, wave, baseY, waves.Count);
            }

            if (GUI.Button(new Rect(Space, baseY + waves.Count * 55 + Space, MainButtonWidth, MainButtonHeight), "New wave"))
            {
                AddNewWave();
            }
            GUI.EndScrollView();

            if (!ReferenceEquals(_selectedWave, null))
            {
                DrawWaveInfo(ListUtils.GetWaveListWidth(0, MainButtonWidth, MinibuttonSize, Space, 20, true) + Space, baseY);
            }
        }

        private void DrawWaveHeader(int index, Wave waveInfo, float baseY, int totalWaves)
        {
            var oldColor = GUI.color;
            if (!ReferenceEquals(_selectedWave, null) && ReferenceEquals(_selectedWave, waveInfo))
            {
                GUI.color = Color.cyan;
            }

            if (GUI.Button(new Rect(Space, baseY + index * 55 + Space, MainButtonWidth, MainButtonHeight), "Wave " + (index + 1).ToString()))
            {
                SelectWave(index);
            }

            GUI.color = oldColor;

            if (GUI.Button(new Rect(130, baseY + index * 55 + 6.75f, MinibuttonSize, MinibuttonSize), "+"))
            {
                InsertNewWave(index);
            }

            if (index > 0)
            {
                if (GUI.Button(new Rect(155, baseY + index * 55 + 6.75f, MinibuttonSize, MinibuttonSize), "↑"))
                {
                    SwapWaves(index, index - 1);
                }
            }
            if (index + 1 < totalWaves)
            {
                if (GUI.Button(new Rect(155, baseY + index * 55 + 31.75f, MinibuttonSize, MinibuttonSize), "↓"))
                {
                    SwapWaves(index, index + 1);
                }
            }

            if (GUI.Button(new Rect(130, baseY + index * 55 + 31.75f, MinibuttonSize, MinibuttonSize), "-"))
            {
                DeleteWaveAt(index);
            }
        }

        private void DrawWaveInfo(float baseX, float baseY)
        {
            using (new GuiColor(new Color(0.6f, 0.6f, 0.7f))) GUI.Box(new Rect(baseX, baseY, position.width - baseX, Space + 40), GUIContent.none);

            GUI.Label(new Rect(baseX + Space, baseY + Space, 100, 15), "Countdown:");
            float newCountdown = EditorGUI.FloatField(new Rect(baseX + 110, baseY + Space, 100, 15), _selectedWave.Countdown);
            GUI.Label(new Rect(baseX + Space, baseY + 25, 100, 15), "Random seed:");
            string newSeed = EditorGUI.TextField(new Rect(baseX + 110, baseY + 25, 100, 15), _selectedWave.Seed);

            DrawSpawnPointsForWave(baseX, baseY + 50);
            DrawClustersForWave(baseX, baseY + 135);

            ApplyWaveChanges(_selectedWave, newCountdown, newSeed);
        }

        private void DrawSpawnPointsForWave(float baseX, float baseY)
        {
            using (new GuiColor(new Color(0.6f, 0.7f, 0.6f))) GUI.Box(new Rect(baseX, baseY, position.width - baseX, Space + 60), GUIContent.none);

            GUI.Label(new Rect(baseX + Space, baseY + Space, 100, 15), "Spawnpoints:", EditorStyles.boldLabel);

            GameObject go;
            int spawnpointToDelete = -1;
            _spawnpointListScrollPosition = GUI.BeginScrollView(new Rect(baseX, baseY, position.width - baseX, Space + 75), _spawnpointListScrollPosition, new Rect(baseX, baseY, Space + (_selectedWave.SpawnPoints.Count + 1) * 105, Space + 40));
            for (int i = 0; i < _selectedWave.SpawnPoints.Count; i++)
            {
                go = (GameObject)EditorGUI.ObjectField(new Rect(baseX + Space + i * 105, baseY + 25, 100, 15), _selectedWave.SpawnPoints[i].gameObject, typeof(GameObject), true);
                if (CheckSpawnPoint(go))
                {
                    _selectedWave.SpawnPoints[i] = go.transform;
                }
                if (GUI.Button(new Rect(baseX + Space + i * 105, baseY + 45, 82, 15), "Delete"))
                {
                    spawnpointToDelete = i;
                }
            }
            go = (GameObject)EditorGUI.ObjectField(new Rect(baseX + Space + _selectedWave.SpawnPoints.Count * 105, baseY + 25, 100, 15), null, typeof(GameObject), true);
            if (CheckSpawnPoint(go))
            {
                _selectedWave.SpawnPoints.Add(go.transform);
            }
            GUI.EndScrollView();
            if (spawnpointToDelete > -1) _selectedWave.SpawnPoints.RemoveAt(spawnpointToDelete);
        }

        private void ApplyWaveChanges(Wave wave, float newCountDown, string newRandomSeed)
        {
            if (newCountDown < 0) newCountDown = 0;
            if (newRandomSeed == null) newRandomSeed = "";
            wave.Countdown = newCountDown;
            wave.Seed = newRandomSeed;
        }

        private bool CheckSpawnPoint(GameObject go)
        {
            return go != null && go.transform.parent.name == "SpawnPoints";
        }

        private void DrawClustersForWave(float baseX, float baseY)
        {
            float remainingWidth = position.width - baseX;
            float remainingHeight = position.height - baseY;

            const int minClustersInRow = 3;
            const int clusterBoxWidth = 300;
            const int clusterBoxHeight = 400;
            int cols = ((int)remainingWidth - (_selectedWave.WaveClusters.Count + 2 ) * (int)Space) / clusterBoxWidth;
            if (cols < minClustersInRow) cols = minClustersInRow;
            int rows = (_selectedWave.WaveClusters.Count + 1) / cols;
            if ((_selectedWave.WaveClusters.Count + 1) % cols > 0) rows++;

            GUI.Box(new Rect(baseX, baseY, remainingWidth, remainingHeight), "");
            GUI.Label(new Rect(baseX + Space, baseY + Space, 200, 15), "Wave clusters / groups:", EditorStyles.boldLabel);

            for(int i = 0; i < _selectedWave.WaveClusters.Count; i++)
            {
                i += DrawCluster(
                    baseX + Space + (i % cols) * (clusterBoxWidth + Space),
                    baseY + Space + 20 + (i / cols) * (clusterBoxHeight + Space),
                    clusterBoxWidth, clusterBoxHeight, i);
            }

            if (GUI.Button(
                new Rect(
                    baseX + Space + (_selectedWave.WaveClusters.Count % cols) * (clusterBoxWidth + Space),
                    baseY + Space + 20 + (_selectedWave.WaveClusters.Count / cols) * (clusterBoxHeight + Space),
                    clusterBoxWidth,
                    clusterBoxHeight),
                "Add cluster / group"))
            {
                _selectedWave.WaveClusters.Add(InstantiateWaveCluster());
                SetDirty(_selectedWave, "Add cluster");
            }
        }

        private int DrawCluster(float baseX, float baseY, int width, int height, int index)
        {
            var rect = new Rect(baseX, baseY, width, height);

            using (new GuiColor(new Color(1f, 0.7f, 0.17f))) GUI.Box(rect, "");

            var image = AssetPreview.GetAssetPreview(_selectedWave.WaveClusters[index].Prefab);
            var content = new GUIContent(image);

            var cluster = _selectedWave.WaveClusters[index];

            if (GUI.Button(new Rect(baseX + width - Space - 20, baseY + Space, 20, 20), "X"))
            {
                DestroyImmediate(_selectedWave.WaveClusters[index].gameObject);
                _selectedWave.WaveClusters.RemoveAt(index);
                return -1;    // redraw the current cluster
            }

            if (GUI.Button(new Rect(baseX + width - Space - 50, baseY + Space, 20, 20), ">"))
            {
                if (index < _selectedWave.WaveClusters.Count - 1)
                {
                    SwapClusters(index, index + 1);
                    return -1;    // redraw the current cluster
                }
            }

            if (GUI.Button(new Rect(baseX + width - Space - 75, baseY + Space, 20, 20), "<"))
            {
                if (index > 0)
                {
                    SwapClusters(index, index - 1);
                    return -2;    // redraw the current and prevoius cluster
                }
            }

            if (GUI.Button(new Rect(baseX + Space, baseY + Space, 40, 40), content))
            {
                Debug.Log("Clicked the prefab");
            }

            const float innerSpace = 215f;

            GUI.Label(new Rect(baseX + Space, baseY + 50, 100, 15), "Spawn data: ", EditorStyles.boldLabel);

            GUI.Label(new Rect(baseX + Space, baseY + 70, 100, 15), "Prefab: ");
            GUI.Label(new Rect(baseX + Space, baseY + 90, 100, 15), "Amount: ");
            GUI.Label(new Rect(baseX + Space, baseY + 110, 100, 15), "Interval: ");
            GUI.Label(new Rect(baseX + Space, baseY + 130, 100, 15), "Deviation: ");
            if (index > 0)
            {
                GUI.Label(new Rect(baseX + Space, baseY + 150, 150, 15), "Initial Countdown: ");
                GUI.Label(new Rect(baseX + Space, baseY + 170, 150, 15), "Spawn with previous: ");
            }

            GUI.Label(new Rect(baseX + Space, baseY + 200, 100, 15), "Enemy data: ", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            var newPrefab = (GameObject)EditorGUI.ObjectField(new Rect(baseX + width - Space - 100, baseY + 70, 100, 15), cluster.Prefab, typeof(GameObject), false);
            int newAmount = EditorGUI.IntField(new Rect(baseX + width - Space - 100, baseY + 90, 100, 15), cluster.Amount);
            float newInterval = EditorGUI.FloatField(new Rect(baseX + width - Space - 100, baseY + 110, 100, 15), cluster.Interval);
            float newDeviation = EditorGUI.Slider(new Rect(baseX + width - Space - 150, baseY + 130, 150, 15), cluster.IntervalDeviation, 0, 1);
            float newCountDown = index == 0 ? 0 : EditorGUI.FloatField(new Rect(baseX + width - Space - 100, baseY + 150, 100, 15), cluster.InitialCountDown);
            bool newSpawnWithPrevious = index != 0 && EditorGUI.Toggle(new Rect(baseX + width - Space - 100, baseY + 170, 15, 15), cluster.SpawnWithPreviousCluster);

            if (newPrefab != cluster.Prefab)
            {
                cluster.Prefab = newPrefab;
                cluster.SampleEnemy = newPrefab;
            }

            if (_selectedWave.WaveClusters[index].SampleEnemy != null)
            {
                var inspectorRect = new Rect(baseX + Space, baseY + innerSpace, width - 2 * Space, height - innerSpace);
                var myEditor = UnityEditor.Editor.CreateEditor( _selectedWave.WaveClusters[index].SampleEnemy.GetComponent<Scrips.EnemyData.WaveData.BaseWaveData>());

                using (new IsolatedArea(inspectorRect))
                {
                    myEditor.OnInspectorGUI();
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                ApplyNewClusterInfo(cluster, newAmount, newCountDown, newInterval, newDeviation, newSpawnWithPrevious);
            }

            return 0;
        }

        private void ApplyNewClusterInfo(
            WaveCluster cluster,
            int newAmount, float newCountdown, float newInterval, float newDeviation, bool newSpawbWithPrevious)
        {
            Undo.RecordObject(cluster, "Modify cluster");

            cluster.Amount = newAmount;
            cluster.InitialCountDown = newCountdown;
            cluster.Interval = newInterval;
            cluster.IntervalDeviation = newDeviation;
            cluster.SpawnWithPreviousCluster = newSpawbWithPrevious;

            EditorUtility.SetDirty(cluster);
        }

        private void AddNewWave()
        {
            WaveController.Waves.Add(InstantiateNewWave());

            SetDirty(WaveController, "Add new wave");
        }

        private void InsertNewWave(int index)
        {
            WaveController.Waves.Insert(index, InstantiateNewWave());

            SetDirty(WaveController, "Add new wave");
        }

        private void DeleteWaveAt(int index)
        {
            if (ReferenceEquals(_selectedWave, WaveController.Waves[index])) _selectedWave = null;
            var wave = WaveController.Waves[index];

            WaveController.Waves.RemoveAt(index);

            DestroyImmediate(wave.gameObject);
            SetDirty(WaveController, "Delete wave");
        }

        private void SelectWave(int index)
        {
            _selectedWave = WaveController.Waves[index];

            _overrideSpawnpointScrollPositions.Clear();
            foreach(var cluster in _selectedWave.WaveClusters)
            {
                _overrideSpawnpointScrollPositions.Add(Vector3.zero);
            }
        }

        private void SwapWaves(int index1, int index2)
        {
            var buffer = WaveController.Waves[index1];
            WaveController.Waves[index1] = WaveController.Waves[index2];
            WaveController.Waves[index2] = buffer;

            SetDirty(WaveController, "Swap waves");
        }

        private void SwapClusters(int index1, int index2)
        {
            var buffer = _selectedWave.WaveClusters[index1];
            _selectedWave.WaveClusters[index1] = _selectedWave.WaveClusters[index2];
            _selectedWave.WaveClusters[index2] = buffer;

            SetDirty(WaveController, "Swap clusters");
        }

        private Wave InstantiateNewWave()
        {
            var go = new GameObject();
            go.transform.parent = WaveController.transform;
            go.name = "wave";
            return go.AddComponent<Wave>();
        }

        private WaveCluster InstantiateWaveCluster()
        {
            var go = new GameObject();
            go.transform.parent = _selectedWave.transform;
            go.name = "wave cluster";
            return go.AddComponent<WaveCluster>();
        }
    }
}

