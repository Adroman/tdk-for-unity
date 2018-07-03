namespace Scrips.CustomTypes.SpecialType.SplashDamage
{
    public class SpashDamageType : SpecialType
    {
        public override Special CreateSpecial() => new SplashDamage();
    }
}