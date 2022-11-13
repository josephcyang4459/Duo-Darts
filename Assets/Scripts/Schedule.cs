using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Schedule : MonoBehaviour
{
    public int hour = 4;
    public int minutes =0;

    public EventStart[] available;

    public Location[] locals;

    public byte location;
    public CutsceneHandler c;

    public TMP_Text LocationName;
    public Canvas list;
    public Button[] bs;
    public TMP_Text[] btnText;

    public GameObject ListCanvasBack;
    public Canvas LocationCanvas;
    public GameObject LocationFirstButton;
    public void Start()
    {
        setTime(0);
    }

    public void setTime(int minutes)
    {
        timeM(minutes);
        for(int i = 0; i < locals.Length; i++)
        {
            locals[i].useing = 0;
            locals[i].s.Clear();
        }

        for (int i = 0; i < available.Length; i++)
        {
            byte b = LocationOf(available[i]);


            if (b < 255)
            {
               
                locals[b].s.Add(available[i]);
                locals[b].useing++;
            }
        }

        for (int i = 0; i < locals.Length; i++)
        {
           
            
        }
    }

    private void timeM(int times)
    {
        minutes += times;
        if (minutes > 60)
        {
            minutes -= 60;
            hour++;
        }
    }

    public void selectLocation(int b)
    {
        LocationName.text = locals[b].name;
        for (int j = 0; j < bs.Length; j++)
        {
            if (j < locals[b].useing)
            {
                btnText[j].text = locals[b].s[j].cutScene.defaultCharacter;
                bs[j].gameObject.SetActive(true);
            }
            else if (bs[j].gameObject.activeSelf)
                bs[j].gameObject.SetActive(false);
            else
                break;
        }
        location = (byte)b;
        LocationCanvas.enabled = false;
        list.enabled = true;
        UI_Helper.SetSelectedUIElement(ListCanvasBack);
    }

    public void selectEvent(int eve)
    {
        list.enabled = false;
        locals[location].s[eve].go(c, location);
    }

    public void back()
    {
        list.enabled = false;
        LocationCanvas.enabled = true;

        UI_Helper.SetSelectedUIElement(LocationFirstButton);
    }

    private bool check(EventStart t)
    {
        if (hour < t.hour)
            return false;

        if (hour == t.hour)
            if (minutes < t.minutes)
            return false;

        if (hour < t.endHour)
            return true;

        if (hour == t.endHour)
            if (minutes < t.endMinutes)
                return true;

        return false;
    }

    public byte LocationOf(EventStart e)
    {
        if (e.done)
            return 255;

        if (check(e))
        {

            return getLocal(e.location);

        }
        return 255;
    }

    private byte getLocal(string s)
    {
        switch (s.ToLower())
        {
            case "lounge":
                return 0;
            case "bar":
                return 1;
            case "dance":
                return 2;
            case "bathroom":
                return 3;
            case "darts":
                return 4;
        }
        return 255;
    }
}
[System.Serializable]
public class Location
{
#if UNITY_EDITOR
    public string name;
        #endif
    public byte useing = 0;
  
    public List<EventStart> s = new();
}
