using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(TestScript))]
    public class TestScriptCustomInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var target = this.target as TestScript;

            if (GUILayout.Button("Test"))
            {
                target.TestAvailableUpgrades();
            }
        }
    }
}