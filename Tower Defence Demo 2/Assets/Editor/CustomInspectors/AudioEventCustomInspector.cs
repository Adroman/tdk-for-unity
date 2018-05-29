using Scrips.Events.Audio;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(AudioEvent))]
    public class AudioEventCustomInspector : UnityEditor.Editor
    {
        [SerializeField] private AudioSource _sample;

        public void OnEnable()
        {
            _sample = EditorUtility.CreateGameObjectWithHideFlags("Audio preview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();
        }

        public void OnDisable()
        {
            DestroyImmediate(_sample.gameObject);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawBeforeButton();

            EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
            if (GUILayout.Button("Test"))
            {
                ((AudioEvent) target).Play(_sample);
            }
            EditorGUI.EndDisabledGroup();
        }

        protected virtual void DrawBeforeButton()
        {

        }
    }
}