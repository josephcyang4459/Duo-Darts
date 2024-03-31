using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio inst;
    [SerializeField] AudioSource Source;
    [SerializeField] AudioClip ClickSound;
    [SerializeField] AudioClip DartSound;


    private void Awake()
    {
        if (inst != null)
        {
            Destroy(gameObject);
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

    public void PlayClip(AudioClips clip)
    {
        return;
        switch (clip)
        {
            case AudioClips.Click: PlayClip(ClickSound);return;
            case AudioClips.Dart: PlayClip(DartSound);return;
        }
    }

    public void PlayClip(AudioClip clip)
    {
        return;
        Source.PlayOneShot(clip);
    }

    public void PlaySong(AudioClip clip)
    {
        return;
        if (Source.clip == clip)
            if (Source.isPlaying)
                return;

        Source.clip = clip;
        Source.Play();
     
    }

    public void StopSong()
    {
        Source.Stop();
    }
}
