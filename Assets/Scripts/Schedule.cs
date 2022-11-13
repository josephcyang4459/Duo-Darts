using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Schedule : MonoBehaviour
{
    public int hour = 4;
    public int minutes;

    public Available[] available;

    public Location[] locals;

    public byte location;

    public void Start()
    {
        setTime();
    }

    public void setTime()
    {
        for(int i = 0; i < locals.Length; i++)
        {
            locals[i].useing = 0;
            locals[i].s.Clear();
        }

        for (int i = 0; i < available.Length; i++)
        {
            byte b = available[i].g.LocationOf(hour, minutes);



            if (b < 255)
            {
                locals[b].s.Add(available[i].g);
                locals[b].btnText[locals[b].useing].text = available[i].message;
                locals[b].useing++;
            }
        }

        for (int i = 0; i < locals.Length; i++)
        {
            for( int j = locals[i].useing; j < locals[i].bs.Length; j++)
            {
                locals[i].bs[j].gameObject.SetActive(false);
            }
            
        }
    }

    public void selectEvent(int eve)
    {
        locals[location].s[eve].go();
    }

}
[System.Serializable]
public class Location
{
#if UNITY_EDITOR
    public string name;
        #endif
    public byte useing;
    public Button[] bs;
    public TMP_Text[] btnText;
    public List<EventStart> s = new();
}
[System.Serializable]
public class Available
{
    public string message;
    public EventStart g;
}
