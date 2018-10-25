using Scrips;
using UnityEditor;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(LevelConfiguration))]
    public class LevelConfigurationCustomInspector : UnityEditor.Editor
    {
        // modified from https://docs.unity3d.com/ScriptReference/SceneAsset.html
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var level = target as LevelConfiguration;

            DrawSceneObjectField(level);

            
        }

        private void DrawSceneObjectField(LevelConfiguration level)
        {
            var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(level.TargetScenePath);

            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            var newScene = EditorGUILayout.ObjectField("Scene", oldScene, typeof(SceneAsset), false) as SceneAsset;

            if (EditorGUI.EndChangeCheck())
            {
                string newPath = AssetDatabase.GetAssetPath(newScene);
                var scenePathProperty = serializedObject.FindProperty(nameof(level.TargetScenePath));
                scenePathProperty.stringValue = newPath;
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}