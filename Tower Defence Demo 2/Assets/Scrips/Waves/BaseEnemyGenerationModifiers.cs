using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Data;
using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Waves
{
    [CreateAssetMenu(menuName = "Tower defense kit/Data/Enemy Generation Data")]
    public class BaseEnemyGenerationModifiers : ScriptableObject
    {
        public EnemyInstance PrefabToUse;

        public int MininumAmount;
        public int MaximumAmount;

        [Space(5)]
        public IntEnemyStat HitpointModifier;
        [Range(0, 1)]
        public float HitpointDeviation;

        [Space(5)]
        public IntEnemyStat ArmorModifier;
        [Range(0, 1)]
        public float ArmorDeviation;

        [Space(5)]
        public IntEnemyStat SpeedModifier;
        [Range(0, 1)]
        public float SpeedDeviation;

        [Space(5)]
        public float Interval;
        [Range(0, 1)]
        public float IntervalDeviation;

        [Space(5)]
        public List<IntLootStat> LootModifiers;

        [Space(5)]
        public List<IntLootStat> PunishmentModifiers;

        [Space(10)]
        public int TestDifficulty;
        [Range(0, 1)]
        public float TestRatio;
        public string TestRandomSeed;

        public WaveCluster GenerateCluster(int baseDifficulty, float ratio, int waveNumber, System.Random rand = null)
        {
            if (rand == null) rand = new System.Random();

            int actualAmount = (int) Math.Round(rand.Next(MininumAmount, MaximumAmount + 1) * ratio);
            int actualDifficulty = (int) (baseDifficulty * ratio / actualAmount);
            float initialHp = HitpointModifier.GenerateValue(actualDifficulty, rand);
            float initialArmor = ArmorModifier.GenerateValue(actualDifficulty, rand);
            float initialSpeed = SpeedModifier.GenerateValue(actualDifficulty, rand);
            var loot = LootModifiers.Select(l => new IntCurrency
            {
                Variable = l.Variable,
                Amount = l.DifficultyStat.GenerateValue(actualDifficulty, rand)
            }).ToList();
            var punishmet = PunishmentModifiers.Select(p => new IntCurrency
            {
                Variable = p.Variable,
                Amount = p.DifficultyStat.GenerateValue(actualDifficulty, rand)
            }).ToList();

            return new WaveCluster
            {
                DifficultyPerMonster = actualDifficulty,
                Difficulty = (int) (baseDifficulty * ratio),
                Amount = actualAmount,
                InitialCountDown = Interval,
                Interval = Interval,
                Prefab = PrefabToUse,
                IntervalDeviation = IntervalDeviation,
                SpawnWithPreviousCluster = false,
                WaveNumber = waveNumber,
                EnemyData = new EnemyWaveData
                {
                    InitialHitpoints = initialHp,
                    InitialArmor = initialArmor,
                    InitialSpeed = initialSpeed,
                    IntLoots = loot,
                    IntPunishments = punishmet,
                    HitpointsDeviation = HitpointDeviation,
                    ArmorDeviation = ArmorDeviation,
                    SpeedDeviation = SpeedDeviation
                },
                OverrideSpawnpoints = new List<Transform>()
            };
        }
    }
}