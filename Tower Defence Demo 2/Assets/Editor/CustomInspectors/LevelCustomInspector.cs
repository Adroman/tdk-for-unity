using System;
using System.Linq;
using Scrips;
using UnityEditor;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(Level))]
    public class LevelCustomInspector : UnityEditor.Editor
    {
        private Level _level;
        private Camera _camera;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _level = (Level)target;
            _camera = FindObjectsOfType<CameraController>().First().GetComponent<Camera>();
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
                GUILayout.Label("Height: ");
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

            var tilesGo = GameObject.Find("Tiles");
            var spawnpointsGo = GameObject.Find("SpawnPoints");
            var goalsGo = GameObject.Find("Goals");

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
                    var tile = (TdTile) PrefabUtility.InstantiatePrefab(_level.TilePrefab);
                    var tileTransform = tile.transform;

                    tileTransform.parent = tilesGo.transform;
                    tileTransform.position = new Vector3(minX + x, minY + y, 1);

                    _level[x, y] = tile;
                }
            }
        }

        private void DrawTilePrefab()
        {
            var go = (TdTile)EditorGUILayout.ObjectField("Default tile: ", _level.TilePrefab, typeof(TdTile), false);
            _level.TilePrefab = go;
        }
    }
}

