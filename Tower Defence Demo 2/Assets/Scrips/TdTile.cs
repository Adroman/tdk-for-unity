using System;
using System.Collections.Generic;
using Scrips.Data;
using Scrips.Instances;
using UnityEngine;
using TileWithDistance = Data.TileWithDistance;

namespace Scrips
{
    [Serializable]
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

        public TileColor TileColor;

        private TowerInstance _currentTower;
        private bool _readyToBuild;
        private SpriteRenderer _renderer;
        private static GameObject _towersParent;

        private List<Transform> _next;
        private float _distanceToGoal = Mathf.Infinity;

        private static Level _level;

        private static Level LevelProp
        {
            get
            {
                if (_level == null)
                    _level = GameObject.Find("Level").GetComponent<Level>();

                return _level;
            }
        }

        public float DistanceToGoal { get { return _distanceToGoal; } private set { _distanceToGoal = value; } }
        public List<Transform> NextTiles { get { return _next; } private set { _next = value; } }

        public bool Buildable
        {
            get { return _buildable; }
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
            get { return _walkable; }
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
            get { return _isSpawnpoint; }
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
            get { return _isGoal; }
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

        public List<GameObject> CalculateDistance(List<TileWithDistance> allNeighbors)
        {
            var result = new List<GameObject>();

            if (!Walkable) return result;

            if (IsGoal)
            {
                DistanceToGoal = 0;
                foreach(var n in allNeighbors)
                {
                    var t = n.gameObject.GetComponent<TdTile>();
                    if (t == null) throw new InvalidOperationException("GameObject has to have Tile component");
                    if (!Walkable) continue;
                    if (float.IsPositiveInfinity(t.DistanceToGoal)) result.Add(n.gameObject);
                }

                return result;
            }


            foreach (var n in allNeighbors)
            {
                var t = n.gameObject.GetComponent<TdTile>();
                if (t == null) continue;
                if (!Walkable) continue;
                if (float.IsPositiveInfinity(t.DistanceToGoal)) result.Add(n.gameObject);
                else
                {
                    float dist = t.DistanceToGoal + n.distance;
                    if (dist < DistanceToGoal)
                    {
                        DistanceToGoal = dist;
                        NextTiles = new List<Transform>
                        {
                            n.gameObject.transform
                        };
                    }
                    else if (Math.Abs(dist - DistanceToGoal) < 0.001f)
                    {
                        NextTiles.Add(n.gameObject.transform);
                    }
                }
            }
            return result;
        }

        public void SetNextTile(IEnumerable<Transform> targets, float distance)
        {
            _next = new List<Transform>(targets);
            DistanceToGoal = distance;
        }

        public void OnDrawGizmos()
        {
            if (NextTiles == null) return;
            Gizmos.color = Color.red;
            foreach (var n in NextTiles)
                Gizmos.DrawLine(transform.position + Vector3.back * 2, n.position + Vector3.back * 2);
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
        }

        private void OnMouseEnter()
        {
            if (Buildable)
            {
                _renderer.color = TileColor.InGameHoverColor;
            }
            if (_currentTower != null) _currentTower.ShowRangeCircle();
        }

        private void OnMouseExit()
        {
            _renderer.color = TileColor.InGameColor;
            _readyToBuild = false;
            if (_currentTower != null) _currentTower.HideRangeCircle();
        }

        private void OnMouseDown()
        {
            _readyToBuild = Buildable;
        }

        private void OnMouseUp()
        {
            if (_readyToBuild)
            {
                _readyToBuild = false;
                BuildTower();
            }
        }

        private void BuildTower()
        {
            // Check, if we have enough resources
            //Debug.Log("Building");
            if (SelectedTowerOption.Option.SelectedTowerPrefab == null) return;
            if (ScoreManager.Instance.Gold < SelectedTowerOption.Option.Price) return;
            ScoreManager.Instance.Gold -= SelectedTowerOption.Option.Price;
            var tower = Instantiate(SelectedTowerOption.Option.SelectedTowerPrefab, transform.position - new Vector3(0, 0, 1), transform.rotation, TowersParent.transform).GetComponent<TowerInstance>();
            Buildable = false;
            _readyToBuild = false;
            _renderer.color = TileColor.InGameColor;
            _currentTower = tower;
        }
    }
}
