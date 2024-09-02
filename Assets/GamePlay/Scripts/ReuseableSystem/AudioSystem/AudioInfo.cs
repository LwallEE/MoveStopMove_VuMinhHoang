using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ReuseSystem.AudioSystem
{
    [Serializable]
    public struct AudioInfo
    {
        public List<AudioClip> soundVariation;
        public bool isLoop;
        public float volume;

        public string name;

        public AudioClip TakeRandom()
        {
            return soundVariation[Random.Range(0, soundVariation.Count)];
        }
    }
}
