using UnityEngine.Audio;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        // Creates an AudioSource component from the sounds array in Unity and adds it to the CombatAudio object
        foreach (Sound s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
        }
    }

    public void Play(string SoundName)
    {
        // Finds a specific sound related to the name and plays it
        Sound s = Array.Find(sounds, Sound => Sound.Name == SoundName);

        // If the sound couldn't be found, then return
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }

        s.Source.Play();
    }
}
