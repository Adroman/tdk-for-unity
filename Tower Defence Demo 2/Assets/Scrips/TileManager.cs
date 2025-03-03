using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scrips
{
    [ExecuteAlways]
    public class TileManager : MonoBehaviour
    {
        private TdTile[,] _tiles;
        [SerializeField]
        private int width, height;
        
        [SerializeField] private TdTile defaultTile;
        [SerializeField] private bool trimExcessTiles = true;
        [SerializeField] private bool fillEmptyTiles = true;

        [SerializeField] private float xOffset;
        [SerializeField] private float yOffset;
        
        [SerializeField] private GameObject spawnpointsParent;
        [SerializeField] private GameObject goalsParent;
        
        public bool TilesAreValid { get; private set; } = false;

        private Dictionary<TdTile, GameObject> _spawnpoints = new Dictionary<TdTile, GameObject>();
        private Dictionary<TdTile, GameObject> _goals = new Dictionary<TdTile, GameObject>();

        public void ValidateTiles()
        {
            TilesAreValid = true;
            MapTiles();
            if (fillEmptyTiles) FillTiles();
            else
            {
                // Make sure there are no empty holes
            }

            if (trimExcessTiles) TrimTiles();
            else
            {
                // Make sure there are no excess tiles
            }
        }

        private void MapTiles()
        {
            Debug.Log("Mapping tiles.");
            _tiles = new TdTile[width, height];
            
            var children = gameObject.GetComponentsInChildren<TdTile>();
            foreach (var tile in children)
            {
                var position = tile.transform.position;
                var xPos = (int)Math.Round(position.x - xOffset, MidpointRounding.ToEven);
                var yPos = (int)Math.Round(position.y - yOffset, MidpointRounding.ToEven);
                if (_tiles[xPos, yPos] == null)
                {
                    _tiles[xPos, yPos] = tile;
                }
                else
                {
                    Debug.LogError($"Another tile at [{position.x}, {position.y}] already exists.");
                    TilesAreValid = false;
                }
            }
            Debug.Log("Done mapping tiles.");
        }

        public int TrimTiles()
        {
            Debug.Log("Trimming tiles.");
            var trimmedTiles = 0;
            var children = gameObject.GetComponentsInChildren<TdTile>();
            foreach (var tile in children)
            {
                var position = tile.transform.position;
                var xPos = (int)Math.Round(position.x - xOffset, MidpointRounding.ToEven);
                var yPos = (int)Math.Round(position.y - yOffset, MidpointRounding.ToEven);
                if (xPos < 0 || xPos >= width || yPos < 0 || yPos >= height)
                {
                    Debug.Log($"Tile at [{position.x}, {position.y}] is out of bounds. Deleting the tile.");
                    DestroyImmediate(tile.gameObject);
                    trimmedTiles++;
                }
            }
            Debug.Log("Done trimming tiles.");
            return trimmedTiles;
        }

        public int FillTiles()
        {
            var tilesFilled = 0;
            if (defaultTile == null)
            {
                Debug.LogError("Unable to determine default tile. DefaultTile is null.");
                return tilesFilled;
            }
            
            MapTiles();
            
            Debug.Log("Filling tiles.");
  
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (_tiles[x, y] == null)
                    {
                        Debug.Log($"Tile at [{x + xOffset}, {y + yOffset}] is empty. Creating new tile from default.");
                        _tiles[x, y] = Instantiate(
                            defaultTile,
                            new Vector3(x + xOffset, y + yOffset),
                            Quaternion.identity,
                            transform);
                        tilesFilled++;
                    }
                }
            }
            Debug.Log("Done filling tiles.");
            return tilesFilled;
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
                $"Spawnpoint {_spawnpoints.Count}", 
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
                $"Goal {_goals.Count}",
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

        public void RefreshSpawnpointsAndGoals()
        {
            Debug.Log("Refreshing spawnpoints and goals.");
        }
    }
}