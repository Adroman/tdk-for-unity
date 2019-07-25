using System;
using Scrips;
using UnityEngine;

namespace Data
{
    public class TileWithCoordinates : IEquatable<TileWithCoordinates>
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

        public bool Equals(TileWithCoordinates other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Tile == other.Tile
                   && X == other.X
                   && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TileWithCoordinates);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Tile != null ? Tile.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ X;
                hashCode = (hashCode * 397) ^ Y;
                return hashCode;
            }
        }

        public static bool operator ==(TileWithCoordinates a, TileWithCoordinates b)
            => ReferenceEquals(a, b) || !ReferenceEquals(a, null) && a.Equals(b);

        public static bool operator !=(TileWithCoordinates a, TileWithCoordinates b) => !(a == b);
    }
}
