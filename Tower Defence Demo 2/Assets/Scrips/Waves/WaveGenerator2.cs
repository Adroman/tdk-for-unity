using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Scrips.Waves
{
    [Serializable]
    public struct WaveNumber
    {
        public int Number;
        public Wave2 Wave;

        public WaveNumber(int number, Wave2 wave)
        {
            Number = number;
            Wave = wave;
        }
    }

    [CreateAssetMenu]
    public class WaveGenerator2 : ScriptableObject
    {
        public List<GameObject> Prefabs = new List<GameObject>();
        public int WavesTotal;
        public int MinClusters;
        public int MaxClusters;
        public int BaseDifficulty;
        public bool Infinite;
        public float DifficultyIncrease;
        [Range(0, 1)]
        public float DifficultyDeviation;
        public string RandomSeed;

        public List<int> ClusterIncreases;
        public List<WaveNumber> Waves = new List<WaveNumber>();

        private int _currentWave;
        private int _currentClusterCount;
        private int _currentDifficulty;
        private System.Random _random;

        public IEnumerable<Wave2> GetWaves()
        {
            if (!Infinite)
            {
                foreach (var wave in Waves.OrderBy(w => w.Number).Select(w => w.Wave))
                {
                    yield return wave;
                }
            }
            else
            {
                ResetPrivateStats();
                while (true)
                {
                    yield return GetNextWave();
                }
            }
        }

        public Wave2 GetNextWave()
        {
            _currentWave++;
            _currentDifficulty = (int) (_currentDifficulty * DifficultyIncrease);
            if (ClusterIncreases.Contains(_currentWave)) _currentClusterCount++;
            int clusters = _random.Next(MinClusters, _currentClusterCount + 1);

            var wave = new Wave2();

            for (int i = 0; i < clusters; i++)
            {
                wave.WaveClusters.Add(GenerateWaveCluster(clusters));
            }

            return wave;
        }

        public WaveCluster2 GenerateWaveCluster(int clusterCount)
        {
            return new WaveCluster2();
        }

        public void GenerateWaves()
        {
            if (Infinite) return;

            ResetPrivateStats();
            Waves.Clear();

            for (int i = 0; i < WavesTotal; i++)
            {
                Waves.Add(new WaveNumber(i + 1, GetNextWave()));
            }
        }

        private void ResetPrivateStats()
        {
            _currentWave = 0;
            _currentDifficulty = BaseDifficulty;
            _currentClusterCount = MinClusters;
            _random = new System.Random(RandomSeed.GetHashCode());
        }
    }
}