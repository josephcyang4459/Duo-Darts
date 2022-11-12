using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class Partner : ScriptableObject
{
    public string Name;
    public float Charisma = 1f;
    public float Intoxication = 1f;
    public float Love = 1f;
}
