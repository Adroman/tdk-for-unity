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

        [SerializeField] private bool logDebugMessages;
        
        private static readonly float Sqrt2 = (float)Math.Sqrt(2);
        
        private void Start()
        {
            _tileManager = GetComponent<TileManager>();
            CalculateWaypoints();
        }

        public bool CalculateWaypoints(TdTile tileInQuestion = null)
        {
            if (_tileManager.Tiles == null && !_tileManager.MapTiles())
            {
                throw new Exception("Tile mapping failed.");
            }

            ResetWaypoints();
            
            var queue = new Queue<TileWithCoordinates>(GetGoals());

            LogDebug("Calculating waypoints...");
            
            while (queue.Count > 0)
            {
                var centerTile = queue.Dequeue();
                LogDebug("Calculating tile at [{0}, {1}]", centerTile.X, centerTile.Y);
                
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

            return !IsAnyEnemyStuck(tileInQuestion);
        }

        private void ResetWaypoints()
        {
            foreach (var tile in _tileManager.Tiles)
            {
                tile.ResetTile();
            }
        }

        private List<TileWithDistance> CalculateNeighborDistances(
            TileWithCoordinates centerTile,
            List<TileWithCoordinates> neighbors)
        {
            return neighbors.Select(tile => new TileWithDistance(
                tile.Tile,
                DistanceBetweenTiles(centerTile, tile))).ToList();
        }

        private float DistanceBetweenTiles(TileWithCoordinates from, TileWithCoordinates to)
        {
            // same tile
            if (from.Tile == to.Tile) return 0;
            
            // touched by sides
            if (from.X == to.X || from.Y == to.Y) return 1;
            
            // touched by corners
            return Sqrt2;
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
                    var targetTile = _tileManager.Tiles[i, j];
                    if (IsTileLegalToWalkTo(new Vector2Int(x, y), 
                            targetTile, new Vector2Int(i, j))) 
                        result.Add(new TileWithCoordinates(i, j, targetTile));
                }

            return result;
        }

        /// <summary>
        /// Determines whether the target tile is walkable directly from the active tile.
        /// In order to be walkable, it must meet the following criteria:
        /// 1. Target must be walkable
        /// 2a. Target must be touching a side with the active tile OR
        /// 2b. Target must be touching a corner with the active tile
        ///     and at least one common side neighbor must be walkable
        /// </summary>
        /// <param name="activeTileCoords"></param>
        /// <param name="targetTile"></param>
        /// <param name="targetTileCoords"></param>
        /// <returns>true if the target tile is walkable directly</returns>
        private bool IsTileLegalToWalkTo(Vector2Int activeTileCoords, 
            TdTile targetTile, Vector2Int targetTileCoords)
        {
            // 1. Tile must be walkable
            if (!targetTile.Walkable) return false;
            
            // 2a. Tiles must touch each other with a side
            var distance = targetTileCoords - activeTileCoords;
            if (distance.x == 0 || distance.y == 0) return true;
            
            // 2b. At least one tile neighboring both tiles must be walkable
            var touchingTile1 = _tileManager.Tiles[activeTileCoords.x, targetTileCoords.y];
            var touchingTile2 = _tileManager.Tiles[targetTileCoords.x, activeTileCoords.y];
            return touchingTile1.Walkable || touchingTile2.Walkable;
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
        
        private IEnumerable<TileWithCoordinates> GetSpawnpoints()
        {
            for (var y = 0; y < _tileManager.Height; y++)
            for (var x = 0; x < _tileManager.Width; x++)
            {
                if (_tileManager.Tiles[x, y].IsSpawnpoint)
                {
                    yield return new TileWithCoordinates(x, y, _tileManager.Tiles[x, y]);
                }
            }
        }

        private bool IsAnyEnemyStuck(TdTile tileInQuestion)
        {
            var spawnPoints = GetSpawnpoints();
            if (spawnPoints.Any(spawnPoint =>
                    float.IsPositiveInfinity(spawnPoint.Tile.DistanceToGoal)))
            {
                LogWarning("Some spawnpoints are blocked.");
                return true;
            }
            
            if (enemies.Any(enemy =>
                    IsEnemyCutOffFromGoal(enemy)
                    || IsEnemyOnTheGivenTile(enemy, tileInQuestion)))
            {
                LogWarning("Some enemies are blocked.");
                return true;
            }
            
            return false;
        }

        private bool IsEnemyOnTheGivenTile(EnemyInstance enemy, TdTile tileInQuestion)
        {
            if (tileInQuestion == null) return false;

            if (Math.Abs(enemy.transform.position.x - tileInQuestion.transform.position.x) < 0.5f
                && Math.Abs(enemy.transform.position.y - tileInQuestion.transform.position.y) < 0.5f)
            {
                LogWarning("Some enemies are on the given tile.");
                return true;
            }
            return false;
        }

        private bool IsEnemyCutOffFromGoal(EnemyInstance enemy)
        {
            var tileEnemyIsOn = enemy.ActiveTile;
            if (float.IsPositiveInfinity(tileEnemyIsOn.DistanceToGoal))
            {
                LogWarning("Some enemies' path are blocked.");
                return true;
            }

            return false;
        }

        private void LogDebug(string format, params object[] args)
        {
            if (logDebugMessages)
            {
                Debug.LogFormat(format, args);
            }
        }

        private void LogWarning(string format, params object[] args)
        {
            if (logDebugMessages)
            {
                Debug.LogWarningFormat(format, args);
            }
        }
    }
}