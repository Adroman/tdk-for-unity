using UnityEngine;

namespace Scrips.CustomTypes.SpecialType.SplashDamage
{
    public class SplashDamage : Special
    {
        public float SplashRadius;

        public AnimationCurve DamageCurve;

        public float EvaluateDamageMultiplier(float distance)
        {
            if (distance > SplashRadius) return 0;
            if (SplashRadius == 0) return distance == 0 ? 1 : 0;

            return DamageCurve.Evaluate(distance / SplashRadius);
        }
    }
}