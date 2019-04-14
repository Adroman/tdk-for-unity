using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Scrips.CustomTypes;
using Scrips.EnemyData.Instances;
using Scrips.Events;
using Scrips.Events.Enemies;
using Scrips.Events.Waves;
using Scrips.UI;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Waves
{
    [RequireComponent(typeof(IntCountdown))]
    [RequireComponent(typeof(WaveGenerator))]
    public class WaveController : MonoBehaviour
    {
        public LogLevel LogLevel;

        public WaveEvent OnWaveStarted;

        public EnemyEvent OnEnemySpawned;

        public IntVariable ActiveEnemies;

        public IntVariable EnemiesWaiting;

        public IntVariable WaveIndex;

        public BooleanVariable LastWave;

        public int QueueLength;

        public bool ActivateWavesAtStart;

        public string RandomSeed;

        public UiWaveQueue UiWaves;

        private WaveGenerator _generator;

        private System.Random _random;

        private Coroutine _waveCountdown;

        private Queue<Wave> _wavesQueue;

        private IEnumerator<Wave> _wavesEnumerator;

        private int _waveNumber;

        private Wave _nextWaveWaiting;

        private IntCountdown _countdownVariable;

        public Wave[] WavesQueue => _wavesQueue.ToArray();

        private void Start()
        {
            _generator = GetComponent<WaveGenerator>();
            _wavesQueue = new Queue<Wave>();
            _wavesEnumerator = _generator.GetWaves().GetEnumerator();
            _waveNumber = 0;
            _countdownVariable = GetComponent<IntCountdown>();

            if (WaveIndex != null) WaveIndex.Value = 0;
            if (EnemiesWaiting != null) EnemiesWaiting.Value = 0;
            if (LastWave != null) LastWave.Value = false;

            InitQueue();

            _random = new System.Random(RandomSeed.GetHashCode());
            if (ActivateWavesAtStart)
            {
                ActivateWaves();
            }
        }

        private void InitQueue()
        {
            for (int i = 0; i < QueueLength - 1; i++)
            {
                if (_wavesEnumerator.MoveNext())
                {
                    _wavesQueue.Enqueue(_wavesEnumerator.Current);
                    UiWaves.SpawnWave(_wavesEnumerator.Current, false);
                }
            }
        }

        private bool TryGetNextWave(out Wave result)
        {
            result = null;
            if (_wavesQueue.Count == 0)
            {
                if (LastWave != null) LastWave.Value = true;
                return false;
            }

            if (_wavesEnumerator.MoveNext())
            {
                _wavesQueue.Enqueue(_wavesEnumerator.Current);
                UiWaves.SpawnWave(_wavesEnumerator.Current, _waveNumber > 0);
            }

            result = _wavesQueue.Dequeue();
            return true;
        }

        public void CallWave()
        {
            if (_nextWaveWaiting != null)
            {
                StartCoroutine(SpawnWave(_nextWaveWaiting, true));
            }

            if (_nextWaveWaiting != null)
            {
                StartCoroutine(ActivateWave());
            }
        }

        private void ActivateWaves()
        {
            if (TryGetNextWave(out _nextWaveWaiting))
                StartCoroutine(ActivateWave());
        }

        private IEnumerator ActivateWave()
        {
            while (true)
            {
                int waveNumber = _waveNumber;

                _countdownVariable.StartCountdown(_nextWaveWaiting.Countdown);
                yield return new WaitForSeconds(_nextWaveWaiting.Countdown);

                if (waveNumber == _waveNumber)
                {
                    StartCoroutine(SpawnWave(_nextWaveWaiting, false));
                    if (_nextWaveWaiting == null) yield break;
                }
                else
                {
                    yield break;
                }
            }
        }

        private IEnumerator SpawnWave(Wave wave, bool calledEarly)
        {
            _waveNumber++;
            UiWaves.DespawnTopWave();
            if (WaveIndex != null) WaveIndex.Value++;
            TryGetNextWave(out _nextWaveWaiting);
            if (EnemiesWaiting != null) EnemiesWaiting.Value += wave.WaveClusters.Sum(c => c.Amount);

            wave.CalledEarly = calledEarly;

            if (OnWaveStarted != null) OnWaveStarted.Invoke(wave);

            for (int clusterIndex = 0; clusterIndex < wave.WaveClusters.Count; clusterIndex++)
            {
                var cluster = wave.WaveClusters[clusterIndex];

                if (!cluster.SpawnWithPreviousCluster)
                {
                    foreach (var time in SpawnCluster(wave, clusterIndex))
                    {
                        yield return time;
                    }
                }
                else
                {
                    StartCoroutine(SpawnCluster(wave, clusterIndex).GetEnumerator());
                }
            }
        }

        private IEnumerable SpawnCluster(Wave wave, int clusterIndex)
        {
            var cluster = wave.WaveClusters[clusterIndex];
            yield return new WaitForSeconds(cluster.InitialCountDown);
            for (int i = 0; i < cluster.Amount; i++)
            {
                if (i > 0)
                    yield return new WaitForSeconds(
                        Utils.Utils.GetDeviatedValue(
                            cluster.Interval,
                            cluster.IntervalDeviation,
                            _random));

                var spawnpoint = wave.GetRandomSpawnpoint(clusterIndex, _random);
                SpawnEnemy(spawnpoint, cluster);
            }
        }

        private void SpawnEnemy(Transform spawnpoint, WaveCluster cluster)
        {
            var enemy = Instantiate(cluster.Prefab, GameObject.Find("Enemies").transform, spawnpoint);
            enemy.transform.position = spawnpoint.position;
            enemy.WaveNumber = cluster.WaveNumber;
            enemy.SetSpawnPoint(spawnpoint, true);
            cluster.EnemyData.SetEnemy(enemy, _random);
            if (OnEnemySpawned != null) OnEnemySpawned.Invoke(enemy);
            if (EnemiesWaiting != null) EnemiesWaiting.Value--;
        }
    }
}
