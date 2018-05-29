using System;
using Scrips;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(Level))]
    public class LevelCustomInspector : UnityEditor.Editor
    {
        private Level _level;
        private GameObject _camera;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _level = (Level)target;
            _camera = GameObject.Find("Main Camera");
            DrawTilePrefab();
            DrawSizeSettings();
            DrawGenerateButton();
        }

        private void DrawSizeSettings()
        {
            GUILayout.Label("Size: ", EditorStyles.boldLabel);

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Width: ");
                int newWidth = EditorGUILayout.IntField(_level.Width);
                GUILayout.Label("Heigth: ");
                int newHeight = EditorGUILayout.IntField(_level.Height);

                newWidth = Math.Max(0, newWidth);
                newHeight = Math.Max(0, newHeight);

                _level.Width = newWidth;
                _level.Height = newHeight;
            }
            GUILayout.EndHorizontal();
        }

        private void DrawGenerateButton()
        {
            if (GUILayout.Button("Generate new map"))
            {
                GenerateNewMap();
            }
        }

        private void GenerateNewMap()
        {
            _level.RecreateTiles();

            float minX = -(_level.Width - 1) / 2f;
            float minY = -(_level.Height - 1) / 2f;

            _camera.GetComponent<Camera>().orthographicSize = _level.Height / 2f;

            var tilesGo = _level.transform.Find("Tiles");
            var spawnpointsGo = _level.transform.Find("SpawnPoints");
            var goalsGo = _level.transform.Find("Goals");

            foreach (Transform child in spawnpointsGo.transform)
            {
                DestroyImmediate(child.gameObject);
            }

            foreach (Transform child in goalsGo.transform)
            {
                DestroyImmediate(child.gameObject);
            }

            for (int x = 0; x < _level.Width; x++)
            {
                for(int y = 0; y < _level.Height; y++)
                {
                    var go = (GameObject) PrefabUtility.InstantiatePrefab(_level.TilePrefab);
                    go.transform.parent = _level.transform.Find("Tiles");
                    go.transform.position = new Vector3(minX + x, minY + y, 1);

                    _level[x, y] = go;
                }
            }
        }

        private void DrawTilePrefab()
        {
            var go = (GameObject)EditorGUILayout.ObjectField("Default tile: ", _level.TilePrefab, typeof(GameObject), false);
            _level.TilePrefab = go;
        }
    }
}

