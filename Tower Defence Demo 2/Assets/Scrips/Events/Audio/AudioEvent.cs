﻿using UnityEngine;

namespace Scrips.Events.Audio
{
    public abstract class AudioEvent : ScriptableObject
    {
        public abstract void Play(AudioSource source);
    }
}