using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrips.Waves
{
    [Serializable]
    public class WaveSpawner : MonoBehaviour
    {
        public float Countdown = 20f;

        [HideInInspector]
        public List<WaveCluster> Spawns;
        [HideInInspector]
        public List<GameObject> SpawnPoints;

        private static GameObject _enemiesParent;

        private static GameObject EnemiesParent
        {
            get
            {
                if (_enemiesParent == null)
                    _enemiesParent = GameObject.Find("Enemies");
                return _enemiesParent;
            }
        }

        void Awake()
        {
            enabled = false;
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ActivateWave()
        {
            enabled = true;

            StartCoroutine(SpawnEnemies());
        }

        public IEnumerator SpawnEnemies()
        {
            foreach(var spawn in Spawns)
            {
                ScoreManager.Instance.EnemiesRemaining += spawn.Amount;

                yield return new WaitForSeconds(spawn.InitialCountDown);
                for(int i = 0; i < spawn.Amount - 1; i++)
                {
                    SpawnEnemy(spawn.Prefab);
                    yield return new WaitForSeconds(spawn.Interval);
                }
                // spawn last enemy
                SpawnEnemy(spawn.Prefab);
            }
        }

        private void SpawnEnemy(GameObject prefabToSpawn)
        {
            var spawnPoint = SelectSpawnPoint();
            var enemy = Instantiate(prefabToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation, EnemiesParent.transform).GetComponent<Scrips.EnemyData.Instances.EnemyInstance>();
            enemy.SetSpawnPoint(spawnPoint.transform, true);
        }

        private GameObject SelectSpawnPoint()
        {
            int index = (int)(SpawnPoints.Count * UnityEngine.Random.value);
            index = System.Math.Min(index, SpawnPoints.Count);
            return SpawnPoints[index];
        }
    }
}
