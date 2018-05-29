using UnityEngine;

namespace Scrips.Events.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioLoop : MonoBehaviour
    {
        public LoopAudioEvent Loop;

        private AudioSource _source;
        private Coroutine _activeCoroutine;

        public void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            Play();
        }

        public void Play()
        {
            if (Loop == null) return;
            _activeCoroutine = StartCoroutine(Loop.PlayAudio(_source));
        }

        public void Stop()
        {
            if (_activeCoroutine == null) return;
            StopCoroutine(_activeCoroutine);
        }
    }
}