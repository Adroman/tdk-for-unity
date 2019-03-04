using UnityEngine;
using UnityEngine.UI;

namespace Scrips.Particles
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSpawner : MonoBehaviour
    {
        public Slider Slider;
        
        private ParticleSystem _particle;
        
        private void Start()
        {
            _particle = GetComponent<ParticleSystem>();
        }

        public void StartParticle()
        {
            var shape = _particle.shape;
            shape.radius = Slider.value;
            var emission = _particle.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(Slider.value * Slider.value * 2500);
            _particle.Stop();
            _particle.Play();
        }
    }
}