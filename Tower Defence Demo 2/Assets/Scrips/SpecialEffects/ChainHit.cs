using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Instances;
using UnityEngine;

namespace Scrips.SpecialEffects
{
    public class ChainHit : BaseSpecialEffect
    {
        public float Length;
        public float Range;

        private float _damage;
        private List<Scrips.EnemyData.Instances.EnemyInstance> _ignoredEnemies;
        private GameObject _instance;

        private static GameObject _bulletsParent;

        private static GameObject BulletsParent
        {
            get
            {
                if (_bulletsParent == null)
                    _bulletsParent = GameObject.Find("Bullets");
                return _bulletsParent;
            }
        }

        public override void InitSpecialEffect(GameObject bulletObject)
        {
            var bullet = bulletObject.GetComponent<BulletInstance>();
            if (bullet == null) throw new NotSupportedException();
            _instance = bulletObject;
            if (_ignoredEnemies == null) _ignoredEnemies = new List<Scrips.EnemyData.Instances.EnemyInstance>();
            _damage = bullet.Damage;
        }

        public override void ApplySpecialEffect(Scrips.EnemyData.Instances.EnemyInstance target)
        {
            if (Length < 1f)
            {
                target.TakeDamage(_damage * Length);
                return;
            }

            var enemies = GameObject.FindObjectsOfType<Scrips.EnemyData.Instances.EnemyInstance>();

            var nextTarget = enemies
                .Select(e => new KeyValuePair<Scrips.EnemyData.Instances.EnemyInstance, float>(e, (e.transform.position - target.transform.position).magnitude))
                .Where(e => e.Value <= Range && e.Key != target)
                .Where(e => !_ignoredEnemies.Contains(e.Key))
                .OrderBy(e => e.Value)
                .Select(e => e.Key)
                .FirstOrDefault();

            if (nextTarget == null) return;

            //_instance.
        }

    }
}
