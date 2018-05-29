using Scrips.Variables;
using UnityEditor;
using UnityEngine;

namespace Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(IntReference))]
    public class IntReferencePropertyDrawer : PropertyDrawer
    {
        private readonly string[] _options = { "Use Constant", "Use Variable" };

        private GUIStyle _style;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (_style == null)
            {
                _style = new GUIStyle(GUI.skin.GetStyle("PaneOptions")) {imagePosition = ImagePosition.ImageOnly};
            }

            label = EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);

            EditorGUI.BeginChangeCheck();

            var useConstant = property.FindPropertyRelative("UseConstant");
            var constantValue = property.FindPropertyRelative("ConstantValue");
            var variable = property.FindPropertyRelative("ReferenceVariable");

            var buttonRect = new Rect(position);
            buttonRect.yMin += _style.margin.top;
            buttonRect.width = _style.fixedWidth + _style.margin.right;
            position.xMin = buttonRect.xMax;

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int result = EditorGUI.Popup(buttonRect, useConstant.boolValue ? 0 : 1, _options, _style);

            useConstant.boolValue = result == 0;

            EditorGUI.PropertyField(position,
                useConstant.boolValue ? constantValue : variable,
                GUIContent.none);

            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}