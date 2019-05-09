using System.Text;
using Scrips.Data.Formula;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(BaseFormula), true)]
    public class BaseFormulaCustomInspector : UnityEditor.Editor
    {
        private int _minLevel;
        private int _maxLevel;
        private readonly StringBuilder _sb = new StringBuilder();
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var target = (BaseFormula) this.target;

            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Debug Info", EditorStyles.boldLabel);
            _minLevel = EditorGUILayout.IntField("Minimum level", _minLevel);
            _maxLevel = EditorGUILayout.IntField("Maximum level", _maxLevel);

            if (GUILayout.Button("Show level requirements"))
            {
                _sb.Clear();
                for (int i = _minLevel; i <= _maxLevel; i++)
                {
                    _sb.Append("Level ")
                        .Append(i)
                        .Append(" requirement: ")
                        .Append(target.GetLevelRequirement(i))
                        .AppendLine();
                }
                Debug.Log(_sb.ToString());
            }
        }
    }
}