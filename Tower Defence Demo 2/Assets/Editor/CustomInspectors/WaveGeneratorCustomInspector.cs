using Scrips.Waves;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(WaveGenerator2))]
    public class WaveGeneratorCustomInspector : UnityEditor.Editor
    {
        private WaveGenerator2 _target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _target = (WaveGenerator2) target;

            if (GUILayout.Button("GenerateWaves") && !_target.Infinite)
            {
                Undo.RecordObject(_target, "Generate waves");
                _target.GenerateWaves();
                EditorUtility.SetDirty(_target);
            }
        }
    }
}