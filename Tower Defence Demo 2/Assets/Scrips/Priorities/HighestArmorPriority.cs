using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Scrips.Priorities
{
    [CreateAssetMenu(menuName = "Towers/Priorities/Highest armor")]
    public class HighestArmorPriority : BasePriority
    {
        public override IOrderedEnumerable<Scrips.EnemyData.Instances.EnemyInstance> Prioritize(
            [NoEnumeration] IEnumerable<Scrips.EnemyData.Instances.EnemyInstance> enemies)
        {
            var orderedEnemies = enemies as IOrderedEnumerable<Scrips.EnemyData.Instances.EnemyInstance>;
            return orderedEnemies != null
                ? orderedEnemies.ThenByDescending(e => e.Armor)
                : enemies.OrderByDescending(e => e.Armor);
        }
    }
}

