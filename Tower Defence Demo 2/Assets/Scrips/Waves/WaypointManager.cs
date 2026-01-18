using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Scrips.EnemyData.Instances;
using Scrips.Events.Alerts;
using Scrips.Variables;
using UnityEngine;
using UnityEngine.Serialization;

namespace Scrips.Waves
{
    [RequireComponent(typeof(TileManager))]
    public class WaypointManager : MonoBehaviour
    {
        private TileManager _tileManager;
        
        [SerializeField] private AlertEvent userErrorAlert;
        [SerializeField] private EnemyCollection enemies;
        
        private void Start()
        {
            _tileManager = GetComponent<TileManager>();
            CalculateWaypoints();
        }

        public bool CalculateWaypoints(int? x = null, int? y = null)
        {
            if (_tileManager.Tiles == null && !_tileManager.MapTiles())
            {
                throw new Exception("Tile mapping failed.");
            }
            
            var queue = new Queue<TileWithCoordinates>(GetGoals());

            Debug.Log("Calculating waypoints...");
            
            while (queue.Count > 0)
            {
                var centerTile = queue.Dequeue();
                Debug.Log($"Calculating tile at [{centerTile.X}, {centerTile.Y}]");
                
                var tile = centerTile.Tile;
                var legalNeighbors = GetLegalNeighbors(centerTile.X, centerTile.Y);
                var nextNeighborsToProcess = tile.CalculateDistance(
                    CalculateNeighborDistances(centerTile, legalNeighbors)
                );
                foreach (var neighbor 
                         in legalNeighbors
                             .Where(n => nextNeighborsToProcess.Contains(n.Tile)))
                {
                    if (!queue.Contains(neighbor))
                        queue.Enqueue(neighbor);
                }
            }

            return !IsAnyEnemyStuck(x, y);
        }

        private List<TileWithDistance> CalculateNeighborDistances(
            TileWithCoordinates centerTile,
            List<TileWithCoordinates> neighbors)
        {
            return neighbors.Select(tile => new TileWithDistance(
                tile.Tile,
                Mathf.Sqrt(
                    (centerTile.X - tile.X) * (centerTile.X - tile.X) +
                    (centerTile.Y - tile.Y) * (centerTile.Y - tile.Y)
            ))).ToList();
        }
        
        private List<TileWithCoordinates> GetLegalNeighbors(int x, int y)
        {
            var minX = Math.Max(0, x - 1);
            var maxX = Math.Min(_tileManager.Width - 1, x + 1);
            var minY = Math.Max(0, y - 1);
            var maxY = Math.Min(_tileManager.Height - 1, y + 1);

            var result = new List<TileWithCoordinates>();

            for (var i = minX; i <= maxX; i++)
            for (var j = minY; j <= maxY; j++)
                if (x != i || y != j)
                {
                    var tile = _tileManager.Tiles[i, j];
                    if (tile.Walkable)
                        result.Add(new TileWithCoordinates(i, j, tile));
                }

            return result;
        }

        private IEnumerable<TileWithCoordinates> GetGoals()
        {
            for (var y = 0; y < _tileManager.Height; y++)
            for (var x = 0; x < _tileManager.Width; x++)
            {
                if (_tileManager.Tiles[x, y].IsGoal)
                {
                    yield return new TileWithCoordinates(x, y, _tileManager.Tiles[x, y]);
                }
            }
        }

        private bool IsAnyEnemyStuck(int? x, int? y)
        {
            if (x.HasValue != y.HasValue)
            {
                throw new Exception("Both coordinates must be null or not null.");
            }
            
            return enemies.Any(
                enemy => 
                    IsEnemyCutOffFromGoal(enemy)
                    && (!x.HasValue || !y.HasValue 
                                    || IsEnemyOnTheGivenTile(enemy, x.Value, y.Value))
                );
        }

        private bool IsEnemyOnTheGivenTile(EnemyInstance enemy, int x, int y)
        {
            var (enemyX, enemyY) = _tileManager.GetTileCoordinates(enemy.transform);
            return x == enemyX && y == enemyY;
        }

        private bool IsEnemyCutOffFromGoal(EnemyInstance enemy) 
            => float.IsPositiveInfinity(enemy.ActiveTile.DistanceToGoal);
    }
}