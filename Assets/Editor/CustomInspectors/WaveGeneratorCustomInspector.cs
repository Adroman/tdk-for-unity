using System.Text;
using Scrips.Waves;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(WaveGenerator))]
    public class WaveGeneratorCustomInspector : UnityEditor.Editor
    {
        private WaveGenerator _target;
        private readonly StringBuilder _sb = new StringBuilder();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _target = (WaveGenerator) target;

            if (GUILayout.Button("GenerateWaves") && !_target.Infinite)
            {
                Undo.RecordObject(_target, "Generate waves");
                _target.GenerateWaves();
                EditorUtility.SetDirty(_target);
            }

            if (GUILayout.Button("Clear waves"))
            {
                Undo.RecordObject(_target, "Clear waves");
                _target.Waves.Clear();
                EditorUtility.SetDirty(_target);
            }
            
            if (GUILayout.Button("Print waves"))
                PrintWaves();
        }
        
        private void PrintWaves()
        {
            _sb.Clear();
            int maxWave = 20;
            int waveNumber = 0;
            foreach (var wave in _target.GetWaves())
            {
                if (++waveNumber > maxWave) break;
                
                _sb.AppendLine($"Wave {waveNumber}:");
                foreach (var cluster in wave.WaveClusters)
                {
                    _sb.Append($"{cluster.Amount} {cluster.Prefab.name} enemies with ")
                        .Append($"{cluster.EnemyData.InitialHitpoints} HP, ")
                        .Append($"{cluster.EnemyData.InitialArmor} armor and ")
                        .Append($"{cluster.EnemyData.InitialSpeed} speed.").AppendLine();
                }
                _sb.AppendLine();
            }
            
            Debug.Log(_sb.ToString());
        }
    }
}