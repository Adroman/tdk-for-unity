using System;
using System.Collections.Generic;
using Scrips.Data;
using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.Waves
{
    [Serializable]
    public class EnemyWaveData
    {
        public float InitialHitpoints;
        [Range(0, 1)]
        public float HitpointsDeviation;

        [Space(5)]
        public float InitialArmor;
        [Range(0, 1)]
        public float ArmorDeviation;

        [Space(5)]
        public float InitialSpeed;
        [Range(0, 1)]
        public float SpeedDeviation;

        public List<IntCurrency> IntLoots;

        public List<IntCurrency> IntPunishments;

        public virtual void SetEnemy(EnemyInstance enemy, System.Random random)
        {
            enemy.InitialHitpoints = Utils.Utils.GetDeviatedValue(InitialHitpoints, HitpointsDeviation, random);
            enemy.InitialArmor = Utils.Utils.GetDeviatedValue(InitialArmor, ArmorDeviation, random);
            enemy.InitialSpeed = Utils.Utils.GetDeviatedValue(InitialSpeed, SpeedDeviation, random);

            enemy.IntLoot = new List<IntCurrency>(IntLoots);
            enemy.IntPunishments = new List<IntCurrency>(IntPunishments);
        }
    }
}