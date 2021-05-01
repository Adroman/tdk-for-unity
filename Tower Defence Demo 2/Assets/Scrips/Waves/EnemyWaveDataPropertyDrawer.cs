using UnityEditor;
using UnityEngine;

namespace Scrips.Waves
{
    //[CustomPropertyDrawer(typeof(EnemyWaveData))]
    public class EnemyWaveDataPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
        }
    }
}