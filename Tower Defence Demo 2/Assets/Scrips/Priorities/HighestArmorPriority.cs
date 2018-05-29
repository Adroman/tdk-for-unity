using System.Collections.Generic;
using System.Linq;

namespace Scrips.Priorities
{
    public class HighestArmorPriority : BasePriority
    {
        public override IOrderedEnumerable<Scrips.EnemyData.Instances.EnemyInstance> Prioritize(IEnumerable<Scrips.EnemyData.Instances.EnemyInstance> enemies)
        {
            var orderedEnemies = enemies as IOrderedEnumerable<Scrips.EnemyData.Instances.EnemyInstance>;
            if (orderedEnemies != null) return orderedEnemies.ThenByDescending(e => e.Armor);
            return enemies.OrderByDescending(e => e.Armor);
        }
    }
}

