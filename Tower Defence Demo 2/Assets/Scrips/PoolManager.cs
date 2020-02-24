using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Towers.Specials;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scrips
{
    [Obsolete]
    public static class PoolManager
    {
        private static readonly Dictionary<GameObject,Pool> Pools = new Dictionary<GameObject, Pool>();

        public static GameObject Spawn(GameObject original, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            return GetOrCreatePool(original).Spawn(position, rotation, parent);
        }

        public static void Despawn(GameObject go)
        {
            var poolComponent = go.GetComponent<PoolComponent>();

            if (poolComponent == null)
            {
                Debug.LogWarning($"Invalid pooling object: {go.name}. Destroying instead");
                Object.Destroy(go);
            }
            else
            {
                foreach (var component in go.GetComponents<SpecialComponent>().ToList())
                {
                    Object.Destroy(component);
                }
                poolComponent.Pool.Despawn(go);
            }
        }

        private static Pool GetOrCreatePool(GameObject prefab)
        {
            Pool result;
            if (Pools.TryGetValue(prefab, out result))
            {
                return result;
            }

            // new pool
            result = new Pool(prefab);
            Pools.Add(prefab, result);
            return result;
        }

        private class Pool
        {
            private int _index = 1;
            private readonly Stack<GameObject> _inactiveObjects = new Stack<GameObject>();
            private readonly GameObject _prefab;

            public Pool(GameObject prefab)
            {
                _prefab = prefab;
            }

            public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
            {
                GameObject go = null;

                while (_inactiveObjects.Count > 0 && (go = _inactiveObjects.Pop()) == null)
                {
                    // do nothing, just pop those items, until we find valid item or empty the stack.
                    // invalid objects could be destroyed by Object.Destroy method somewhere else.
                }

                if (go == null)
                {
                    // we didn't find the object to reuse
                    go = Object.Instantiate(_prefab, position, rotation);
                    go.name = $"{_prefab.name} ({_index++})";
                    go.AddComponent<PoolComponent>().Pool = this;
                }

                go.transform.position = position;
                go.transform.rotation = rotation;
                go.transform.parent = !ReferenceEquals(parent,null) ? parent : go.transform;
                go.SetActive(true);
                return go;
            }


            public void Despawn(GameObject go)
            {
                go.SetActive(false);
                _inactiveObjects.Push(go);
            }
        }

        private class PoolComponent : MonoBehaviour
        {
            public Pool Pool;
        }
    }
}
