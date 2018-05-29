using System;
using System.Collections.Generic;
using Data;
using Scrips.Data;
using Scrips.Instances;
using UnityEditor;
using UnityEngine;
using TileWithDistance = Data.TileWithDistance;

namespace Scrips
{
    [Serializable]
    //[ExecuteInEditMode]
    public class TdTile : MonoBehaviour
    {
        public bool Buildable;

        public TileColor TileColor;

        [HideInInspector]
        public List<GameObject> WaypointTypesSupported;

        [HideInInspector]
        public List<TileType> WaypointTypes;

        private TowerInstance _currentTower;
        private bool _readyToBuild;
        private SpriteRenderer _renderer;
        private static GameObject _towersParent;

        private List<Transform> _next = null;
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

        public List<GameObject> CalculateDistance(List<TileWithDistance> allNeighbors)
        {
            var result = new List<GameObject>();

            if (WaypointTypes.Count == 0) return result;

            if (WaypointTypes.Contains(TileType.Goal))
            {
                DistanceToGoal = 0;
                foreach(var n in allNeighbors)
                {
                    var t = n.gameObject.GetComponent<TdTile>();
                    if (t == null) throw new InvalidOperationException("GameObject has to have Tile component");
                    if (t.WaypointTypes.Count == 0) continue;
                    if (float.IsPositiveInfinity(t.DistanceToGoal)) result.Add(n.gameObject);
                }

                return result;
            }


            foreach (var n in allNeighbors)
            {
                var t = n.gameObject.GetComponent<TdTile>();
                if (t == null) continue;
                if (t.WaypointTypes.Count == 0) continue;
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
                    else if (dist == DistanceToGoal)
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
            //if (!Application.isPlaying) return;

            //EditorApplication.playModeStateChanged -= OnPlayModeChanged;
            //EditorApplication.playModeStateChanged += OnPlayModeChanged;

            _renderer = GetComponent<SpriteRenderer>();
            _renderer.color = Application.isPlaying ? TileColor.InGameColor : TileColor.EditorColor;
        }

        private void Update()
        {
            //_renderer.color = Application.isPlaying ? TileColor.InGameColor : TileColor.EditorColor;
        }

        private void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                _renderer.color = TileColor.EditorColor;
            }
        }

        private void OnMouseEnter()
        {
            if (Buildable)
            {
                _renderer.color = TileColor.InGameHoverColor;
            }
        }

        private void OnMouseExit()
        {
            _renderer.color = TileColor.InGameColor;
            _readyToBuild = false;
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

        private void OnDestroy()
        {
            if(Application.isPlaying == false && Application.isEditor && EditorPrefs.GetInt("SelectedEditorTool", 0) == 0)
            {
                Debug.Log("Not allowing to delete");
            }
        }

        private void BuildTower()
        {
            // Check, if we have enough resources
            //Debug.Log("Building");
            if (SelectedTowerOption.Option.SelectedTowerPrefab == null) return;
            if (ScoreManager.Instance.Gold < SelectedTowerOption.Option.Price) return;
            ScoreManager.Instance.Gold -= SelectedTowerOption.Option.Price;
            Instantiate(SelectedTowerOption.Option.SelectedTowerPrefab, transform.position - new Vector3(0, 0, 1), transform.rotation, TowersParent.transform);
            Buildable = false;
            _readyToBuild = false;
            _renderer.color = TileColor.InGameColor;
        }
    }
}
