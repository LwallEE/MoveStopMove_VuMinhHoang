using System.Collections;
using System.Collections.Generic;
using ReuseSystem;
using ReuseSystem.AudioSystem;
using UnityEngine;

public class TestAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    [SerializeField] private string soundName;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            AudioManager.Instance.PlayClip(soundName);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioManager.Instance.PlayClip(clip,true);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            AudioManager.Instance.StopMusic(soundName);
        }
    }
}
