using System;
using Scrips.Pooling;
using Scrips.SpecialEffects;
using Scrips.Towers.Specials;
using UnityEngine;

namespace Scrips.Towers.Bullets
{
    public class BulletInstance : MonoBehaviour
    {
        public float Speed;
        public float Damage;
        public Scrips.EnemyData.Instances.EnemyInstance Target;
        public GameObject SpecialEffect;
        public BulletManager BulletManager;

        [HideInInspector]
        public PoolComponent PoolComponent;

        public bool HasPoolComponent { get; private set; }

        private void Start()
        {
            PoolComponent = GetComponent<PoolComponent>();
            HasPoolComponent = PoolComponent != null;
        }

        // Update is called once per frame
        private void Update ()
        {
            if (Target == null)
            {
                BulletManager.DespawnBullet(gameObject);
                return;
            }
            // direction
            var dir = Target.transform.position - transform.position;

            // distance to target
            float distanceLeft = dir.magnitude;
            float distanceToTravel = Speed * Time.deltaTime;

            if (distanceLeft < distanceToTravel)
            {
                TargetReached();
            }

            // movement
            transform.Translate(dir.normalized * distanceToTravel, Space.World);
        }

        private void TargetReached()
        {
            Target.TakeDamage(Damage);
            if(SpecialEffect != null)
            {
                var sp = SpecialEffect.GetComponent<BaseSpecialEffect>();
                //SpecialEffect.GetComponent<BaseSpecialEffect>().InitSpecialEffect();
                SpecialEffect.GetComponent<BaseSpecialEffect>().ApplySpecialEffect(Target);
            }

            foreach (var specialComponent in GetComponents<SpecialComponent>())
            {
                specialComponent.ApplySpecialEffect(Target);
            }

            BulletManager.DespawnBullet(gameObject);
        }
    }
}
