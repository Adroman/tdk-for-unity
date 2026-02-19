using System;
using Scrips;
using UnityEngine;

namespace Data
{
    public readonly struct TileWithCoordinates : IEquatable<TileWithCoordinates>
    {
        public readonly TdTile Tile;
        public readonly int X;
        public readonly int Y;

        public TileWithCoordinates(int x, int y, TdTile tile)
        {
            Tile = tile;
            X = x;
            Y = y;
        }

        public bool Equals(TileWithCoordinates other) =>
            Tile == other.Tile
            && X == other.X
            && Y == other.Y;

        public override bool Equals(object obj) => 
            obj is TileWithCoordinates other && Equals(other);

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Tile != null ? Tile.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ X;
                hashCode = (hashCode * 397) ^ Y;
                return hashCode;
            }
        }

        public static bool operator ==(TileWithCoordinates a, TileWithCoordinates b)
            => a.Equals(b);

        public static bool operator !=(TileWithCoordinates a, TileWithCoordinates b) => !(a == b);
    }
}
