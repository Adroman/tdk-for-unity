namespace Scrips.Spells
{
    public class DrainSpellInstance : SpellInstance
    {
        protected override void ChangeParticles()
        {
            var mainParticle = Particles.main;
            mainParticle.startSize = Spell.Range * 2f;
        }
    }
}