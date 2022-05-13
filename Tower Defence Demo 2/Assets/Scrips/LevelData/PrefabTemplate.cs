using System.Collections.Generic;
using UnityEngine;

namespace Scrips.LevelData
{
    [CreateAssetMenu(menuName = "Tower defense kit/Levels/Prefab Template")]
    public class PrefabTemplate : ScriptableObject
    {
        public List<GameObject> PrefabsToInstantiate;
    }
}