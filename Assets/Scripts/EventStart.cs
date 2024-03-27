using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EventStart : ScriptableObject
{

    public byte hour;
    public byte minutes;

    public byte endHour;
    public byte endMinutes;
    public Locations Location;
    public CutScene cutScene;

    public bool done = false;  
}

