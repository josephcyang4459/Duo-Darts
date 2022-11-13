using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventStart : MonoBehaviour
{

    public timing[] available;


    public byte LocationOf(int hour, int Minuts)
    {
        for( int i = 0; i < available.Length; i++)
        {
            if (check(available[i], hour, Minuts))
                return available[i].location;
        }

        return 255;
    }


public void go()
    {
        Debug.Log("Here");
    }

    private bool check(timing t, int b, int b1)
    {
        if (b >= t.hour && b <= t.endHour)
            if (b1 >= t.minutes && b1 <= t.endMinutes)
                return true;

        return false;
    }
}


[System.Serializable]
public class timing
{
    public byte hour;
    public byte minutes;

    public byte endHour;
    public byte endMinutes;
    public byte location;
}