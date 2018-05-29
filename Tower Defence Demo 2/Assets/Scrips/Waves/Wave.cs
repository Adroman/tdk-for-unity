using System;
using System.Collections;
using System.Collections.Generic;
using Scrips.EnemyData.WaveData;
using UnityEngine;

namespace Scrips.Waves
{
    [Serializable]
    public class Wave : MonoBehaviour
    {
        public float Countdown;

        public List<WaveCluster> WaveClusters;
        public List<Transform> SpawnPoints;

        public string Seed = "";

        public Wave()
        {
            Countdown = 0;
            WaveClusters = new List<WaveCluster>();
            SpawnPoints = new List<Transform>();
        }

        private System.Random _random;

        private static Transform _enemiesParent;

        private static Transform EnemiesParent
        {
            get
            {
                if (_enemiesParent == null)
                    _enemiesParent = GameObject.Find("Enemies").transform;
                return _enemiesParent;
            }
        }

        public void Awake()
        {
            enabled = false;
        }

        public void ActivateWave()
        {
            enabled = true;

            StartCoroutine(SpawnEnemies());
        }

        public IEnumerator SpawnEnemies()
        {
            _random = new System.Random(Seed.GetHashCode());
            foreach (var cluster in WaveClusters)
            {
                ScoreManager.Instance.EnemiesRemaining += cluster.Amount;

                yield return new WaitForSeconds(cluster.InitialCountDown);
                for (int i = 0; i < cluster.Amount - 1; i++)
                {
                    SpawnEnemy(cluster);
                    yield return new WaitForSeconds(GetDeviatedValue(cluster.Interval, cluster.IntervalDeviation));
                }
                // spawn last enemy
                SpawnEnemy(cluster);
            }
        }

        private void SpawnEnemy(WaveCluster clusterInfo)
        {
            var sample = clusterInfo.SampleEnemy;
            var sampleComponent = sample.GetComponent<BaseWaveData>();

            var spawnPoint = SelectSpawnPoint(clusterInfo);
            var enemy = Instantiate(clusterInfo.SampleEnemy, spawnPoint.position, spawnPoint.rotation, EnemiesParent.transform).GetComponent<global::Scrips.EnemyData.Instances.EnemyInstance>();

            enemy.SetSpawnPoint(spawnPoint, true);
            enemy.gameObject.SetActive(true);
        }

        private float GetDeviatedValue(float baseValue, float deviation)
        {
            return baseValue * (GetRandomNumber(1 - deviation, 1 + deviation));
        }

        private Transform SelectSpawnPoint(WaveCluster cluster)
        {
            // if we have overrides, use those
            if (cluster != null && cluster.OverrideSpawnpoints != null && cluster.OverrideSpawnpoints.Count > 0)
                return SelectSpawnPoint(cluster.OverrideSpawnpoints);

            // otherwise use wave's
            return SelectSpawnPoint(SpawnPoints);
        }

        private Transform SelectSpawnPoint(IReadOnlyList<Transform> spawnpoints)
        {
            if (spawnpoints == null || spawnpoints.Count == 0)
                Debug.LogError("ERROR: No spawnpoint for selected wave/cluster");
            int index = _random.Next(0, spawnpoints.Count);
            return spawnpoints[index];
        }

        // origin: https://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers
        private static float GetRandomNumber(float minimum, float maximum, System.Random rand = null)
        {
            if (rand == null) rand = new System.Random();
            return (float)rand.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
