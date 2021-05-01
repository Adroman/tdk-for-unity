using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Utils;
using Scrips.Data;
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
        private WaveNumber _selectedWave = null;
        private List<Vector3> _overrideSpawnpointScrollPositions = new List<Vector3>();

        private Vector2 _waveListScrollPosition;
        private Vector2 _spawnpointListScrollPosition;
        private Vector2 _clusterListScrollPosition;

        private const float MainButtonWidth = 120;
        private const float MainButtonHeight = 50;
        private const float Space = 5;
        private const float MiniButtonSize = 20;
        private const float IconSize = 30;

        private WaveGenerator Generator => _waveController.GetComponent<WaveGenerator>();

        [MenuItem("Tools/Tower defence kit/Wave editor")]
        public static WaveEditor Init()
        {
            return GetWindow<WaveEditor>(false, "Wave editor");
        }

        private void OnGUI()
        {
            minSize = new Vector2(0, 0);
            DrawHeader();
            DrawWaveList(220);
        }

        private void SetDirty(Object obj, string message)
        {
            EditorSceneManager.MarkSceneDirty(_waveController.gameObject.scene);
            Undo.RegisterCompleteObjectUndo(obj, message);
        }

        private void DrawHeader()
        {
            _waveController = (WaveController)EditorGUI.ObjectField(new Rect(200, Space + 0, 200, 20), _waveController, typeof(WaveController), true);

            if (ReferenceEquals(_waveController, null))
            {
                var oldColor = GUI.color;
                GUI.color = Color.red;
                {
                    GUI.Label(new Rect(10, Space + 0, 200, 20), "Insert WaveController here: ", EditorStyles.boldLabel);
                }

                GUI.color = oldColor;
                return;
            }
            else
            {
                GUI.Label(new Rect(10, Space + 0, 200, 20), "Wave controller to use: ", EditorStyles.boldLabel);
            }

            float topOffset = 30;
            GUI.Label(new Rect(10, Space + topOffset + 0, 200, 20), "Automatic Wave Generation", EditorStyles.boldLabel);

            GUI.Label(new Rect(10, Space + topOffset + 20, 150, 20), "Number of waves:");
            GUI.Label(new Rect(10, Space + topOffset + 40, 150, 20), "Min Clusters:");
            GUI.Label(new Rect(10, Space + topOffset + 60, 150, 20), "Max Clusters:");
            GUI.Label(new Rect(10, Space + topOffset + 80, 150, 20), "Base Difficulty:");
            GUI.Label(new Rect(10, Space + topOffset + 100, 150, 20), "Difficulty increase:");
            GUI.Label(new Rect(10, Space + topOffset + 120, 150, 20), "Difficulty deviation:");
            GUI.Label(new Rect(10, Space + topOffset + 140, 150, 20), "Random seed:");
            GUI.Label(new Rect(10, Space + topOffset + 160, 150, 20), "Infinite amount of waves:");

            EditorGUI.BeginChangeCheck();

            int newWaveAmount = EditorGUI.IntField(new Rect(200, Space + topOffset + 20, 150, 20), Generator.WavesTotal);
            int newMinClusters = EditorGUI.IntField(new Rect(200, Space + topOffset + 40, 150, 20), Generator.MinClusters);
            int newMaxClusters = EditorGUI.IntField(new Rect(200, Space + topOffset + 60, 150, 20), Generator.MaxClusters);
            int newBaseDifficulty = EditorGUI.IntField(new Rect(200, Space + topOffset + 80, 150, 20), Generator.BaseDifficulty);
            float newDifficultyIncrease = EditorGUI.FloatField(new Rect(200, Space + topOffset + 100, 150, 20), Generator.DifficultyIncrease);
            float newDifficultyDeviation = EditorGUI.Slider(new Rect(200, Space + topOffset + 120, 150, 20), Generator.DifficultyDeviation, 0, 1);
            string newSeed = EditorGUI.TextField(new Rect(200, Space + topOffset + 140, 150, 20), Generator.RandomSeed);
            bool newUseInfiniteGeneration = EditorGUI.Toggle(new Rect(200, Space + topOffset + 160, 20, 20), Generator.Infinite);

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
                Generator.RandomSeed = newSeed;
                Generator.WavesTotal = newWaveAmount;
                Generator.Infinite = newUseInfiniteGeneration;
            }

            BaseEnemyGenerationModifiers enemyInfo;

            GUI.Label(new Rect(400, topOffset + 25, 200, 20), "Prefabs to use for generation: ", EditorStyles.boldLabel);

            int prefabToDelete = -1;

            for (int i = 0; i < Generator.EnemiesToUse.Count; i++)
            {
                enemyInfo = (BaseEnemyGenerationModifiers)EditorGUI.ObjectField(new Rect(400 + Space + i * 105, topOffset + 45, 100, 20), Generator.EnemiesToUse[i], typeof(BaseEnemyGenerationModifiers), false);

                if (!ReferenceEquals(enemyInfo, null))
                {
                    // Replace Prefab
                    Generator.EnemiesToUse[i] = enemyInfo;
                }

                if (GUI.Button(new Rect(400 + Space + i * 105, topOffset + 65, 82, 20), "Delete"))
                {
                    // Delete Prefab
                    prefabToDelete = i;
                }
            }
            enemyInfo = (BaseEnemyGenerationModifiers)EditorGUI.ObjectField(new Rect(400 + Space + Generator.EnemiesToUse.Count * 105, topOffset + 45, 100, 20), null, typeof(BaseEnemyGenerationModifiers), false);

            if (!ReferenceEquals(enemyInfo, null))
            {
                // Add Prefab
                Generator.EnemiesToUse.Add(enemyInfo);
            }

            if (prefabToDelete > -1) Generator.EnemiesToUse.RemoveAt(prefabToDelete);

            if (GUI.Button(new Rect(400 + Space, topOffset + 105, 150, 35), "Generate") && Generator.EnemiesToUse.Count > 0)
            {
                GenerateWave();
            }
        }

        private void GenerateWave()
        {
            Generator.GenerateWaves();
            _selectedWave = null;
        }

        private void RefreshWaveList()
        {
            if (Generator.Waves == null)
            {
                Generator.Waves = new List<WaveNumber>();
                return;
            }

            Generator.Waves = Generator.Waves?.Where(wave => wave != null).OrderBy(wave => wave.Number).ToList();
        }

        private void DrawWaveList(float baseY)
        {
            if (ReferenceEquals(_waveController, null))
                return;

            //RefreshWaveList();

            var waves = Generator.Waves;

            var innerRect = new Rect(0, baseY, ListUtils.GetWaveListWidth(0, MainButtonWidth, MiniButtonSize, Space, 20, false), ListUtils.GetScrollHeight(waves.Count + 1, Space, MainButtonHeight));
            var scrollRect = new Rect(0, baseY, ListUtils.GetWaveListWidth(0, MainButtonWidth, MiniButtonSize, Space, 20, true), position.height - baseY);

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
                DrawWaveInfo(ListUtils.GetWaveListWidth(0, MainButtonWidth, MiniButtonSize, Space, 20, true) + Space, baseY);
            }
        }

        private void DrawWaveHeader(int index, WaveNumber waveInfo, float baseY, int totalWaves)
        {
            var oldColor = GUI.color;
            if (!ReferenceEquals(_selectedWave, null) && ReferenceEquals(_selectedWave, waveInfo))
            {
                GUI.color = Color.cyan;
            }

            if (GUI.Button(new Rect(Space, baseY + index * 55 + Space, MainButtonWidth, MainButtonHeight), $"Wave {waveInfo.Number}"))
            {
                SelectWave(index);
            }

            GUI.color = oldColor;

            if (GUI.Button(new Rect(130, baseY + index * 55 + 6.75f, MiniButtonSize, MiniButtonSize), "+"))
            {
                InsertNewWave(index);
            }

            if (index > 0)
            {
                if (GUI.Button(new Rect(155, baseY + index * 55 + 6.75f, MiniButtonSize, MiniButtonSize), "↑"))
                {
                    SwapWaves(index, index - 1);
                }
            }
            if (index + 1 < totalWaves)
            {
                if (GUI.Button(new Rect(155, baseY + index * 55 + 31.75f, MiniButtonSize, MiniButtonSize), "↓"))
                {
                    SwapWaves(index, index + 1);
                }
            }

            if (GUI.Button(new Rect(130, baseY + index * 55 + 31.75f, MiniButtonSize, MiniButtonSize), "-"))
            {
                DeleteWaveAt(index);
            }
        }

        private void DrawWaveInfo(float baseX, float baseY)
        {
            using (new GuiColor(new Color(0.6f, 0.6f, 0.7f))) GUI.Box(new Rect(baseX, baseY, position.width - baseX, Space + 40), GUIContent.none);

            GUI.Label(new Rect(baseX + Space, baseY + Space, 100, 20), "Countdown:");
            int newCountdown = EditorGUI.IntField(new Rect(baseX + 110, baseY + Space, 100, 20), _selectedWave.Wave.Countdown);

            DrawSpawnPointsForWave(baseX, baseY + 50);
            DrawClustersForWave(baseX, baseY + 135);

            ApplyWaveChanges(_selectedWave.Wave, newCountdown);
        }

        private void DrawSpawnPointsForWave(float baseX, float baseY)
        {
            using (new GuiColor(new Color(0.6f, 0.7f, 0.6f))) GUI.Box(new Rect(baseX, baseY, position.width - baseX, Space + 60), GUIContent.none);

            var wave = _selectedWave.Wave;

            GUI.Label(new Rect(baseX + Space, baseY + Space, 100, 15), "Spawnpoints:", EditorStyles.boldLabel);

            GameObject go;
            int spawnpointToDelete = -1;
            _spawnpointListScrollPosition = GUI.BeginScrollView(new Rect(baseX, baseY, position.width - baseX, Space + 75), _spawnpointListScrollPosition, new Rect(baseX, baseY, Space + (wave.Spawnpoints.Count + 1) * 105, Space + 40));
            for (int i = 0; i < wave.Spawnpoints.Count; i++)
            {
                go = (GameObject)EditorGUI.ObjectField(new Rect(baseX + Space + i * 105, baseY + 25, 100, 15), wave.Spawnpoints[i].gameObject, typeof(GameObject), true);
                if (CheckSpawnPoint(go))
                {
                    wave.Spawnpoints[i] = go.transform;
                }
                if (GUI.Button(new Rect(baseX + Space + i * 105, baseY + 45, 82, 15), "Delete"))
                {
                    spawnpointToDelete = i;
                }
            }
            go = (GameObject)EditorGUI.ObjectField(new Rect(baseX + Space + wave.Spawnpoints.Count * 105, baseY + 25, 100, 15), null, typeof(GameObject), true);
            if (CheckSpawnPoint(go))
            {
                wave.Spawnpoints.Add(go.transform);
            }
            GUI.EndScrollView();
            if (spawnpointToDelete > -1) wave.Spawnpoints.RemoveAt(spawnpointToDelete);
        }

        private void ApplyWaveChanges(Wave wave, int newCountDown)
        {
            if (newCountDown < 0) newCountDown = 0;
            wave.Countdown = newCountDown;
        }

        private bool CheckSpawnPoint(GameObject go)
        {
            return go != null && go.transform.parent.name == "SpawnPoints" && go.GetComponent<Spawnpoint>() != null;
        }

        private void DrawClustersForWave(float baseX, float baseY)
        {
            float remainingWidth = position.width - baseX;
            float remainingHeight = position.height - baseY;

            const int minClustersInRow = 3;
            const int clusterBoxWidth = 300;
            const int clusterBoxHeight = 400;
            int cols = ((int)remainingWidth - (_selectedWave.Wave.WaveClusters.Count + 2 ) * (int)Space) / clusterBoxWidth;
            if (cols < minClustersInRow) cols = minClustersInRow;
            int rows = (_selectedWave.Wave.WaveClusters.Count + 1) / cols;
            if ((_selectedWave.Wave.WaveClusters.Count + 1) % cols > 0) rows++;

            GUI.Box(new Rect(baseX, baseY, remainingWidth, remainingHeight), "");
            GUI.Label(new Rect(baseX + Space, baseY + Space, 200, 15), "Wave clusters / groups:", EditorStyles.boldLabel);

            for(int i = 0; i < _selectedWave.Wave.WaveClusters.Count; i++)
            {
                i += DrawCluster(
                    baseX + Space + (i % cols) * (clusterBoxWidth + Space),
                    baseY + Space + 20 + (i / cols) * (clusterBoxHeight + Space),
                    clusterBoxWidth, clusterBoxHeight, i);
            }

            if (GUI.Button(
                new Rect(
                    baseX + Space + (_selectedWave.Wave.WaveClusters.Count % cols) * (clusterBoxWidth + Space),
                    baseY + Space + 20 + (_selectedWave.Wave.WaveClusters.Count / cols) * (clusterBoxHeight + Space),
                    clusterBoxWidth,
                    clusterBoxHeight),
                "Add cluster / group"))
            {
                _selectedWave.Wave.WaveClusters.Add(InstantiateWaveCluster());
                //SetDirty(_selectedWave, "Add cluster");
            }
        }

        private int DrawCluster(float baseX, float baseY, int width, int height, int index)
        {
            var rect = new Rect(baseX, baseY, width, height);
            var wave = _selectedWave.Wave;

            using (new GuiColor(new Color(1f, 0.7f, 0.17f))) GUI.Box(rect, "");

            var image = AssetPreview.GetAssetPreview(wave.WaveClusters[index].Prefab);
            var content = new GUIContent(image);

            var cluster = wave.WaveClusters[index];

            if (GUI.Button(new Rect(baseX + width - Space - 20, baseY + Space, 20, 20), "X"))
            {
                wave.WaveClusters.RemoveAt(index);
                return -1;    // redraw the current cluster
            }

            if (GUI.Button(new Rect(baseX + width - Space - 50, baseY + Space, 20, 20), ">"))
            {
                if (index < wave.WaveClusters.Count - 1)
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
                    return -2;    // redraw the current and previous cluster
                }
            }

            if (GUI.Button(new Rect(baseX + Space, baseY + Space, 40, 40), content))
            {
                Debug.Log("Clicked the prefab");
            }

            const float innerSpace = 215f;

            GUI.Label(new Rect(baseX + Space, baseY + 50, 100, 20), "Spawn data: ", EditorStyles.boldLabel);

            GUI.Label(new Rect(baseX + Space, baseY + 70, 100, 20), "Prefab: ");
            GUI.Label(new Rect(baseX + Space, baseY + 90, 100, 20), "Amount: ");
            GUI.Label(new Rect(baseX + Space, baseY + 110, 100, 20), "Interval: ");
            GUI.Label(new Rect(baseX + Space, baseY + 130, 100, 20), "Deviation: ");
            if (index > 0)
            {
                GUI.Label(new Rect(baseX + Space, baseY + 150, 150, 20), "Initial Countdown: ");
                GUI.Label(new Rect(baseX + Space, baseY + 170, 150, 20), "Spawn with previous: ");
            }

            GUI.Label(new Rect(baseX + Space, baseY + 200, 100, 20), "Enemy data: ", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            var newPrefab = (EnemyInstance)EditorGUI.ObjectField(new Rect(baseX + width - Space - 100, baseY + 70, 100, 20), cluster.Prefab, typeof(EnemyInstance), false);
            int newAmount = EditorGUI.IntField(new Rect(baseX + width - Space - 100, baseY + 90, 100, 20), cluster.Amount);
            float newInterval = EditorGUI.FloatField(new Rect(baseX + width - Space - 100, baseY + 110, 100, 20), cluster.Interval);
            float newDeviation = EditorGUI.Slider(new Rect(baseX + width - Space - 150, baseY + 130, 150, 20), cluster.IntervalDeviation, 0, 1);
            float newCountDown = index == 0 ? 0 : EditorGUI.FloatField(new Rect(baseX + width - Space - 100, baseY + 150, 100, 20), cluster.InitialCountDown);
            bool newSpawnWithPrevious = index != 0 && EditorGUI.Toggle(new Rect(baseX + width - Space - 100, baseY + 170, 15, 20), cluster.SpawnWithPreviousCluster);

            if (newPrefab != cluster.Prefab)
            {
                cluster.Prefab = newPrefab;
            }

            if (wave.WaveClusters[index].EnemyData != null)
            {
                var serializedObject = new SerializedObject(Generator);
                var wavesArray = serializedObject.FindProperty(nameof(Generator.Waves));
                var specificNumberedWave = wavesArray?.GetArrayElementAtIndex(_selectedWave.Number - 1);    // this needs to be fixed
                var specificWave = specificNumberedWave?.FindPropertyRelative(nameof(_selectedWave.Wave));
                var waveClusters = specificWave?.FindPropertyRelative(nameof(wave.WaveClusters));
                var specificCluster = waveClusters?.GetArrayElementAtIndex(index);
                var clusterEnemyInfo = specificCluster?.FindPropertyRelative(nameof(cluster.EnemyData));

                var inspectorRect = new Rect(baseX + Space, baseY + innerSpace, width - 2 * Space, EditorGUI.GetPropertyHeight(clusterEnemyInfo));
                EditorGUI.BeginProperty(inspectorRect, GUIContent.none, clusterEnemyInfo);
                EditorGUI.PropertyField(inspectorRect, clusterEnemyInfo);
                EditorGUI.EndProperty();

                // var myEditor = UnityEditor.Editor.CreateEditor( wave.WaveClusters[index].EnemyData);
                //
                // using (new IsolatedArea(inspectorRect))
                // {
                //     myEditor.OnInspectorGUI();
                // }
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
            // Undo.RecordObject(cluster, "Modify cluster");

            cluster.Amount = newAmount;
            cluster.InitialCountDown = newCountdown;
            cluster.Interval = newInterval;
            cluster.IntervalDeviation = newDeviation;
            cluster.SpawnWithPreviousCluster = newSpawbWithPrevious;

            // EditorUtility.SetDirty(cluster);
        }

        private void AddNewWave()
        {
            Generator.Waves.Add(InstantiateNewWave());

            SetDirty(Generator, "Add new wave");
        }

        private void InsertNewWave(int index)
        {
            // WaveController.Waves.Insert(index, InstantiateNewWave());
            //
            // SetDirty(WaveController, "Add new wave");
        }

        private void DeleteWaveAt(int index)
        {
            if (ReferenceEquals(_selectedWave, Generator.Waves[index])) _selectedWave = null;
            var wave = Generator.Waves[index];

            Generator.Waves.RemoveAt(index);

            SetDirty(Generator, "Delete wave");
        }

        private void SelectWave(int index)
        {
            _selectedWave = Generator.Waves[index];

            _overrideSpawnpointScrollPositions.Clear();
            foreach(var cluster in _selectedWave.Wave.WaveClusters)
            {
                _overrideSpawnpointScrollPositions.Add(Vector3.zero);
            }
        }

        private void SwapWaves(int index1, int index2)
        {
            var buffer = Generator.Waves[index1];
            Generator.Waves[index1] = Generator.Waves[index2];
            Generator.Waves[index2] = buffer;
            //adsad
            SetDirty(Generator, "Swap waves");
        }

        private void SwapClusters(int index1, int index2)
        {
            var wave = _selectedWave.Wave;
            var buffer = wave.WaveClusters[index1];
            wave.WaveClusters[index1] = wave.WaveClusters[index2];
            wave.WaveClusters[index2] = buffer;

            SetDirty(Generator, "Swap clusters");
        }

        private WaveNumber InstantiateNewWave()
        {
            return new WaveNumber(Generator.Waves.Count + 1);
        }

        private WaveCluster InstantiateWaveCluster()
        {
            return new WaveCluster();
        }
    }
}

