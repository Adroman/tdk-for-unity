using Scrips.Waves;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(WaveGenerator))]
    public class WaveGeneratorCustomInspector : UnityEditor.Editor
    {
        private WaveGenerator _target;

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
        }
    }
}