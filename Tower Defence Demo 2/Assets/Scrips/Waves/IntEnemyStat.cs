using System;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

namespace Scrips.Waves
{
    [Serializable]
    public struct IntEnemyStat
    {
        [PublicAPI]
        public bool UseBaseValue;

        [PublicAPI]
        public int BaseValue;

        [PublicAPI]
        public float ModifierFromDifficulty;

        [PublicAPI]
        [Range(0, 1)]
        public float Deviation;

        [PublicAPI]
        public int GenerateValue(int difficulty, Random rand)
        {
            if (UseBaseValue) return BaseValue;

            return (int) Utils.Utils.GetDeviatedValue(difficulty * ModifierFromDifficulty, Deviation, rand);
        }
    }
}