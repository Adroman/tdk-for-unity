using Scrips.Waves;
using UnityEditor;
using UnityEngine;

namespace Editor.WaveEditor
{
    public class WaveEditorWindow : EditorWindow
    {
        public WaveGenerator WaveGenerator;

        public int? SelectedWave;

        public Vector2 WaveListScrollPosition;

        [MenuItem("Tools/Tower defence kit/Wave editor")]
        public static WaveEditorWindow Init()
        {
            return GetWindow<WaveEditorWindow>(false, "Wave editor");
        }

        private void OnGUI()
        {
            minSize = new Vector2(50, 50);

            var rect = new Rect(10, 10, 300, 15);

            WaveGenerator = EditorGUI.ObjectField(rect, WaveGenerator, typeof(WaveGenerator), true) as WaveGenerator;

            if (WaveGenerator == null)
            {
                var waveGenerators = FindObjectsOfType<WaveGenerator>();
                if (waveGenerators.Length == 1)
                    WaveGenerator = waveGenerators[0];
            }

            if (WaveGenerator != null)
            {
                var header = new WaveGeneratorHeader(this, WaveGenerator);
                header.Draw(new Vector2(10, 30));
                //draw the rest
            }
            else
            {
                //label
                rect.x += 320;
                EditorGUI.LabelField(rect, "PUT THAT WAVE GENERATOR IN IT");
            }
        }
    }
}