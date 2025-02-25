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
        MainSource.PlayOneShot(clip);
    }

    public void PlaySong(MusicTrack trackID)
    {
        if (MainSource.clip == Music.List[(int)trackID])
            if (MainSource.isPlaying)
                return;

        MainSource.clip = Music.List[(int)trackID];
        if(MainSource.clip!=null)
        MainSource.Play();
     
    }

    public void StopSong()
    {
        MainSource.Stop();
    }
}


public enum MusicTrack {
    MainMenu,
    LocationSelect,
    Cutscene,
    Darts
    
}