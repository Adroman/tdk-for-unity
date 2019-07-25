using System;
using System.Collections.Generic;
using Scrips.Data;
using Scrips.EnemyData.Instances;
using Scrips.Modifiers;
using Scrips.Modifiers.Currency;
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

        public virtual void SetEnemy(EnemyInstance enemy, ModifierController modifierController, System.Random random)
        {
            // import modifiers

            enemy.InitialHitpoints.Value = Utils.Utils.GetDeviatedValue(InitialHitpoints, HitpointsDeviation, random);
            enemy.InitialArmor.Value = Utils.Utils.GetDeviatedValue(InitialArmor, ArmorDeviation, random);
            enemy.InitialSpeed.Value = Utils.Utils.GetDeviatedValue(InitialSpeed, SpeedDeviation, random);

            enemy.IntLoot = new List<ModifiedCurrency>();
            enemy.IntPunishments = new List<ModifiedCurrency>();

            foreach (var intLoot in IntLoots)
            {
                var modLoot = new ModifiedCurrency();
                modLoot.Currency = intLoot;
                // import modifiers
                modLoot.Amount.Value = intLoot.Amount;
                enemy.IntLoot.Add(modLoot);
            }

            foreach (var intPunishment in IntPunishments)
            {
                var modPunishment = new ModifiedCurrency();
                modPunishment.Currency = intPunishment;
                // import modifiers
                modPunishment.Amount.Value = intPunishment.Amount;
                enemy.IntPunishments.Add(modPunishment);
            }

            modifierController.ImportModifiers(enemy);
        }
    }
}