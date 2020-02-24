using System.Collections.Generic;
using System.Linq;
using Scrips.Towers.Specials;
using UnityEngine;

namespace Scrips.Pooling
{
    public static class PoolManager
    {
        private static readonly Dictionary<PoolComponent,Pool> Pools = new Dictionary<PoolComponent, Pool>();

        public static PoolComponent Spawn(PoolComponent original, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return GetOrCreatePool(original).Spawn(position, rotation, parent);
        }

        public static void Despawn(PoolComponent poolObject)
        {
            foreach (var component in poolObject.GetComponents<SpecialComponent>().ToList())
            {
                Object.Destroy(component);
            }
            poolObject.Pool.Despawn(poolObject);
        }

        private static Pool GetOrCreatePool(PoolComponent poolPrefab)
        {
            if (Pools.TryGetValue(poolPrefab, out var result))
            {
                return result;
            }

            // new pool
            result = new Pool(poolPrefab);
            Pools.Add(poolPrefab, result);
            return result;
        }
    }
}