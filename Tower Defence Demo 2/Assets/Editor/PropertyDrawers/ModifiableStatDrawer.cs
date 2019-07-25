using Scrips.Modifiers.Stats;
using Unity.UNetWeaver;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(FloatModifiableStat))]
    [CustomPropertyDrawer(typeof(IntModifiableStat))]
    public class ModifiableStatDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded)
            {
                int arrSize = property.FindPropertyRelative("_modifiers").arraySize;
                return base.GetPropertyHeight(property, label) * (2 + arrSize);
            }
            return base.GetPropertyHeight(property, label) * 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float baseHeight = base.GetPropertyHeight(property, label);
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;

            EditorGUI.indentLevel = 0;

            var baseAmountRect = new Rect(position.x, position.y, 75, baseHeight);
            var modifiedAmountRect = new Rect(position.x + 75, position.y, position.width - 50, baseHeight);

            EditorGUI.LabelField(baseAmountRect, "Base");
            EditorGUI.LabelField(modifiedAmountRect, "Modified");

            baseAmountRect.y += baseHeight;
            modifiedAmountRect.y += baseHeight;
            
            EditorGUI.PropertyField(baseAmountRect, property.FindPropertyRelative("_baseValue"), GUIContent.none);
            EditorGUI.PropertyField(modifiedAmountRect, property.FindPropertyRelative("_modifiedValue"), GUIContent.none);

            EditorGUI.indentLevel = 0;
            var anotherRect = new Rect(position.x, position.y + baseHeight, position.width, baseHeight);

            property.isExpanded = EditorGUI.Foldout(anotherRect, property.isExpanded, GUIContent.none);

            if (property.isExpanded)
            {
                var arrProperty = property.FindPropertyRelative("_modifiers");
                for (int i = 0; i < arrProperty.arraySize; i++)
                {
                    var rect = new Rect(position.x, position.y + (2 + i) * baseHeight, position.width, baseHeight);
                    var element = arrProperty.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(rect, element, GUIContent.none);
                    //EditorGUI.ObjectField(rect, element, GUIContent.none);
                }
            }


            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}