using JetBrains.Annotations;
using UnityEngine;

namespace Scrips.Events.Audio
{
    [PublicAPI]
    [RequireComponent(typeof(AudioSource))]
    public class AudioEventListener : MonoBehaviour
    {
        public AudioEvent AudioEvent;

        private AudioSource _source;

        public void Start()
        {
            _source = GetComponent<AudioSource>();
        }

        public void Invoke()
        {
            AudioEvent.Play(_source);
        }
    }
}