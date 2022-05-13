using System.Collections.Generic;
using System.Linq;
using Scrips.LevelData;
using Scrips.LevelData.ExtraSteps;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor.CustomWindows
{
    public class PrefabImporter : EditorWindow
    {
        private const float MainButtonWidth = 120;
        private const float Space = 5;
        private const float MiniButtonSize = 20;
        private const float IconSize = 30;
        
        private PrefabTemplate _templateToUse;

        [MenuItem("Tools/Tower defense kit/Prefab Importer")]
        static void Init()
        {
            var window = GetWindow<PrefabImporter>();
            window.Show();
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(10, Space + 0, 300, 20), "Select a template to use for import: ", EditorStyles.boldLabel);
            
            _templateToUse = (PrefabTemplate)EditorGUI.ObjectField(new Rect(300, Space + 0, 200, 20), _templateToUse, typeof(PrefabTemplate), false);

            if (GUI.Button(new Rect(10, 50, 82, 20), "Import"))
            {
                ImportPrefabs();
            }
        }

        private void ImportPrefabs()
        {
            if (_templateToUse == null)
            {
                Debug.LogWarning("No template was selected.");
                return;
            }

            if (_templateToUse.PrefabsToInstantiate == null || _templateToUse.PrefabsToInstantiate.Count == 0)
            {
                Debug.LogWarning("Template does not contain any prefab to instantiate.");
                return;
            }
            
            Debug.Log($"Using {_templateToUse.name}");

            var createdObjects = new List<GameObject>();

            foreach (var objectToCreate in _templateToUse.PrefabsToInstantiate)
            {
                var gameObject = Instantiate(objectToCreate, objectToCreate.transform.position, objectToCreate.transform.rotation);
                if (gameObject.name.EndsWith("(Clone)"))
                {
                    gameObject.name = objectToCreate.name;
                }
                createdObjects.Add(gameObject);
            }

            var objectsToModify = Resources.FindObjectsOfTypeAll<PostImportComponent>();
            {
                foreach (var handler in objectsToModify)
                {
                    handler.HandlePostImport();
                }
            }

            // foreach (var objectToModify in createdObjects)
            // {
            //     foreach (var handler in objectToModify.GetComponents<PostImportComponent>())
            //     {
            //         handler.HandlePostImport();
            //     }
            // }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}