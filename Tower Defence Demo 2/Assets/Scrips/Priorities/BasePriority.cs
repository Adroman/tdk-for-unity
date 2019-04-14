using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Scrips.Priorities
{
    public abstract class BasePriority : ScriptableObject
    {
        public abstract IOrderedEnumerable<Scrips.EnemyData.Instances.EnemyInstance> Prioritize(
            [NoEnumeration] IEnumerable<Scrips.EnemyData.Instances.EnemyInstance> enemies);
    }
}

