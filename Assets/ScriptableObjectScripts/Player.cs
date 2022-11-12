using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class Player : ScriptableObject {
    public string Name;
    public float Charisma = 1f;
    public float Intoxication = 1f;
    public float Composure = 1f;
    public float Skill = 1f;
    public float Luck = 1f;
}
