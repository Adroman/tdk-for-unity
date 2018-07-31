using System;
using Scrips.CustomTypes.IncreaseType;
using UnityEngine;

namespace Scrips.Towers.Specials.SplashDamage
{
    [CreateAssetMenu(menuName = "Tower Upgrades/Splash Damage")]
    public class SplashDamageUpgrade : SpecialUpgrade
    {
        public BaseIncreaseType RadiusIncrease;
        public float ValueToUse;

        public override void Upgrade(SpecialComponent component)
        {
            var splashComponent = component as SplashDamageComponent;

            if (splashComponent == null)
            {
                Debug.LogWarning("Invalid component");
                return;
            }

            float newRadius = RadiusIncrease.Increase(splashComponent.SplashRadius, ValueToUse);

            if (splashComponent.HasUpperLimit)
            {
                splashComponent.SplashRadius = Math.Min(
                    newRadius,
                    splashComponent.UpperLimit);
            }
            else
            {
                splashComponent.SplashRadius = newRadius;
            }
        }
    }
}