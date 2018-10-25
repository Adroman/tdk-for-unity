using UnityEngine;

namespace Scrips.Audio
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class AudioAnimator : MonoBehaviour
    {
        private AudioSource _source;

        private Animator _animator;

        private void OnEnable()
        {
            _source = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
        }

        public void TurnOffAudio()
        {
            _animator.SetTrigger("FadeOut");
        }
    }
}