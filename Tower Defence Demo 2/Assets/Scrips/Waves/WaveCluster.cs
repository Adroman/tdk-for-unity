using System;
using System.Collections.Generic;
using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Waves
{
    [Serializable]
    public class WaveCluster
    {
        // cluster info
        public int Amount;
        public float Interval;
        public float InitialCountDown;
        public bool SpawnWithPreviousCluster;

        [Range(0, 1)]
        public float IntervalDeviation;

        public List<Transform> OverrideSpawnpoints = new List<Transform>();

        // Enemy info
        public EnemyInstance Prefab;

        public EnemyWaveData EnemyData;
    }
}