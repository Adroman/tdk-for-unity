using UnityEngine;

namespace Scrips.Towers.Specials.PoisonDamage
{
    [CreateAssetMenu(menuName = "Tower defense kit/Tower Specials/Poison")]
    public class PoisonType : SpecialType
    {
        public float Dps;

        public float Duration;

        public override SpecialComponent GetOrCreateSpecialComponent(GameObject go)
        {
            var component = go.GetComponent<PoisonComponent>();
            if (component == null) component = go.AddComponent<PoisonComponent>();
            component.SpecialType = this;
            component.Dps = Dps;
            component.Duration = Duration;
            return component;
        }

        public override SpecialComponent GetSpecialComponent(GameObject go)
        {
            return go.GetComponent<PoisonComponent>();
        }

        public override string GetUiText() => $"Deals {Dps} damage per second over {Duration} seconds";
    }
}