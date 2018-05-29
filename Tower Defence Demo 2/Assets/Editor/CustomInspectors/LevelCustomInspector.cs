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
            _level.Tiles = new GameObject[_level.Width, _level.Height];
            int size = _level.Height / 2;

            //_camera.GetComponent<Camera>().orthographicSize = size;
            for (int x = 0; x < _level.Width; x++)
            {
                for(int y = 0; y < _level.Height; y++)
                {

                }
            }
        }

        private void DrawTilePrefab()
        {
            var go = (GameObject)EditorGUILayout.ObjectField(_level.TilePrefab, typeof(GameObject), false);
            _level.TilePrefab = Level.StTilePrefab = go;
        }
    }
}

