using Scrips.EnemyData.WaveData;
using UnityEditor;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(WaveDataWithCustomInspector))]
    public class ComplexEnemyInstanceCustomInspector : UnityEditor.Editor
    {
        private WaveDataWithCustomInspector _enemy;

        public override void OnInspectorGUI()
        {
            _enemy = (WaveDataWithCustomInspector) target;

            float newSpeed = EditorGUILayout.FloatField(_enemy.InitialSpeed);
            float newHp = EditorGUILayout.FloatField(_enemy.InitialHitpoints);

            EditorGUILayout.LabelField("<----------------->");

            float newArmor = EditorGUILayout.FloatField(_enemy.InitialArmor);

            _enemy.InitialSpeed = newSpeed;
            _enemy.InitialArmor = newArmor;
            _enemy.InitialHitpoints = newHp;
        }
    }
}