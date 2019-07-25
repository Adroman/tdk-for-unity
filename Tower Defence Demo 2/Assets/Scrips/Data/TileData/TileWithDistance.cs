using Scrips;
using UnityEngine;

namespace Data
{
    public class TileWithDistance
    {
        public readonly TdTile Tile;
        public readonly float Distance;

        public TileWithDistance(TdTile tile, float distance)
        {
            Tile = tile;
            Distance = distance;
        }
    }
}
