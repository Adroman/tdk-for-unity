using UnityEngine;

namespace Data
{
    public class TileWithDistance
    {
        public GameObject gameObject;
        public float distance;

        public TileWithDistance(GameObject go, float distance)
        {
            gameObject = go;
            this.distance = distance;
        }
    }
}
