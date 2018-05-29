using Scrips.Variables;
using UnityEditor;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(IntVariable))]
    public class IntVariableCustomInspector : UnityEditor.Editor
    {
        private IntVariable _target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _target = (IntVariable) target;

            EditorGUILayout.BeginHorizontal();

            int newValue = EditorGUILayout.IntField("Value", _target.Value);

            EditorGUILayout.EndHorizontal();

            _target.Value = newValue;
        }
    }
}