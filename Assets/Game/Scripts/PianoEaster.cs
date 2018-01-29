using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoEaster : MonoBehaviour
{
    public AudioClip Song;

    AudioSource audio;
    Light light;

    // Use this for initialization
    void Start()
    {
        light = GetComponentInChildren<Light>();
        audio = GetComponent<AudioSource>();
    }

    public void SmashEgg()
    {
        audio.clip = Song;
        audio.Play();
        light.color = new Color(1, 0.3f, 0.1f, 1);
        light.intensity = 60;
    }
}
