using System.Collections;
using System.Collections.Generic;
using ReuseSystem.ObjectPooling;
using UnityEngine;

namespace ReuseSystem.AudioSystem
{ 
    public class AudioManager : Singleton<AudioManager>
    {
        private List<SoundPlayer> currentLoopSound = new List<SoundPlayer>();
        [SerializeField] private GameObject soundPrefab;
        [SerializeField] private List<AudioInfo> infos;
        
        //play clip according to audio Info name
        public void PlayClip(string audioInfoName)
        {
            foreach (var sound in infos)
            {
                if (sound.name == audioInfoName)
                {
                    PlayClip(sound.TakeRandom(), sound.isLoop, sound.volume);
                    break;
                }
            }
        }
        public void PlayClip(AudioClip clip, bool isLoop = false, float volume = 1f)
        {
            var sound = LazyPool.Instance.GetObj<SoundPlayer>(soundPrefab);
            sound.InitData(clip,isLoop,volume);
            if (isLoop)
            {
                currentLoopSound.Add(sound);
            }
        }

        public void StopMusic(AudioClip clip)
        {
            foreach (var sound in currentLoopSound)
            {
                if (sound.IsPlayingClip(clip))
                {
                    sound.StopMusic();
                    currentLoopSound.Remove(sound);
                    return;
                }
            }
        }

        public void StopMusic(string audioInfoName)
        {
            foreach (var sound in infos)
            {
                if (sound.name == audioInfoName)
                {
                    foreach (var item in sound.soundVariation)
                    {
                        StopMusic(item);
                    }
                    break;
                }
            }
        }
        
    }
}
