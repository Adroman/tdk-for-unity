using System.Collections.Generic;
using UnityEngine;

namespace Scrips.Data
{
    [CreateAssetMenu(menuName = "Tower defense kit/Tile database")]
    public class TileDatabase : ScriptableObject
    {

        public List<TileData> Tiles = new List<TileData>();
    }
}