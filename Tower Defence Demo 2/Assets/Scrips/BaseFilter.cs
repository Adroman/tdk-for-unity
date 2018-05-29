using System.Collections.Generic;
using UnityEngine;

namespace Scrips
{
    public class BaseFilter : MonoBehaviour
    {
        public virtual IEnumerable<Scrips.EnemyData.Instances.EnemyInstance> FilterEnemies(IEnumerable<Scrips.EnemyData.Instances.EnemyInstance> enemies)
        {
            return enemies;
        }
    }
}
