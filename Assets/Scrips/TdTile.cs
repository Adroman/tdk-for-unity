using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Data;
using Scrips.Events.Alerts;
using Scrips.Spells;
using Scrips.Towers;
using Scrips.Towers.BaseData;
using Scrips.Waves;
using UnityEngine;
using TileWithDistance = Data.TileWithDistance;

namespace Scrips
{
    public class TdTile : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private bool _buildable;

        [HideInInspector]
        [SerializeField]
        private bool _walkable;

        [HideInInspector]
        [SerializeField]
        private bool _isSpawnpoint;

        [HideInInspector]
        [SerializeField]
        private bool _isGoal;

        private bool _hasTowerOnIt;
        
        private WaypointManager _waypointManager;

        [SerializeField]
        private Camera _camera;

        private CircleRenderer _spellCircle;

        private SpellSpawner _spellSpawner;

        public TileColor TileColor;

        private TileManager _tileManager;
        
        private TowerInstance _currentTower;
        private SpriteRenderer _renderer;
        private TowerSelector _towerSelector;
        private static GameObject _towersParent;
        
        [SerializeField]
        private AlertEvent userErrorAlertEvent;
        private bool _hasUserErrorAlertEvent = false;

        public float DistanceToGoal { get; private set; } = Mathf.Infinity;
        public List<TdTile> NextTiles { get; } = new List<TdTile>();

        public bool Buildable
        {
            get => _buildable;
            set => _buildable = value;
        }

        public bool Walkable
        {
            get => _walkable && !_hasTowerOnIt;
            set
            {
                _walkable = value;
                if (!value)
                {
                    _isSpawnpoint = false;
                    _isGoal = false;
                }
            }
        }

        public bool IsSpawnpoint
        {
            get => _isSpawnpoint;
            set
            {
                _isSpawnpoint = value;
                if (value)
                {
                    _isGoal = false;
                    _buildable = false;
                    _walkable = true;
                }
            }
        }

        public bool IsGoal
        {
            get => _isGoal;
            set
            {
                _isGoal = value;
                if (value)
                {
                    _isSpawnpoint = false;
                    _buildable = false;
                    _walkable = true;
                }
            }
        }

        public TowerInstance CurrentTower
        {
            get => _currentTower;
            set
            {
                _currentTower = value;
                _hasTowerOnIt = value != null;
            }
        }

        private void OnEnable()
        {
            _tileManager = GetComponentInParent<TileManager>();
            if (_tileManager == null)
            {
                Debug.LogError(
                    $"This TdTile instance is not TileManager's child GameObject. Tile name: {gameObject.name}. Coordinates: {transform.position}");
            }
            else
            {
                if (IsSpawnpoint) _tileManager.RegisterSpawnpoint(this);
                else if (IsGoal) _tileManager.RegisterGoal(this);
            }
        }

        private void OnDisable()
        {
            if (_tileManager == null)
            {
                Debug.LogError(
                    $"This TdTile instance is not TileManager's child GameObject. Tile name: {gameObject.name}. Coordinates: {transform.position}.");
            }
            else
            {
                if (IsSpawnpoint) _tileManager.RemoveSpawnpoint(this);
                else if (IsGoal) _tileManager.RemoveGoal(this);
            }
        }

        public void ResetTile()
        {
            DistanceToGoal = Mathf.Infinity;
            NextTiles.Clear();
        }

        public List<TdTile> CalculateDistance(List<TileWithDistance> allNeighbors)
        {
            var result = new List<TdTile>();

            if (!Walkable) return result;

            if (IsGoal)
            {
                DistanceToGoal = 0;
                result.AddRange(
                    from n in allNeighbors 
                    let t = n.Tile 
                    where float.IsPositiveInfinity(t.DistanceToGoal) 
                    select n.Tile
                    );

                return result;
            }
            foreach (var n in allNeighbors)
            {
                var t = n.Tile;
                if (float.IsPositiveInfinity(t.DistanceToGoal)) result.Add(n.Tile);
                else
                {
                    var dist = t.DistanceToGoal + n.Distance;
                    if (dist < DistanceToGoal)
                    {
                        DistanceToGoal = dist;
                        NextTiles.Clear();
                        NextTiles.Add(n.Tile);
                    }
                    else if (Math.Abs(dist - DistanceToGoal) < 0.001f)
                    {
                        NextTiles.Add(n.Tile);
                    }
                }
            }
            return result;
        }

