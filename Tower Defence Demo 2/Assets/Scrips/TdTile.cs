using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Data;
using Scrips.Instances;
using Scrips.Modifiers;
using Scrips.Spells;
using Scrips.Towers.BaseData;
using UnityEngine;
using TileWithDistance = Data.TileWithDistance;

namespace Scrips
{
    //[Serializable]
    [ExecuteInEditMode]
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

        [SerializeField]
        private Camera _camera;

        private CircleRenderer _spellCircle;

        private SpellSpawner _spellSpawner;

        public TileColor TileColor;

        private TowerInstance _currentTower;
        private bool _readyToBuild;
        private SpriteRenderer _renderer;
        private static GameObject _towersParent;

        private static Level _level;

        private static Level LevelProp
        {
            get
            {
                if (_level == null)
                    _level = GameObject.FindObjectsOfType<Level>().First();

                return _level;
            }
        }

        public float DistanceToGoal { get; private set; } = Mathf.Infinity;
        public List<TdTile> NextTiles { get; private set; }

        public bool Buildable
        {
            get => _buildable;
            set
            {
                _buildable = value;
                if (value)
                {
                    _isGoal = false;
                    _isSpawnpoint = false;
                    _walkable = false;
                }
            }
        }

        public bool Walkable
        {
            get => _walkable;
            set
            {
                _walkable = value;
                if (value)
                {
                    _buildable = false;
                }
                else
                {
                    _isGoal = false;
                    _isSpawnpoint = false;
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

        public List<TdTile> CalculateDistance(List<TileWithDistance> allNeighbors)
        {
            var result = new List<TdTile>();

            if (!Walkable) return result;

            if (IsGoal)
            {
                DistanceToGoal = 0;
                foreach(var n in allNeighbors)
                {
                    var t = n.Tile.GetComponent<TdTile>();
                    if (t == null) throw new InvalidOperationException("GameObject has to have Tile component");
                    if (!Walkable) continue;
                    if (float.IsPositiveInfinity(t.DistanceToGoal)) result.Add(n.Tile);
                }

                return result;
            }


            foreach (var n in allNeighbors)
            {
                var t = n.Tile.GetComponent<TdTile>();
                if (t == null) continue;
                if (!Walkable) continue;
                if (float.IsPositiveInfinity(t.DistanceToGoal)) result.Add(n.Tile);
                else
                {
                    float dist = t.DistanceToGoal + n.Distance;
                    if (dist < DistanceToGoal)
                    {
                        DistanceToGoal = dist;
                        NextTiles = new List<TdTile>
                        {
                            n.Tile
                        };
                    }
                    else if (Math.Abs(dist - DistanceToGoal) < 0.001f)
                    {
                        NextTiles.Add(n.Tile);
                    }
                }
            }
            return result;
        }

        public void SetNextTile(IEnumerable<TdTile> targets, float distance)
        {
            NextTiles = new List<TdTile>(targets);
            DistanceToGoal = distance;
        }

        public void OnDrawGizmos()
        {
            if (NextTiles == null) return;
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
        }

        public void HighlightTile()
        {
            if (Buildable)
            {
                _renderer.color = TileColor.InGameHoverColor;
            }

            if (_currentTower != null) _currentTower.ShowRangeCircle();
        }

        public void StopHighlightTile()
        {
            _renderer.color = TileColor.InGameColor;
            _readyToBuild = false;
            if (_currentTower != null) _currentTower.HideRangeCircle();
        }

        public void ReadyToBuild()
        {
            if (Buildable)
                _readyToBuild = true;
            else if (_currentTower != null)
                _readyToBuild = true;
        }

        public void Build(TowerUiData selectedTower)
        {
            if (_readyToBuild)
            {
                _readyToBuild = false;
                if (_currentTower == null)
                    BuildTower(selectedTower);
                else
                    _currentTower.Upgrade(_currentTower.GetPossibleUpgrades().FirstOrDefault());
            }
        }

        private void BuildTower(TowerUiData selectedTower)
        {
            var tower = selectedTower.BaseTowerData.BuildTower(
                transform.position - new Vector3(0, 0, 1), transform.rotation, TowersParent.transform,
                selectedTower);
            if (tower == null) return;
            Buildable = false;
            _readyToBuild = false;
            _renderer.color = TileColor.InGameColor;
            _currentTower = tower;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Entered");
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Exited");
        }
    }
}
