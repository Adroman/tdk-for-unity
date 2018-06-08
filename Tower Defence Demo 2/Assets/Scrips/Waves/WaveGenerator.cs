using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Scrips.Waves
{
    [Serializable]
    [PublicAPI]
    public class WaveNumber
    {
        public int Number;
        public Wave Wave;

        public WaveNumber(int number, Wave wave)
        {
            Number = number;
            Wave = wave;
        }
    }

    public class WaveGenerator : MonoBehaviour
    {
        public int CountdownForEachWave;
        public List<BaseEnemyGenerationModifiers> EnemiesToUse = new List<BaseEnemyGenerationModifiers>();
        public int WavesTotal;
        public int MinClusters;
        public int MaxClusters;
        public int BaseDifficulty;
        public bool Infinite;
        public float DifficultyIncrease;
        [Range(0, 1)]
        public float DifficultyDeviation;
        public string RandomSeed;

        public List<Transform> SpawnpointsToUse = new List<Transform>();
        public List<int> ClusterIncreases = new List<int>();
        public List<WaveNumber> Waves = new List<WaveNumber>();

        private int _currentWave;
        private int _currentDifficulty;
        private System.Random _random;

        public IEnumerable<Wave> GetWaves()
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

        private Wave GetNextWave()
        {
            _currentWave++;
            _currentDifficulty = (int) (_currentDifficulty * DifficultyIncrease);
            if (ClusterIncreases.Contains(_currentWave)) MaxClusters++;
            int clusters = _random.Next(MinClusters, MaxClusters + 1);

            var wave = Waves.FirstOrDefault(w => w.Number == _currentWave)?.Wave;

            if (wave != null) return wave;

            wave = new Wave
            {
                Countdown = CountdownForEachWave,
                Spawnpoints = SpawnpointsToUse
            };

            for (int i = 0; i < clusters; i++)
            {
                wave.WaveClusters.Add(GenerateWaveCluster(clusters));
            }

            return wave;
        }

        private WaveCluster GenerateWaveCluster(int clusterCount)
        {
            float ratio = 1f / clusterCount;
            return EnemiesToUse[_random.Next(0, EnemiesToUse.Count)]
                .GenerateCluster(_currentDifficulty, ratio, _random);
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
            _random = new System.Random(RandomSeed.GetHashCode());
        }
    }
}