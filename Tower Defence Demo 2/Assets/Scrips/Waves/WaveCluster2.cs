using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scrips.Waves
{
    [Serializable]
    public class WaveCluster2
    {
        // cluster info
        public int Amount;
        public float Interval;
        public float InitialCountDown;
        public bool SpawnWithPreviousCluster;

        [Range(0, 1)]
        public float IntervalDeviation;

        public List<Transform> OverrideSpawnpoints;

        // Enemy info
        public GameObject Prefab;

        public EnemyWaveData EnemyData;
    }
}