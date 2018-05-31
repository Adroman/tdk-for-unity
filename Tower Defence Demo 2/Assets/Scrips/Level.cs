using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.Profiling;

namespace Scrips
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private GameObject[] _serializedTiles;

        [HideInInspector]
        public int Width, Height;

        [HideInInspector]
        public GameObject TilePrefab;

        public GameObject this[int x, int y]
        {
            get { return _serializedTiles[y * Width + x]; }
            set { _serializedTiles[y * Width + x] = value; }
        }

        public void Start()
        {
            Profiler.BeginSample("Pathfinding");
            CalculateWaypoints();
            Profiler.EndSample();
        }

        public void RecreateTiles()
        {
            _serializedTiles = new GameObject[Width * Height];
            var tilesGo = GameObject.Find("Tiles");

            for (int i = tilesGo.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(tilesGo.transform.GetChild(i).gameObject);
            }
        }

        public void CalculateWaypoints()
        {
            var goals = GetGoals().ToList();

            var queue = new Queue<TileWithCoordinates>(goals);

            while(queue.Count > 0)
            {
                var go = queue.Dequeue();
                var tile = go.gameObject.GetComponent<TdTile>();
                var neighbors = GetNeighbors(go.x, go.y);
                //Debug.Log($"Tile {go.x}, {go.y}");
                var nextNeighbors = tile.CalculateDistance(neighbors.Select(
                    kv => new TileWithDistance(
                        kv.gameObject,
                        (new Vector2(kv.x, kv.y) - new Vector2(go.x, go.y)).magnitude
                    )
                ).ToList()).ToList();
                foreach(var nextN in neighbors.Where(n => nextNeighbors.Contains(n.gameObject)))
                {
                    if (!queue.Contains(nextN))
                    {
                        //Debug.Log($"Adding {nextN.x}, {nextN.y} from {go.x} {go.y}");
                        queue.Enqueue(nextN);
                    }
                }
            }
        }

        private List<TileWithCoordinates> GetNeighbors(int x, int y)
        {
            int minX = Math.Max(0, x - 1);
            int maxX = Math.Min(Width - 1, x + 1);
            int minY = Math.Max(0, y - 1);
            int maxY = Math.Min(Height - 1, y + 1);

            var result = new List<TileWithCoordinates>();

            for (int i = minX; i <= maxX; i++)
            {
                for(int j = minY; j <= maxY; j++)
                {
                    if (x != i || y != j)
                    {
                        try
                        {
                            var go = this[i, j];
                            var tile = go.GetComponent<TdTile>();
                            if (tile.Walkable)
                                result.Add(new TileWithCoordinates(i, j, this[i, j]));
                        }
                        catch(IndexOutOfRangeException)
                        {
                            Debug.LogError($"Index out of range: [{i}, {j}]");
                        }
                    }
                }
            }

            return result;
        }

        private List<TileWithCoordinates> GetGoals()
        {

            var result = new List<TileWithCoordinates>();

            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    var tile = this[x, y];
                    if (tile == null) Debug.LogError($"Tile at [{x}, {y}] is null, which is BS");
                    var tileComponent = tile.GetComponent<TdTile>();
                    if (tileComponent.IsGoal)
                        result.Add(new TileWithCoordinates(x, y, tile));
                }
            }

            return result;
        }
    }
}
