using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event Start", menuName = "Cutscene/Event Start")]
public class EventStart : ScriptableObject
{

    public int hour;
    public int minutes;
    public int endHour;
    public int endMinutes;
    public Locations Location;
    public CutScene cutScene;

    public bool done = false;  
}

