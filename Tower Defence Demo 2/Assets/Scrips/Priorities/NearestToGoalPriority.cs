using System.Collections.Generic;
using System.Linq;

namespace Scrips.Priorities
{
    public class NearestToGoalPriority : BasePriority
    {
        public override IOrderedEnumerable<Scrips.EnemyData.Instances.EnemyInstance> Prioritize(IEnumerable<Scrips.EnemyData.Instances.EnemyInstance> enemies)
        {
            var orderedEnemies = enemies as IOrderedEnumerable<Scrips.EnemyData.Instances.EnemyInstance>;
            if (orderedEnemies != null)
                orderedEnemies = orderedEnemies.ThenBy(e => e.DistanceToGoal);
            else
                orderedEnemies = enemies.OrderBy(e => e.DistanceToGoal);

            return orderedEnemies.ThenBy(e => e.DistanceToGoal);
        }
    }
}

