using System.Collections.Generic;
using UnityEngine;

namespace Scrips.Pooling
{
    public class Pool
    {
        private int _index = 1;
        private readonly Stack<PoolComponent> _inactiveObjects = new Stack<PoolComponent>();
        private readonly PoolComponent _prefabComponent;

        public Pool(PoolComponent prefab)
        {
            _prefabComponent = prefab;
        }

        public PoolComponent Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            PoolComponent poolObject = null;

            while (_inactiveObjects.Count > 0 && (poolObject = _inactiveObjects.Pop()) == null)
            {
                // do nothing, just pop those items, until we find valid item or empty the stack.
                // invalid objects could be destroyed by Object.Destroy method somewhere else.
            }

            if (ReferenceEquals(poolObject, null))
            {
                // we didn't find the object to reuse
                poolObject = Object.Instantiate(_prefabComponent.gameObject, position, rotation).GetComponent<PoolComponent>();
                poolObject.gameObject.name = $"{_prefabComponent.name} ({_index++})";
                poolObject.Pool = this;
            }

            var gameObject = poolObject.gameObject;

            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;
            if (!ReferenceEquals(parent, null)) gameObject.transform.parent = parent;
            gameObject.SetActive(true);
            return poolObject;
        }


        public void Despawn(PoolComponent poolObject)
        {
            poolObject.gameObject.SetActive(false);
            _inactiveObjects.Push(poolObject);
        }
    }
}