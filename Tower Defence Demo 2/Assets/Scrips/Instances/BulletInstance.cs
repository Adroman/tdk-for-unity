using Scrips.SpecialEffects;
using UnityEngine;

namespace Scrips.Instances
{
    public class BulletInstance : MonoBehaviour
    {
        public float Speed;
        public float Damage;
        public Scrips.EnemyData.Instances.EnemyInstance Target;
        public GameObject SpecialEffect;

        // Update is called once per frame
        private void Update ()
        {
            if (Target == null)
            {
                PoolManager.Despawn(gameObject);
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

            PoolManager.Despawn(gameObject);
        }
    }
}
