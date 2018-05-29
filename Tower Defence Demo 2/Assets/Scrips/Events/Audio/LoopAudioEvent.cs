using System.Collections;
using System.Collections.Generic;
using Scrips.CustomTypes;
using Scrips.Utils;
using UnityEngine;

namespace Scrips.Events.Audio
{
    [CreateAssetMenu(menuName = "Audio Events/Loop", order = 10)]
    public class LoopAudioEvent : ScriptableObject
    {
        public LogLevel LogLevel;

        public List<AudioClip> AudioClips = new List<AudioClip>();

        public IEnumerator PlayAudio(AudioSource source)
        {
            while (true)
            {
                int i = Random.Range(0, AudioClips.Count);

                DebugUtils.LogDebug(LogLevel, "Playing clip: " + i);

                source.clip = AudioClips[Random.Range(0, AudioClips.Count)];
                source.Play();

                yield return new WaitWhile(() => source.isPlaying);
            }
        }
    }
}