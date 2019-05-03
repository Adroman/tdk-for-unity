using UnityEngine;

namespace Scrips.Data
{
    [CreateAssetMenu(menuName = "Tower defense kit/Tile data")]
    public class TileData : ScriptableObject
    {
        public string Name;
        public GameObject Prefab;
    }
}