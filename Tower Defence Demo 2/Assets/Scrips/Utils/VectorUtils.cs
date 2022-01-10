using UnityEngine;

namespace Scrips.Utils
{
    public static class VectorUtils
    {
        // ReSharper restore Unity.ExpensiveCode
        public static float Distance2D(this Vector3 from, Vector3 to)
        {
            var vFrom = new Vector2(from.x, from.y);
            var vTo = new Vector2(to.x, to.y);

            return Vector2.Distance(vFrom, vTo);
        }

        public static float SquareDistance2D(this Vector3 from, Vector3 to)
        {
            var xDistance = from.x - to.y;
            var yDistance = from.y - to.y;

            return xDistance * xDistance + yDistance * yDistance;
        }
    }
}