using System.Collections.Generic;
using UnityEngine;

namespace Scrips.Events.Audio
{
    [CreateAssetMenu(menuName = "Audio Events/Simple", order = 0)]
    public class SimpleAudioEvent : AudioEvent
    {
        [HideInInspector] public List<AudioClip> AudioClips = new List<AudioClip>();

        [HideInInspector] public float MinVolume;
        [HideInInspector] public float MaxVolume;
        [HideInInspector] public float MinPitch;
        [HideInInspector] public float MaxPitch;

        public override void Play(AudioSource source)
        {
            if (AudioClips.Count == 0)
            {
                Debug.LogWarning("Audio Event has no audio clips");
                return;
            }

            source.clip = AudioClips[Random.Range(0, AudioClips.Count)];
            source.volume = Random.Range(MinVolume, MaxVolume);
            source.pitch = Random.Range(MinPitch, MaxPitch);
            source.Play();
        }
    }
}