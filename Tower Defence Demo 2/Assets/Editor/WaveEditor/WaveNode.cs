using System.Collections.Generic;
using Scrips.Waves;
using UnityEditor;
using UnityEngine;

namespace Editor.WaveEditor
{
    public class WaveNode
    {
        private readonly Wave _waveToDraw;
        private readonly WaveEditorWindow _waveEditor;
        private readonly List<WaveNumber> _waves;

        private readonly Vector2 _fieldSize = new Vector2(100, 15);
        private readonly Vector2 _smallerFieldSize = new Vector2(80, 15);
        private readonly Vector2 _labelSize = new Vector2(300, 15);
        
        public WaveNode(WaveEditorWindow waveEditor, List<WaveNumber> waves, Wave waveToDraw)
        {
            _waveEditor = waveEditor;
            _waves = waves;
            _waveToDraw = waveToDraw;
        }
        
        public void Draw(Vector2 position)
        {
            float baseY = position.y;
            position.y -= 20;
            
            EditorGUI.LabelField(new Rect(position, _labelSize), "Wave info", EditorStyles.boldLabel);
        }
    }
}