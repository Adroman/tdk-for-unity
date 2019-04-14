using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Scrips.Priorities
{
    [CreateAssetMenu(menuName = "Towers/Priorities/Nearest to goal")]
    public class NearestToGoalPriority : BasePriority
    {
        public override IOrderedEnumerable<Scrips.EnemyData.Instances.EnemyInstance> Prioritize(
            [NoEnumeration] IEnumerable<Scrips.EnemyData.Instances.EnemyInstance> enemies)
        {
            var orderedEnemies = enemies as IOrderedEnumerable<Scrips.EnemyData.Instances.EnemyInstance>;
            return orderedEnemies != null
                ? orderedEnemies.ThenBy(e => e.DistanceToGoal)
                : enemies.OrderBy(e => e.DistanceToGoal);
        }
    }
}

