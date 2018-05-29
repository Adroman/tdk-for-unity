using System;
using System.Collections.Generic;
using Scrips.EnemyData.AutoGenerateModifers;
using UnityEngine;

namespace Scrips.Waves
{
    [Serializable]
    public class WaveGenerator
    {
        public List<GameObject> Prefabs = new List<GameObject>();
        public int WavesTotal;
        public int MinClusters;
        public int MaxClusters;
        public int BaseDifficulty;
        public float DifficultyIncrease;
        public float DifficultyDeviation;
        public string Seed;

        private System.Random _random;
        private int _currentDifficulty;
        private WaveController _waveController;

        private WaveController WaveController
        {
            get
            {
                if (_waveController == null) _waveController = GameObject.Find("Level/Waves").GetComponent<WaveController>();
                return _waveController;
            }
        }

        public List<Wave> GenerateWaves()
        {
            _currentDifficulty = BaseDifficulty;
            _random = string.IsNullOrEmpty(Seed) ? new System.Random() : new System.Random(Seed.GetHashCode());

            var waves = new List<Wave>();
            for (int i = 0; i < WavesTotal; i++)
            {
                int clusterCount = _random.Next(MinClusters, MaxClusters + 1);
                var wave = InstantiateNewWave();
                wave.WaveClusters = new List<WaveCluster>();
                for (int j = 0; j < clusterCount; j++)
                {
                    wave.WaveClusters.Add(GenerateWaveCluster(GetDeviatedValue(_currentDifficulty / clusterCount, DifficultyDeviation), clusterCount, wave));
                }
                _currentDifficulty = (int) (_currentDifficulty * DifficultyIncrease);
                waves.Add(wave);
            }

            return waves;
        }

        private int GetDeviatedValue(int baseValue, double deviation)
        {
            double randomValue = 1 + (_random.NextDouble() * deviation - deviation);

            return (int) (baseValue * deviation);
        }

        private WaveCluster GenerateWaveCluster(int difficulty, int clusters, Wave wave)
        {
            int index = _random.Next(0, Prefabs.Count);

            var prefab = Prefabs[index];

            var modifier = prefab.GetComponent<BaseAutoGenerateModifiers>();

            return modifier.GenerateCluster(difficulty, clusters, wave, _random);
        }

        private Wave InstantiateNewWave()
        {
            var go = new GameObject();
            go.transform.parent = WaveController.transform;
            go.name = "wave";
            return go.AddComponent<Wave>();
        }
    }
}