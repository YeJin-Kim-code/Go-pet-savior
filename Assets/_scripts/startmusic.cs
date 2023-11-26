using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startmusic : MonoBehaviour
{
    public AudioClip startMusic;

    private void Start()
    {
        GameObject soundObject = new GameObject("BackGroundSound");
        AudioSource backAudio = soundObject.AddComponent<AudioSource>();
        backAudio.clip = startMusic;
        backAudio.loop = true;
        backAudio.volume = 0.6f;
        backAudio.Play();
    }
}
