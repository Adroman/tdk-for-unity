using System.Collections.Generic;
using System.Linq;
using Scrips.Waves;
using UnityEditor;
using UnityEngine;

namespace Editor.WaveEditor
{
    public class WaveListNode
    {
        private readonly List<WaveNumber> _waves;

        private readonly WaveEditorWindow _waveEditor;

        private readonly Vector2 _bigButtonSize = new Vector2(100, 30);

        public WaveListNode(WaveEditorWindow waveEditor, List<WaveNumber> waves)
        {
            _waveEditor = waveEditor;
            _waves = waves;
        }

        public void Draw(Vector2 position)
        {
            float baseY = position.y;

            var innerRect = new Rect(0, baseY, 115, 5 + _waves.Count * 35);
            var scrollRect = new Rect(0, baseY, 130, _waveEditor.position.height - baseY);
            _waveEditor.WaveListScrollPosition = GUI.BeginScrollView(scrollRect, _waveEditor.WaveListScrollPosition, innerRect);

            var oldColor = GUI.color;

            for (int i = 0; i < _waves.Count; i++)
            {
                if (i == _waveEditor.SelectedWave)
                    GUI.color = Color.cyan;

                position.y = baseY + i * 35;
                if (GUI.Button(new Rect(position, _bigButtonSize), $"Wave {i + 1}"))
                {
                    SelectWave(i);
                }

                GUI.color = oldColor;
            }

            GUI.EndScrollView();

            position.x += 135;
            position.y = baseY;
            var selectedWave = _waves.FirstOrDefault(w => w.Number == _waveEditor.SelectedWave + 1);
            if (selectedWave != null)
            {
                var waveNode = new WaveNode(_waveEditor, _waves, selectedWave.Wave);
                waveNode.Draw(position);
            }
        }

        private void SelectWave(int index)
        {
            _waveEditor.SelectedWave = index;
        }
    }
}