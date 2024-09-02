using System;
using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using ReuseSystem.ObjectPooling;
using UnityEngine;

namespace ReuseSystem.AudioSystem
{
   public class SoundPlayer : MonoBehaviour
   {
      private AudioSource source;

      private void Awake()
      {
         source = GetComponent<AudioSource>();
      }

      public void InitData(AudioClip clip, bool isLoop, float volume)
      {
         source.loop = isLoop;
         source.clip = clip;
         source.volume = volume;

         source.Play();
      }

      public bool IsPlayingClip(AudioClip clip)
      {
         return source.clip == clip && gameObject.activeSelf;
      }

      public void StopMusic()
      {
         source.Stop();
         LazyPool.Instance.AddObjectToPool(gameObject);
      }

      private void FixedUpdate()
      {
         //automatic add to pool if it is play one shot, after it finish play
         if (!source.loop)
         {
            //check if it has finished play
            if (!source.isPlaying)
            {
               LazyPool.Instance.AddObjectToPool(gameObject);
            }
         }
      }
   }
}
