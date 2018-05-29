using System;
using Scrips.Events.Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace Scrips.EnemyData.Triggers
{
    public class BaseTriggers : MonoBehaviour
    {
        public EnemyEvent OnDeath;
        public EnemyEvent OnSpawn;
        public EnemyEvent OnFinish;
    }
}