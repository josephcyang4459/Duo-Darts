using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

[CreateAssetMenu]
public class Partner : ScriptableObject
{
    public string Name;
    public TMP_FontAsset Font;
    public float Charisma = 1f;
    public float Intoxication = 1f;
    public float Love = 1f;
    public Sprite[] Expressions;
}
