using System.Collections.Generic;
using Scrips.Data;
using Scrips.Towers.BaseData;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Modifiers
{
    public class ModifierController : MonoBehaviour
    {
        public int Version; // make it private

        [SerializeField]
        private List<BaseModifier> _allModifiers;

        public IReadOnlyList<BaseModifier> Modifiers => _allModifiers;

        public void AddModifier(BaseModifier modifier)
        {
            _allModifiers.Add(modifier);
            Version++;
        }

        public void RemoveModifier(BaseModifier modifier)
        {
            if (_allModifiers.Remove(modifier))
            {
                Version++;
            }
        }
    }
}