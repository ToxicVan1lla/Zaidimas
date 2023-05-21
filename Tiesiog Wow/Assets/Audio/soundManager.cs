using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    public static soundManager instance;
    private AudioSource Audio;

    private void Awake()
    {
        instance = this;
        Audio = GetComponent<AudioSource>();
    }
    public void playSound(AudioClip sound)
    {
        Audio.PlayOneShot(sound);
    }
}
