using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio inst;
    [SerializeField] AudioSource MainSource;
    [SerializeField] AudioSource ReverbSource;
    [SerializeField] AudioReverbZone ReverbZone;
    [SerializeField] AudioClip ClickSound;
    [SerializeField] AudioClipList DartClips;

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

    public void ChangeVolume(float value) {
        if (Mathf.Approximately(value, 0)) {
            MainSource.mute = true;
            ReverbSource.mute = true;
            return;
        }

        if (MainSource.mute)
            MainSource.mute = false;
        MainSource.volume = value;
        if (ReverbSource.mute)
            ReverbSource.mute = false;
        ReverbSource.volume = value;
    }

    public void PlayClip(AudioClips clip)
    {
        switch (clip)
        {
            //case AudioClips.Click: PlayClip(ClickSound);return;
            case AudioClips.RandomDart: PlayClip(DartClips.List[Random.Range(0,DartClips.List.Length)]);return;
        }
    }

    public void PlayDartClipReverb(DartAudioClips clip, AudioReverbPreset preset) {
        ReverbZone.reverbPreset = preset;
        ReverbSource.PlayOneShot(DartClips.List[(int)clip]);
    }

    public void PlayClip(AudioClip clip)
    {
        MainSource.PlayOneShot(clip);
    }

    public void PlaySong(AudioClip clip)
    {
        return;
        if (MainSource.clip == clip)
            if (MainSource.isPlaying)
                return;

        MainSource.clip = clip;
        MainSource.Play();
     
    }

    public void StopSong()
    {
        MainSource.Stop();
    }
}
