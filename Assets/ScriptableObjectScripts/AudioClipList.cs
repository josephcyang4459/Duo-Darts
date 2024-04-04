using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Audio Clip List", menuName ="Reference/List/Audio Clip")]
public class AudioClipList : ScriptableObject
{
    public AudioClip[] List;
}
