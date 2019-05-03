using Scrips.CustomTypes.IncreaseType;
using Scrips.Modifiers;
using UnityEditor;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(BaseModifier), true)]
    public class BaseModifierCustomInspector : UnityEditor.Editor
    {
        private BaseModifier _target;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            _target = (BaseModifier) target;
            var desiredIncreaseType = _target.IncreaseType;

            if (desiredIncreaseType == null
                || (desiredIncreaseType.GetType() != typeof(AdditiveIncreaseType)
                    && desiredIncreaseType.GetType() != typeof(MultiplicativeIncreaseType)))
            {
                EditorGUILayout.HelpBox("Increase type has to be additive or multiplicative", MessageType.Error);
            }
        }
    }
}