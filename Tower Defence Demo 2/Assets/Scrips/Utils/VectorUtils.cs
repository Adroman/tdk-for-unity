using UnityEngine;

namespace Scrips.Utils
{
    public static class VectorUtils
    {
        public static float Distance2D(this Vector3 from, Vector3 to)
        {
            var vFrom = new Vector2(from.x, from.y);
            var vTo = new Vector2(from.x, from.y);

            return Vector2.Distance(vFrom, vTo);
        }
    }
}