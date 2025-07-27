using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio inst;
    [SerializeField] AudioSource MusicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource ReverbSource;
    [SerializeField] AudioReverbZone ReverbZone;
    [SerializeField] AudioClip ClickSound;
    [SerializeField] AudioClipList DartClips;
    [SerializeField] AudioClipList SoftDarts;
    [SerializeField] AudioClipList MediumDarts;
    [SerializeField] AudioClipList HardDarts;
    [SerializeField] AudioClipList Music;

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
        OptionsMenu.SFXVolumeChange += ChangeSFXVolume;
        ChangeVolume(PlayerPrefs.GetFloat("volume", .5f));
        ChangeVolume(PlayerPrefs.GetFloat("sfx_volume", .5f));
    }

    private void OnDestroy() {
        OptionsMenu.VolumeChange -= ChangeVolume;
        OptionsMenu.SFXVolumeChange -= ChangeSFXVolume;
    }

    public void ChangeVolume(float value) {
        if (Mathf.Approximately(value, 0)) {
            MusicSource.mute = true;
            return;
        }

        if (MusicSource.mute)
            MusicSource.mute = false;
        MusicSource.volume = value;
    }

    public void ChangeSFXVolume(float value) {
        if (Mathf.Approximately(value, 0)) {
            SFXSource.mute = true;
            ReverbSource.mute = true;
            return;
        }

        if (SFXSource.mute)
            SFXSource.mute = false;
        SFXSource.volume = value;
        if (ReverbSource.mute)
            ReverbSource.mute = false;
        ReverbSource.volume = value;
    }

    public void PlayClip(AudioClips clip)
    {
        switch (clip)
        {
            case AudioClips.Click: PlayClip(ClickSound);return;
            case AudioClips.RandomDart: PlayClip(DartClips.List[Random.Range(0,DartClips.List.Length)]);return;
        }
    }

    public void PlayDartClipReverb(DartAudioClips clip, AudioReverbPreset preset) {
        AudioClipList GetList(DartAudioClips clip) {
            switch (clip) {
                case DartAudioClips.Medium: return SoftDarts;
                case DartAudioClips.Hard: return SoftDarts;
            }
            return SoftDarts;
        }
        ReverbZone.reverbPreset = preset;
        ReverbSource.PlayOneShot(GetList(clip).List[Random.Range(0, GetList(clip).List.Length)]);
    }

    public void PlayClip(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlaySong(MusicTrack trackID) {
        if (Music.List[(int)trackID] == null)
            return;
        if (MusicSource.clip == Music.List[(int)trackID])
            if (MusicSource.isPlaying)
                return;

        MusicSource.clip = Music.List[(int)trackID];
        MusicSource.Play();

    }

    public void StopSong()
    {
        MusicSource.Stop();
    }
}


public enum MusicTrack {
    MainMenu,
    LocationSelect,
    Cutscene,
    Darts,
    Credits
}