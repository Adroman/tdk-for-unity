using System;
using UnityEngine;

namespace Data
{
    public class TileWithCoordinates : IEquatable<TileWithCoordinates>
    {
        public readonly GameObject gameObject;
        public readonly int x;
        public readonly int y;

        public TileWithCoordinates(int x, int y, GameObject go)
        {
            gameObject = go;
            this.x = x;
            this.y = y;
        }

        public bool Equals(TileWithCoordinates other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return gameObject == other.gameObject
                   && x == other.x
                   && y == other.y;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as TileWithCoordinates);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (gameObject != null ? gameObject.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ x;
                hashCode = (hashCode * 397) ^ y;
                return hashCode;
            }
        }

        public static bool operator ==(TileWithCoordinates a, TileWithCoordinates b)
            => ReferenceEquals(a, b) || !ReferenceEquals(a, null) && a.Equals(b);

        public static bool operator !=(TileWithCoordinates a, TileWithCoordinates b) => !(a == b);
    }
}
