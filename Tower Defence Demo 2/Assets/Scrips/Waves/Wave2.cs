using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scrips.Waves
{
    [Serializable]
    public class Wave2
    {
        public List<WaveCluster2> WaveClusters;
        public List<Transform> SpawnPoints;
    }
}