using System;
using Boo.Lang;
using Scrips.Data;
using UnityEngine;

namespace Scrips.Waves
{
    [Serializable]
    public class EnemyWaveData
    {
        public float InitialHitpoints;
        [Range(0, 1)]
        public float HitpointsDeviation;

        [Space(5)]
        public float InitialArmor;
        [Range(0, 1)]
        public float ArmorDeviation;

        [Space(5)]
        public float InitialSpeed;
        [Range(0, 1)]
        public float SpeedDeviation;

        public List<IntCurrency> IntLoot;

        public List<IntCurrency> IntPunishments;
    }
}