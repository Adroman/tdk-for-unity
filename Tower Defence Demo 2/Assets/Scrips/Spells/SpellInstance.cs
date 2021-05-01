using System;
using System.Collections;
using System.Linq;
using Scrips.Modifiers.Stats;
using Scrips.Utils;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Spells
{
    public abstract class SpellInstance : MonoBehaviour
    {
        protected ParticleSystem Particles;

        public EnemySpell Spell;

        public EnemyCollection Enemies;

        protected FloatModifiableStat Range;
        
        private void OnEnable()
        {
            Range = new FloatModifiableStat{Value = Spell.Range};
            Particles = GetComponent<ParticleSystem>();
            AdjustParticles();
            HitEnemies();
            StartCoroutine(DestroyItself());
        }

        private IEnumerator DestroyItself()
        {
            var startLifeTime = Particles.main.startLifetime;

            yield return new WaitForSeconds(Particles.main.duration +
                                            Math.Max(
                                                startLifeTime.constant,
                                                startLifeTime.constantMax));
            Destroy(gameObject);
        }

        private void AdjustParticles()
        {
            if (Particles != null)
            {
                ChangeParticles();
            }
        }

        protected abstract void ChangeParticles();

        private void HitEnemies()
        {
            foreach (var enemy in Enemies.Instances.Where(e => e.transform.position.Distance2D(transform.position) <= Range.Value))
            {
                Spell.ApplySpell(enemy);
            }
        }
    }
}