using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Scrips.Data;
using Scrips.EnemyData.WaveData;
using Scrips.Waves;
using UnityEngine;

namespace Scrips.EnemyData.AutoGenerateModifers
{
    [PublicAPI]
    public class BaseAutoGenerateModifiers : MonoBehaviour
    {
        private void Start()
        {
            _spawnpoints = GameObject.Find("SpawnPoints").transform;
        }

        public virtual WaveCluster GenerateCluster(int difficulty, int clusters, Wave wave, System.Random random)
        {
            float bigHp = (float) (difficulty * (random.NextDouble() + 2) / 5);    // take a random number between 2/5 and 3/5 from difficulty
            bigHp *= HitpointsModifiers;

            float bigArmor = (difficulty - bigHp) / 10;                // rest of the difficulty goes to Armor
            bigArmor *= ArmorModifiers;

            int amount = random.Next(MinimumAmount, MaximumAmount + 1) / clusters + random.Next(0, 1);
            amount = Math.Max(amount, 1);        // at least one enemy

            float actualHp = bigHp / amount;
            float actualArmor = bigArmor / amount;
            float loot = difficulty * LootModifiers / amount / 5;

            var cluster = Utils.Utils.InstantiateWaveCluster(wave);

            cluster.Prefab = gameObject;

            var sample = cluster.SampleEnemy;

            var waveData = sample.GetComponent<BaseWaveData>();

            waveData.ArmorDeviation = 0;
            waveData.HitpointsDeviation = 0;
            waveData.SpeedDeviation = 0;
            waveData.InitialHitpoints = actualHp;
            waveData.InitialArmor = actualArmor;
            waveData.InitialSpeed = SpeedModifiers;

            waveData.IntLoot = new List<IntCurrency>();
            foreach (var lootCurrency in BaseIntLoot)
            {
                var newLoot = new IntCurrency(lootCurrency);
                newLoot.ModifyAmount(loot);
                waveData.IntLoot.Add(newLoot);
            }

            cluster.Amount = amount;
            cluster.Interval = IntervalModifiers;
            cluster.InitialCountDown = IntervalModifiers;
            cluster.SpawnWithPreviousCluster = false;

            if (_spawnpoints == null) _spawnpoints = GameObject.Find("SpawnPoints").transform;

            wave.Countdown = cluster.Interval * (cluster.Amount + 2);
            wave.SpawnPoints.Add(_spawnpoints.GetChild(random.Next(0, _spawnpoints.childCount)));

            return cluster;
        }

        private Transform _spawnpoints;

        public IntCurrency[] BaseIntLoot;

        public float HitpointsModifiers;
        public float ArmorModifiers;
        public float SpeedModifiers;
        public float LootModifiers;

        public int MinimumAmount;
        public int MaximumAmount;

        public float IntervalModifiers;
    }
}