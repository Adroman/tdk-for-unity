using System;
using System.Collections.Generic;
using System.Linq;
using Scrips.Instances;
using Scrips.Towers.Bullets;
using UnityEngine;

namespace Scrips.SpecialEffects
{
    public class SplashDamage : BaseSpecialEffect
    {
        public float SplashRadius;
        [Range(0,100)]
        public float SplashStrength;

        private float _baseDamage;

        public override void InitSpecialEffect(GameObject bulletObject)
        {
            var bullet = bulletObject.GetComponent<BulletInstance>();
            if (bullet == null) throw new NotSupportedException();
            _baseDamage = bullet.Damage;
        }

        public override void ApplySpecialEffect(Scrips.EnemyData.Instances.EnemyInstance target)
        {
            var enemies = GameObject.FindObjectsOfType<Scrips.EnemyData.Instances.EnemyInstance>();

            var targets = enemies
                .Select(e => new KeyValuePair<Scrips.EnemyData.Instances.EnemyInstance, float>(e, (e.transform.position - target.transform.position).magnitude))
                .Where(e => e.Value <= SplashRadius && e.Key != target)
                .Select(e => e.Key).ToList();

            foreach(var subTarget in targets)
            {
                subTarget.TakeDamage(_baseDamage * SplashStrength / 100);
            }
        }
    }
}
