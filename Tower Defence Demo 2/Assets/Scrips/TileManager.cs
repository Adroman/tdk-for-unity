using System;
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
        
        public bool TilesAreValid { get; private set; } = false;

        public void ValidateTiles()
        {
            TilesAreValid = true;
            MapTiles();
            if (fillEmptyTiles)
            {
                FillTiles();
            }
            else
            {
                // Make sure there are no empty holes
            }

            if (trimExcessTiles)
            {
                TrimTiles();
            }
            else
            {
                // Make sure there are no excess tile
            }
        }

        private void MapTiles()
        {
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
        }

        public void TrimTiles()
        {
            var children = gameObject.GetComponentsInChildren<TdTile>();
            foreach (var tile in children)
            {
                var position = tile.transform.position;
                var xPos = (int)Math.Round(position.x - xOffset, MidpointRounding.ToEven);
                var yPos = (int)Math.Round(position.y - yOffset, MidpointRounding.ToEven);
                if (xPos < 0 || xPos >= width || yPos < 0 || yPos >= height)
                {
                    Debug.Log($"Tile at [{position.x}, {position.y}] is out of bounds. Deleting the tile.");
                    Destroy(tile.gameObject);
                }
            }
        }

        public void FillTiles()
        {
            if (defaultTile == null)
            {
                Debug.LogError("Unable to determine default tile. DefaultTile is null.");
                return;
            }

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
                            transform.parent);
                    }
                }
            }
        }
    }
}