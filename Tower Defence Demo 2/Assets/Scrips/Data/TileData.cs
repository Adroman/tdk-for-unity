using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scrips.Data
{
    [Serializable]
    public class TileData
    {
        public string Name;
        public GameObject Prefab;
    }

    [CreateAssetMenu]
    public class TileDatabase : ScriptableObject
    {

        public List<TileData> Tiles = new List<TileData>();
    }
}