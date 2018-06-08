using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Scrips.Waves
{
    [Serializable]
    [PublicAPI]
    public class Wave
    {
        public int Countdown;
        public List<WaveCluster> WaveClusters = new List<WaveCluster>();
        public List<Transform> Spawnpoints = new List<Transform>();

        public Transform GetRandomSpawnpoint(int clusterIndex, System.Random random)
        {
            var cluster = WaveClusters[clusterIndex];
            return cluster.OverrideSpawnpoints.Count > 0
                ? cluster.OverrideSpawnpoints[random.Next(0, cluster.OverrideSpawnpoints.Count)]
                : Spawnpoints[random.Next(0, Spawnpoints.Count)];
        }
    }
}