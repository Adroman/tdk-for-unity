using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scrips.Priorities
{
    public class BasePriority : MonoBehaviour
    {
        public virtual IOrderedEnumerable<Scrips.EnemyData.Instances.EnemyInstance> Prioritize(IEnumerable<Scrips.EnemyData.Instances.EnemyInstance> enemies)
        {
            throw new NotImplementedException("You should not use basePriority");
        }
    }
}

