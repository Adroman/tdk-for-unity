using Scrips.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(UiCurrencyIntTextWithImage))]
    public class UiCurrencyIntTextWithImageInspector : UnityEditor.Editor
    {
        private UiCurrencyIntTextWithImage _target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _target = (UiCurrencyIntTextWithImage)target;

            if (GUILayout.Button("Apply image and text from variable."))
            {
                Undo.RecordObject(_target, "Update variable text");
                _target.UpdateText();

                var image = _target.GetComponentInChildren<Image>();
                image.sprite = _target.Variable.Icon;
                image.color = _target.Variable.IconColor;

                EditorUtility.SetDirty(_target);
            }
        }
    }
}