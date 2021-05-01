using Scrips.Towers.Specials;

namespace Scrips.Modifiers.Towers.TowerSpecials
{
    public abstract class BaseTowerSpecialModifier<TSpecialComponent, TSpecialInfo> : BaseModifier
        where TSpecialComponent : SpecialComponent
    {
        public abstract void AddToTowerSpecial(TSpecialComponent targetComponent);

        public abstract void RemoveFromTowerSpecial(TSpecialComponent targetComponent);
    }
}