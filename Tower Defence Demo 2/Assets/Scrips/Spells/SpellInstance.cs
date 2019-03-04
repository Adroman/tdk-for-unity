using System;
using System.Collections;
using System.Linq;
using Scrips.Utils;
using Scrips.Variables;
using UnityEngine;

namespace Scrips.Spells
{
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class SpellInstance : MonoBehaviour
    {
        protected CircleCollider2D Collider;

        protected ParticleSystem Particles;

        public EnemySpell Spell;

        public EnemyCollection Enemies;

        private void OnEnable()
        {
            Collider = GetComponent<CircleCollider2D>();
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
            foreach (var enemy in Enemies.Instances.Where(e => e.transform.position.Distance2D(transform.position) <= Spell.Range))
            {
                Spell.ApplySpell(enemy);
            }
        }
    }
}