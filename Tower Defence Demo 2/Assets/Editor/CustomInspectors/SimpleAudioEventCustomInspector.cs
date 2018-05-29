using Scrips.Events.Audio;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(SimpleAudioEvent))]
    public class SimpleAudioEventCustomInspector : AudioEventCustomInspector
    {
        private SimpleAudioEvent _target;

        public override void OnInspectorGUI()
        {
            _target = (SimpleAudioEvent) target;
            base.OnInspectorGUI();
        }

        protected override void DrawBeforeButton()
        {
            DrawAudioClips();
            EditorGUILayout.Space();
            DrawMinMaxSlider("Volume: ", ref _target.MinVolume, ref _target.MaxVolume, 0, 1);
            DrawMinMaxSlider("Pitch: ", ref _target.MinPitch, ref _target.MaxPitch, 0, 2);
        }

        private void DrawAudioClips()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Audio Clips:");
            for (int i = 0; i < _target.AudioClips.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("Clip " + i + ":");

                var modifiedClip = (AudioClip)EditorGUILayout.ObjectField(_target.AudioClips[i], typeof(AudioClip), false);
                _target.AudioClips[i] = modifiedClip;
                bool deleteClip = GUILayout.Button("X");
                if (deleteClip)
                {
                    DeleteAudioClip(ref i);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("New clip:");
            var newClip = (AudioClip)EditorGUILayout.ObjectField(null, typeof(AudioClip), false);
            if (newClip != null) AddAudioClip(newClip);

            EditorGUILayout.EndHorizontal();
        }

        private void AddAudioClip(AudioClip clip)
        {
            Undo.RecordObject(_target, "Add Audio Clip");
            _target.AudioClips.Add(clip);
            EditorUtility.SetDirty(_target);
        }

        private void DeleteAudioClip(ref int i)
        {
            Undo.RecordObject(_target, "Delete Audio Clip");
            _target.AudioClips.RemoveAt(i++);
            EditorUtility.SetDirty(_target);
        }

        private void DrawMinMaxSlider(string label, ref float min, ref float max, float minValue, float maxValue)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.MinMaxSlider(label, ref min, ref max, minValue, maxValue);
            EditorGUILayout.TextField($"{min:F2} - {max:F2}");

            EditorGUILayout.EndHorizontal();
        }
    }
}