using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    public static soundManager instance;
    public AudioSource Audio;
    public int Playing;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null && instance != this)
            Destroy(gameObject);
    }

    public void playSound(AudioClip sound)
    {
        if (sound.name == "coins")
            StartCoroutine(count(sound));

        Audio.PlayOneShot(sound);
    }
    public void adjustVolume(float volume)
    {

        Audio.volume = volume;
        GameObject.Find("Player").GetComponent<Movement>().changeVolume(volume);
    }

    private IEnumerator count(AudioClip sound)
    {
        Playing++;
        yield return new WaitForSeconds(sound.length);
        Playing--;
    }
}
