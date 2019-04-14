using Scrips.Statistics;
using UnityEditor;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(EnemyStatistics))]
    public class EnemyStatisticsCustomInspector : UnityEditor.Editor
    {
        private EnemyStatistics _target;

        private bool _showDetails;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            _target = (EnemyStatistics) target;

            _showDetails = EditorGUILayout.Foldout(_showDetails, "Detailed info");

            if (_showDetails)
            {
                foreach (var pair in _target.EnemiesByAttribute)
                {
                    EditorGUILayout.LabelField(pair.Key.Name, pair.Value.ToString());
                }
            }
        }
    }
}