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
        [SerializeField] private TdTile[] _serializedTiles;

        [HideInInspector]
        public int Width, Height;

        [HideInInspector]
        public TdTile TilePrefab;

        public TdTile this[int x, int y]
        {
            get => _serializedTiles[y * Width + x];
            set => _serializedTiles[y * Width + x] = value;
        }

        public void Start()
        {
            Profiler.BeginSample("Pathfinding");
            //CalculateWaypoints();
            Profiler.EndSample();
        }

        public void RecreateTiles()
        {
            _serializedTiles = new TdTile[Width * Height];
            var tilesGo = GameObject.Find("Tiles");

            for (int i = tilesGo.transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(tilesGo.transform.GetChild(i).gameObject);
            }
        }

        public void ReAssignTiles()
        {
            Debug.Log("Reassigning tiles");
            Debug.Log($"Width: {Width}");
            Debug.Log($"Height: {Height}");

            _serializedTiles = new TdTile[Width * Height];
            var tilesGo = GameObject.Find("Tiles");

            float minX = -(Width - 1) / 2f;
            float minY = -(Height - 1) / 2f;
            
            Debug.Log($"minX: {minX}");
            Debug.Log($"minY: {minY}");
            
            foreach (var tile in tilesGo.GetComponentsInChildren<TdTile>())
            {
                var position = tile.transform.position;

                var indexX = Mathf.RoundToInt(position.x - minX);
                var indexY = Mathf.RoundToInt(position.y - minY);
                
                // Debug.Log($"position.x: {position.x}");
                // Debug.Log($"position.y: {position.y}");
                // Debug.Log($"indexX: {indexX}");
                // Debug.Log($"indexY: {indexY}");
                
                this[indexX, indexY] = tile;
            }
        }
    }
}
