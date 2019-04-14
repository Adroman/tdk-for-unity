using System.Collections.Generic;
using Scrips.Attributes;
using Scrips.EnemyData.Instances;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Statistics
{
    public class EnemyStatistics : MonoBehaviour
    {
        public IntVariable EnemiesTotal;

        public readonly Dictionary<EnemyAttribute, int> EnemiesByAttribute = new Dictionary<EnemyAttribute, int>();

        public void UpdateStatistics(EnemyInstance enemy)
        {
            if (EnemiesTotal != null) EnemiesTotal.Value++;

            foreach (var enemyAttribute in enemy.Attributes)
            {
                int amount;
                EnemiesByAttribute.TryGetValue(enemyAttribute, out amount);
                amount++;
                EnemiesByAttribute[enemyAttribute] = amount;
            }
        }
    }
}