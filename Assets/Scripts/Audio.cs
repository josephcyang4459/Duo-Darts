using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio inst;
    [SerializeField] AudioSource Source;

    private void Awake()
    {
        if (inst != null)
        {
            DestroyImmediate(this);
            return;
        }

        DontDestroyOnLoad(this);
        inst = this;
        OptionsMenu.VolumeChange += ChangeVolume;
        ChangeVolume(PlayerPrefs.GetFloat("volume", .5f));
    }

    public void ChangeVolume(float value)
    {
        if (Mathf.Approximately(value, 0))
        {
            Source.mute = true;
            return;
        }

        if (Source.mute)
            Source.mute = false;
        Source.volume = value;
    }

    public void PlayClip(AudioClip clip)
    {
        Source.PlayOneShot(clip);
    }

    public void PlaySong(AudioClip clip)
    {
        if (Source.clip == clip)
            return;
        Source.clip = clip;
        Source.Play();
     
    }

    public void StopSong()
    {
        Source.Stop();
    }
}
