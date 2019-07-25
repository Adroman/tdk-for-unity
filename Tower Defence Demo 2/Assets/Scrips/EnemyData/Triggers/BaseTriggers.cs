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

        private bool _hasOnDeathTrigger;
        private bool _hasOnSpawnTrigger;
        private bool _hasOnFinishTrigger;

        public bool HasOnDeathTrigger => _hasOnDeathTrigger;
        public bool HasOnSpawnTrigger => _hasOnSpawnTrigger;
        public bool HasOnFinishTrigger => _hasOnFinishTrigger;

        private void Start()
        {
            _hasOnDeathTrigger = OnDeath != null;
            _hasOnSpawnTrigger = OnSpawn != null;
            _hasOnFinishTrigger = OnFinish != null;
        }
    }
}