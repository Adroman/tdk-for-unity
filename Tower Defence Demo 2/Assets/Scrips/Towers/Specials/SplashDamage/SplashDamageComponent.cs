using System;
using System.Linq;
using Scrips.EnemyData.Instances;
using Scrips.Utils;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Towers.Specials.SplashDamage
{
    public struct EnemyWithDistance
    {
        public EnemyInstance Enemy;
        public float Distance;

        public EnemyWithDistance(EnemyInstance enemy, float distance)
        {
            Enemy = enemy;
            Distance = distance;
        }
    }

    public class SplashDamageComponent : SpecialComponent
    {
        public float SplashRadius;

        public bool HasUpperLimit;
        public float UpperLimit;

        public AnimationCurve DamageCurve;

        public EnemyCollection Enemies;

        [NonSerialized]
        public float Damage;

        public override SpecialType SpecialType { get; set; }

        public override void ApplySpecialEffect(EnemyInstance target)
        {
            var enemiesNearby = Enemies.Instances
                .Select(e => new EnemyWithDistance(e, e.transform.position.Distance2D(target.transform.position)))
                .Where(e => e.Distance <= SplashRadius);

            foreach (var enemyNearby in enemiesNearby)
            {
                float damage = Damage * EvaluateDamageMultiplier(enemyNearby.Distance);
                enemyNearby.Enemy.TakeDamage(damage);
            }
        }

        public override string GetUiText()
        {
            return $"Splash radius: {SplashRadius}";
        }

        public override void CopyDataToTargetComponent(SpecialComponent targetComponent)
        {
            var target = targetComponent as SplashDamageComponent;
            if (target == null)
            {
                Debug.LogError("Invalid component");
                return;
            }

            target.Damage = Damage;
            target.SplashRadius = SplashRadius;
            target.HasUpperLimit = HasUpperLimit;
            target.UpperLimit = UpperLimit;
            target.DamageCurve = DamageCurve;
            target.Enemies = Enemies;
        }

        public float EvaluateDamageMultiplier(float distance)
        {
            if (distance > SplashRadius) return 0;
            if (SplashRadius == 0) return distance == 0 ? 1 : 0;

            return DamageCurve.Evaluate(distance / SplashRadius);
        }
    }
}