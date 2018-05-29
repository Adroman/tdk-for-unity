using System;
using UnityEngine;

namespace Scrips.CustomTypes
{
    public abstract class Amount
    {
        private float Value { get; }

        public static Amount Flat(float value) => new FlatAmount(value);

        public static Amount Percenatage(float value) => new PercentageAmount(value);

        public abstract float AddFrom(float fromValue);

        public abstract float SubstractFrom(float fromValue);

        private Amount(float value)
        {
            if (value <= 0) throw new ArgumentException("Value has to be a positive number");
            Value = value;
        }



        private sealed class FlatAmount : Amount
        {
            public FlatAmount(float value) : base(value)
            {
            }

            public override float AddFrom(float fromValue) => fromValue + Value;

            public override float SubstractFrom(float fromValue)
            {
                //Debug.Log($"{fromValue} - {Value}");

                return Mathf.Max(0, fromValue - Value);
            }
        }



        private sealed class PercentageAmount : Amount
        {
            public PercentageAmount(float value) : base(value)
            {
            }

            public override float AddFrom(float fromValue) => fromValue + fromValue * Value;

            public override float SubstractFrom(float fromValue)
            {
                //Debug.Log($"{fromValue} * {1 - Value}");
                return Mathf.Max(0, fromValue - fromValue * Value);
            }
        }
    }
}