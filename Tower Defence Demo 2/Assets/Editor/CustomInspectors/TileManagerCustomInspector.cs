using System;
using Scrips;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor.CustomInspectors
{
    [CustomEditor(typeof(TileManager))]
    public class TileManagerCustomInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var manager = (TileManager)target;

            GUILayout.Label("Editor settings");

            if (GUILayout.Button(
                    new GUIContent(
                        "Validate tiles",
                        "Checks if all the tiles are on the play area and no tile outside. It also checks the routing.")))
            {
                ValidateTiles(manager);
            }

            if (GUILayout.Button(
                    new GUIContent(
                        "Fill empty tiles",
                        "Fills the empty spaces with the default tile. Ignores the Fill toggle.")))
                FillTiles(manager);
    

            if (GUILayout.Button(
                    new GUIContent(
                        "Trim tiles", 
                        "Trims the tiles placed outside the play area. Ignores the Trim toggle."))) 
                TrimTiles(manager);
            
            if (GUILayout.Button(
                    new GUIContent(
                        "Refresh spawnpoints and goals.",
                        "Re-instantiates deleted spawnpoints and goals.")))
            {
                var instantiated = manager.RefreshSpawnpointsAndGoals();
                if (instantiated > 0)
                {
                    EditorSceneManager.MarkSceneDirty(manager.gameObject.scene);
                    Undo.RecordObject(manager, "Re-instantiate tiles");
                }
            }
        }

        private void ValidateTiles(TileManager manager)
        {
            if (!manager.MapTiles())
            {
                Debug.LogError("Mapping tiles failed. Check the warnings above for details.");
                return;
            }

            if (manager.FillEmptyTiles)
            {
                FillTiles(manager);
            }
            else if (!manager.CheckNotEmptyTiles())
            {
                Debug.LogError("Some spaces are empty in the tiles area. Check the warnings above for details.");
            }

            if (manager.TrimExcessTiles)
            {
                TrimTiles(manager);
            }
            else if (!manager.CheckNotOffBoundsTiles())
            {
                Debug.LogError("Some tiles are placed outside of the tiles area. Check the warnings above for details.");
            }
        }
        
        /// <summary>
        /// Fills the empty area with a default tile inside the game boundaries.
        /// </summary>
        public void FillTiles(TileManager manager)
        {
            var tilesFilled = 0;
            if (manager.DefaultTile == null)
            {
                Debug.LogError("Unable to determine default tile. DefaultTile is null.");
            }
            
            Debug.Log("Filling tiles.");
  
            for (var x = 0; x < manager.Width; x++)
            {
                for (var y = 0; y < manager.Height; y++)
                {
                    if (manager.Tiles[x, y] == null)
                    {
                        Debug.Log($"Tile at [{x + manager.XOffset}, {y + manager.YOffset}] is empty. Creating new tile from default.");
                        manager.Tiles[x, y] = Instantiate(
                            manager.DefaultTile,
                            new Vector3(x + manager.XOffset, y + manager.YOffset, 0),
                            Quaternion.identity,
                            manager.transform);
                        tilesFilled++;
                        
                        Undo.RegisterCreatedObjectUndo(manager.Tiles[x, y], "(Fill) Create new tile");
                    }
                }
            }
            Debug.Log($"Done filling tiles. {tilesFilled} tile(s) filled.");
            if (tilesFilled > 0) EditorSceneManager.MarkSceneDirty(manager.gameObject.scene);
        }
        
        /// <summary>
        /// Trims the tiles outside the game boundaries.
        /// The boundaries are defined from the offset to the offset + width/height.
        /// </summary>
        private void TrimTiles(TileManager manager)
        {
            Debug.Log("Trimming tiles.");
            var children = manager.gameObject.GetComponentsInChildren<TdTile>();
            var trimmedTiles = 0;
            foreach (var tile in children)
            {
                var position = tile.transform.position;
                var xPos = (int)Math.Round(position.x - manager.XOffset, MidpointRounding.ToEven);
                var yPos = (int)Math.Round(position.y - manager.YOffset, MidpointRounding.ToEven);
                if (xPos < 0 || xPos >= manager.Width || yPos < 0 || yPos >= manager.Height)
                {
                    Debug.Log($"Tile at [{position.x}, {position.y}] is out of bounds. Deleting the tile.");
                    Undo.DestroyObjectImmediate(tile.gameObject);
                    trimmedTiles++;
                }
            }
            Debug.Log($"Done trimming tiles. {trimmedTiles} tile(s) trimmed.");
            if (trimmedTiles > 0) EditorSceneManager.MarkSceneDirty(manager.gameObject.scene);
        }
    }
}