        public void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (var n in NextTiles)
                Gizmos.DrawLine(transform.position + Vector3.back * 2, n.transform.position + Vector3.back * 2);
        }

        private static GameObject TowersParent
        {
            get
            {
                if (_towersParent == null)
                {
                    _towersParent = GameObject.Find("Towers");
                }
                return _towersParent;
            }
        }

        // Use this for initialization
        private void Start()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.color = Application.isPlaying ? TileColor.InGameColor : TileColor.EditorColor;
            _camera = GameObject.Find("Main Camera")?.GetComponent<Camera>();
            _spellCircle = GameObject.Find("SpellPoint")?.GetComponent<CircleRenderer>();
            _spellSpawner = GameObject.Find("SpellPoint")?.GetComponent<SpellSpawner>();
            _towerSelector = GetComponentInParent<TowerSelector>();
            _waypointManager = FindObjectOfType<WaypointManager>();
            
            if (_waypointManager == null) Debug.LogError("WaypointManager is null");
            _hasUserErrorAlertEvent = userErrorAlertEvent != null;
            _hasTowerOnIt = _currentTower != null;
        }

        private void AlertUser(string title, string message)
        {
            if (!_hasUserErrorAlertEvent) return;
            userErrorAlertEvent.Invoke(title, message);
        }

        public void HighlightTile()
        {
            if (Buildable && _towerSelector.SelectedTower != null)
            {
                _renderer.color = TileColor.InGameHoverColor;
            }

            if (_hasTowerOnIt) CurrentTower.ShowRangeCircle();
        }

        public void StopHighlightTile()
        {
            _renderer.color = TileColor.InGameColor;
            if (_hasTowerOnIt) CurrentTower.HideRangeCircle();
        }

        public void SelectTile()
        {
            if (Buildable && !_hasTowerOnIt)
                BuildTower();
            else if (_hasTowerOnIt)
                // TODO: Instead of upgrade, show UI
                CurrentTower.Upgrade(CurrentTower.GetPossibleUpgrades().FirstOrDefault());
        }

        private void BuildTower()
        {
            var selectedTower = _towerSelector.SelectedTower;
            if (selectedTower == null)
            {
                Debug.LogWarning("No selected tower to build.");
                AlertUser("Build error", "No selected tower to build.");
                return;
            }

            if (Walkable)
            {
                Walkable = false;
                bool result = _waypointManager.CalculateWaypoints(this);
                if (!result)
                {
                    // Tower blocks an enemy, revert walkable stat and recalculate waypoints
                    Debug.LogWarning("Tower blocks an enemy.");
                    AlertUser("Error building tower", "Tower blocks an enemy.");
                    
                    Walkable = true;
                    
                    result = _waypointManager.CalculateWaypoints();

                    if (!result)
                    {
                        throw new Exception("Waypoint recalculation failed the second time.");
                    }
                    return;
                }

                Walkable = true;
            }
            
            var tower = selectedTower.BaseTowerData.BuildTower(
                transform.position - new Vector3(0, 0, 1), transform.rotation, TowersParent.transform,
                selectedTower, userErrorAlertEvent);
            if (tower == null) return;
            Buildable = false;
            _renderer.color = TileColor.InGameColor;
            CurrentTower = tower;
        }

        private void OnMouseEnter()
        {
            HighlightTile();
        }

        private void OnMouseExit()
        {
            StopHighlightTile();
        }

        private void OnMouseUpAsButton()
        {
            SelectTile();
        }
    }
}
