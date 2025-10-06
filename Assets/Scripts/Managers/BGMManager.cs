using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip bgm;
    private AudioSource source;
    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = bgm;
        source.loop = true;
        source.playOnAwake = false;
        source.volume = 0.1f;
        source.Play();
    }
}
