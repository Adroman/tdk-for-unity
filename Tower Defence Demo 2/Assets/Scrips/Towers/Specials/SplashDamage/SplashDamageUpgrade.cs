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

            splashComponent.SplashRadius = Math.Min(
                RadiusIncrease.Increase(splashComponent.SplashRadius, ValueToUse),
                splashComponent.UpperLimit);
        }
    }
}