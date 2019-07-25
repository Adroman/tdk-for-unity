using System;
using System.Collections.Generic;
using Scrips.CustomTypes.IncreaseType;
using UnityEngine;

namespace Scrips.Modifiers.Stats
{
    [Serializable]
    public class IntModifiableStat
    {
        [SerializeField]
        private int _baseValue;

        [SerializeField]
        private int _modifiedValue;

        [SerializeField]
        private bool _isDirty;

        public int Value
        {
            get
            {
                if (_isDirty)
                {
                    _modifiedValue = CalculateModifiedAmount();
                    _isDirty = false;
                }

                return _modifiedValue;
            }
            set
            {
                _baseValue = value;
                _isDirty = true;
            }
        }

        public int BaseValue => _baseValue;

        public void AddModifier(BaseModifier modifier)
        {
            _modifiers.Add(modifier);
            _isDirty = true;
        }

        public void RemoveModifier(BaseModifier modifier)
        {
            _isDirty = _modifiers.Remove(modifier);
        }

        [SerializeField]
        private List<BaseModifier> _modifiers = new List<BaseModifier>();

        private int CalculateModifiedAmount()
        {
            float percentageAmount = 0;
            float flatAmount = 0;

            foreach (var modifier in _modifiers)
            {
                switch (modifier.IncreaseType)
                {
                    case MultiplicativeIncreaseType _:
                        percentageAmount += modifier.Amount * modifier.Level;
                        break;
                    case AdditiveIncreaseType _:
                        flatAmount += modifier.Amount * modifier.Level;
                        break;
                }
            }

            percentageAmount = Math.Max(-1, percentageAmount);    // if we get -150% modifier, we just zero it out to -100%

            return Mathf.RoundToInt(_baseValue * (1 + percentageAmount) + flatAmount);
        }
    }
}