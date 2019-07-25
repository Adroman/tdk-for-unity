using Scrips.Data;
using Scrips.Waves;
using UnityEditor;
using UnityEngine;

namespace Editor.WaveEditor
{
    public class WaveGeneratorHeader
    {
        private readonly WaveGenerator _generator;
        private readonly WaveEditorWindow _waveEditor;

        private readonly Vector2 _fieldSize = new Vector2(100, 15);
        private readonly Vector2 _smallerFieldSize = new Vector2(80, 15);
        private readonly Vector2 _labelSize = new Vector2(300, 15);

        private readonly string[] _infiniteOptions = new string[] {"Set amount", "Infinite amount"};

        public WaveGeneratorHeader(WaveEditorWindow waveEditor, WaveGenerator waveGenerator)
        {
            _waveEditor = waveEditor;
            _generator = waveGenerator;
        }

        public void Draw(Vector2 position)
        {
            var rect = new Rect(position, _labelSize);

            EditorGUI.LabelField(rect, "Countdown");

            rect.x += 310;
            rect.size = _fieldSize;
            _generator.CountdownForEachWave = EditorGUI.IntField(rect, _generator.CountdownForEachWave);

            position += Vector2.up * 20;
            EditorGUI.LabelField(new Rect(position, _labelSize), "Number of waves");
            int infinite = EditorGUI.Popup(new Rect(position + Vector2.right * 310, new Vector2(15, 15)),
                !_generator.Infinite ? 0 : 1, _infiniteOptions);
            if (infinite == 0)
            {
                // set amount
                _generator.Infinite = false;
                _generator.WavesTotal = EditorGUI.IntField(
                    new Rect(position + Vector2.right * 330, _smallerFieldSize),
                    _generator.WavesTotal);
            }
            else
            {
                _generator.Infinite = true;
                EditorGUI.LabelField(new Rect(position + Vector2.right * 330, _smallerFieldSize), "Infinite");
            }

            position += Vector2.up * 20;
            EditorGUI.LabelField(new Rect(position, _labelSize), "Generator settings", EditorStyles.boldLabel);

            position += Vector2.up * 20;
            EditorGUI.LabelField(new Rect(position, _labelSize), "Enemies to use");
            for (int i = _generator.EnemiesToUse.Count; i >= 0; i--)
            {
                if (DrawEnemyPrefab(position + Vector2.right * (i * 105 + 310), i))
                {
                    _generator.EnemiesToUse.RemoveAt(i);
                    Undo.RecordObject(_generator, "Remove enemy");
                    EditorUtility.SetDirty(_generator);
                }
            }

            position += Vector2.up * 20;
            EditorGUI.LabelField(new Rect(position, _labelSize), "Spawnpoints to use");
            for (int i = _generator.SpawnpointsToUse.Count; i >= 0; i--)
            {
                if (DrawSpawnpointTile(position + Vector2.right * (i * 105 + 310), i))
                {
                    _generator.SpawnpointsToUse.RemoveAt(i);
                    Undo.RecordObject(_generator, "Remove spawnpoint");
                    EditorUtility.SetDirty(_generator);
                }
            }

            position += Vector2.up * 20;
            EditorGUI.LabelField(new Rect(position, _labelSize), "Number of clusters to generate");

            rect.y = position.y;
            //rect.x += 310;
            rect.size = _fieldSize;
            EditorGUI.LabelField(rect, "Min");
            rect.x += 30;
            rect.width -= 30;
            _generator.MinClusters = EditorGUI.IntField(rect, _generator.MinClusters);

            rect.x += 75;
            EditorGUI.LabelField(rect, "Max");
            rect.x += 30;
            _generator.MaxClusters = EditorGUI.IntField(rect, _generator.MaxClusters);

            position.y += 20;
            EditorGUI.LabelField(new Rect(position, _labelSize), "Random seed");
            position.x += 310;
            _generator.RandomSeed = EditorGUI.TextField(new Rect(position, _fieldSize), _generator.RandomSeed);

            position.y += 20;
            position.x -= 310;
            if (GUI.Button(new Rect(position, _labelSize), "Generate waves"))
                _generator.GenerateWaves();

            position.x += 310;
            if (GUI.Button(new Rect(position, _labelSize), "Clear waves"))
                _generator.Waves.Clear();

            position.x -= 310;
            position.y += 20;
            EditorGUI.LabelField(new Rect(position, _labelSize), "Waves", EditorStyles.boldLabel);

            position.y += 20;
            var waveList = new WaveListNode(_waveEditor, _generator.Waves);
            waveList.Draw(position);
        }

        private bool DrawSpawnpointTile(Vector2 position, int index)
        {
            bool newIndex = index >= _generator.SpawnpointsToUse.Count;
            var enemyToUse = !newIndex
                ? _generator.SpawnpointsToUse[index]
                : null;

            var rect = new Rect(position + Vector2.right * 20, _smallerFieldSize);
            var newSpawnpoint = EditorGUI.ObjectField(rect, enemyToUse, typeof(Spawnpoint), true)
                as Spawnpoint;

            if (newSpawnpoint != null)
            {
                if (newIndex)
                {
                    _generator.SpawnpointsToUse.Add(newSpawnpoint.transform);
                    Undo.RecordObject(_generator, "Add new spawnpoint");
                    EditorUtility.SetDirty(_generator);
                }
                else
                {
                    _generator.SpawnpointsToUse[index] = newSpawnpoint.transform;
                }
            }

            rect.position = position;
            rect.width = 15;
            return !newIndex && GUI.Button(rect, "X");
        }

        private bool DrawEnemyPrefab(Vector2 position, int index)
        {
            bool newIndex = index >= _generator.EnemiesToUse.Count;
            var enemyToUse = !newIndex
                             ? _generator.EnemiesToUse[index]
                             : null;

            var rect = new Rect(position + Vector2.right * 20, _smallerFieldSize);
            var newEnemy = EditorGUI.ObjectField(rect, enemyToUse, typeof(BaseEnemyGenerationModifiers), false)
                as BaseEnemyGenerationModifiers;

            if (newEnemy != null)
            {
                if (newIndex)
                {
                    _generator.EnemiesToUse.Add(newEnemy);
                    Undo.RecordObject(_generator, "Add new enemy");
                    EditorUtility.SetDirty(_generator);
                }
                else
                {
                    _generator.EnemiesToUse[index] = newEnemy;
                }
            }

            rect.position = position;
            rect.width = 15;
            return !newIndex && GUI.Button(rect, "X");
        }
    }
}