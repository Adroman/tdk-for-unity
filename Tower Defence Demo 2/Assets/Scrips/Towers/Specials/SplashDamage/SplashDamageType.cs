using UnityEngine;

namespace Scrips.Towers.Specials.SplashDamage
{
    [CreateAssetMenu(menuName = "Tower Specials/Splash damage")]
    public class SplashDamageType : SpecialType
    {
        public float SplashRadius;

        public float UpperLimit;

        public AnimationCurve DamageCurve;

        public override SpecialComponent GetOrCreateSpecialComponent(GameObject go)
        {
            var component = go.GetComponent<SplashDamageComponent>();
            if (component == null) component = go.AddComponent<SplashDamageComponent>();
            component.SpecialType = this;
            component.DamageCurve = DamageCurve;
            component.SplashRadius = SplashRadius;
            component.UpperLimit = UpperLimit;
            return component;
        }

        public override SpecialComponent GetSpecialComponent(GameObject go)
        {
            return go.GetComponent<SplashDamageComponent>();
        }
    }
}