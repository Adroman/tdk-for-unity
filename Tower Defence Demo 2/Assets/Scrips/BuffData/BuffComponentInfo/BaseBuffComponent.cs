using Scrips.EnemyData.Instances;
using UnityEngine;

namespace Scrips.BuffData.BuffComponentInfo
{
    public abstract class BaseBuffComponent : MonoBehaviour
    {
        public bool InfiniteDuration;

        public float Duration;

        public abstract BaseBuffData CreateBuff(EnemyInstance target);
    }
}