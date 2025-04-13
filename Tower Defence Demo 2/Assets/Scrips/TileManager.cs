using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Scrips
{
    public class TileManager : MonoBehaviour
    {
        private TdTile[,] _tiles;
        
        [SerializeField] 
        [Min(0)]
        [Tooltip("Width of the game area.")]
        private int width;
        
        [SerializeField] 
        [Min(0)] 
        [Tooltip("Height of the game area.")]
        private int height;
        
        [SerializeField] private TdTile defaultTile;
        
        [Tooltip("Toggle to trim excess tiles on validation.")]
        [SerializeField] 
        private bool trimExcessTiles = true;
        
        [Tooltip("Toggle to fill empty tiles with a default tile on validation.")]
        [SerializeField] private bool fillEmptyTiles = true;

        [SerializeField]
        [Tooltip("The lowest x coordinate for a tile. It is recommended to end with .5 value.")]
        private float xOffset;
        
        [SerializeField]
        [Tooltip("The lowest y coordinate for a tile. It is recommended to end with .5 value.")]
        private float yOffset;
        
        [SerializeField]
        [Tooltip("Game object under which the spawnpoints are instantiated.")]
        private GameObject spawnpointsParent;
        
        [SerializeField] 
        [Tooltip("Game object under which the goals are instantiated.")]
        private GameObject goalsParent;
        
        public TdTile[,] Tiles => _tiles;
        public float XOffset => xOffset;
        public float YOffset => yOffset;
        public int Width => width;
        public int Height => height;
        public bool FillEmptyTiles => fillEmptyTiles;
        public bool TrimExcessTiles => trimExcessTiles;
        public TdTile DefaultTile => defaultTile;

        private readonly Dictionary<TdTile, GameObject> _spawnpoints = new Dictionary<TdTile, GameObject>();
        private readonly Dictionary<TdTile, GameObject> _goals = new Dictionary<TdTile, GameObject>();

        /// <summary>
        /// When the TileManager is selected it will draw a rectangle where tiles are supposed to be placed.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            var corner1 = new Vector3(xOffset - 0.6f, yOffset - 0.6f, 0);
            var corner2 = new Vector3(xOffset - 0.6f, yOffset + height - 0.4f, 0);
            var corner3 = new Vector3(xOffset + width - 0.4f, yOffset + height - 0.4f, 0);
            var corner4 = new Vector3(xOffset + width - 0.4f, yOffset - 0.6f, 0);
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(corner1, corner2);
            Gizmos.DrawLine(corner2, corner3);
            Gizmos.DrawLine(corner3, corner4);
            Gizmos.DrawLine(corner4, corner1);
        }

        /// <summary>
        /// Performs the following operations.
        /// Fills or ensures (chosen by the Fill toggle) there are no empty tiles within the game area.
        /// Trims or ensures (chosen by the Trim toggle) there are no tiles outside of the game area.
        /// </summary>
        public bool ValidateTiles()
        {
            var tilesAreValid = MapTiles();
            
            tilesAreValid &= CheckNotEmptyTiles();

            tilesAreValid &= CheckNotOffBoundsTiles();
            
            return tilesAreValid;
        }

        /// <summary>
        /// Checks if the is no empty space in tiles area.
        /// </summary>
        /// <returns>true if check succeeds</returns>
        public bool CheckNotEmptyTiles()
        {
            var valid = true;
            for (var x = 0; x < width; x++)
                for (var y = 0; y < height; y++)
                    if (_tiles[x, y] == null)
                    {
                        valid = false;
                        Debug.LogWarning($"There is no tile at {x}, {y}.");
                    }

            return valid;
        }

        private (int, int) GetTileCoordinates(Transform tileTransform)
        {
            var position = tileTransform.position;
            var xPos = (int)Math.Round(position.x - xOffset, MidpointRounding.ToEven);
            var yPos = (int)Math.Round(position.y - yOffset, MidpointRounding.ToEven);
            return (xPos, yPos);
        }

        /// <summary>
        /// Checks if every tile is withing the bounds of the tile area.
        /// </summary>
        /// <returns>true if the check succeeds</returns>
        public bool CheckNotOffBoundsTiles()
        {
            var valid = true;
            foreach (Transform child in transform)
            {
                var (xPos, yPos) = GetTileCoordinates(child);
                if (xPos < 0 || xPos >= width || yPos < 0 || yPos >= height)
                {
                    Debug.LogWarning($"Tile at [{child.position}] is out of bounds.");
                    valid = false;
                }
            }
            return valid;
        }

        public bool MapTiles()
        {
            var valid = true;
            
            Debug.Log("Mapping tiles.");
            _tiles = new TdTile[width, height];
            
            var children = gameObject.GetComponentsInChildren<TdTile>();
            foreach (var tile in children)
            {
                var (xPos, yPos) = GetTileCoordinates(tile.transform);

                if (xPos > width - 1 || xPos < 0 || yPos > height - 1 || yPos < 0)
                {
                    Debug.LogWarning($"Tile at [{tile.transform.position}] is out of bounds.");
                    //valid = false;
                    continue;
                }
                
                if (_tiles[xPos, yPos] == null)
                    _tiles[xPos, yPos] = tile;
                else
                {
                    Debug.LogWarning($"Another tile at [{tile.transform.position}] already exists.");
                    valid = false;
                }
            }
            Debug.Log("Done mapping tiles.");

            return valid;
        }

        private GameObject CreateEmptyGameObject(string newObjectName, Vector3 position, Quaternion rotation, Transform parent)
        {
            var result = new GameObject(newObjectName);
            result.transform.SetParent(parent);
            result.transform.SetPositionAndRotation(position, rotation);
            return result;
        }

        public void RegisterSpawnpoint(TdTile tile)
        {
            Debug.Log("Registering spawnpoint.");
            
            var spawnObject = CreateEmptyGameObject(
                "Spawnpoint", 
                tile.transform.position, 
                tile.transform.rotation, 
                spawnpointsParent.transform);
            
            _spawnpoints[tile] = spawnObject;
        }

        public void RemoveSpawnpoint(TdTile tile)
        {
            Debug.Log("Removing spawnpoint.");
            
            DestroyImmediate(_spawnpoints[tile]);
            
            _spawnpoints.Remove(tile);
        }

        public void RegisterGoal(TdTile tile)
        {
            Debug.Log("Registering goal.");
            
            var goalObject = CreateEmptyGameObject(
                "Goal",
                tile.transform.position,
                tile.transform.rotation,
                goalsParent.transform);
            
            _goals[tile] = goalObject;
        }

        public void RemoveGoal(TdTile tile)
        {
            Debug.Log("Removing goal.");
            
            DestroyImmediate(_goals[tile]);
            
            _goals.Remove(tile);
        }

        public int RefreshSpawnpointsAndGoals()
        {
            Debug.Log("Refreshing spawnpoints.");
            var instantiated = 0;
            
            foreach (var key in _spawnpoints.Keys)
            {
                var tile = _spawnpoints[key];
                if (tile == null)
                {
                    _spawnpoints[key] = CreateEmptyGameObject(
                        "Spawnpoint",
                        tile.transform.position,
                        tile.transform.rotation,
                        spawnpointsParent.transform);
                    instantiated++;
                }
            }
            
            Debug.Log("Refreshing spawnpoints.");
            foreach (var key in _goals.Keys)
            {
                var tile = _goals[key];
                if (tile == null)
                {
                    _goals[key] = CreateEmptyGameObject(
                        "Goal",
                        tile.transform.position,
                        tile.transform.rotation,
                        goalsParent.transform);
                    instantiated++;
                }
            }

            return instantiated;
        }
    }
}