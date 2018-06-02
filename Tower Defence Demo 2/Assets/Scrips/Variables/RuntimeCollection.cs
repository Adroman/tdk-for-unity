using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Scrips.Variables
{
    [PublicAPI]
    public class RuntimeCollection<T> : ScriptableObject
    {
        public List<T> Instances;

        public IntVariable Count;

        public void AddInstance(T instance)
        {
            if (!Instances.Contains(instance))
            {
                Instances.Add(instance);
                UpdateCount();
            }
        }

        public void RemoveInstance(T instance)
        {
            if (Instances.Remove(instance))
            {
                UpdateCount();
            }
        }

        private void UpdateCount()
        {
            if (Count != null && Count.Value != Instances.Count) Count.Value = Instances.Count;
        }
    }
}