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

    public Partner[] partners;

    public byte location;
    public CutsceneHandler c;

    public TMP_Text LocationName;
    public Canvas EventListCanvas;
    public Button[] EventButtons;
    public TMP_Text[] btnText;
    public Sprite[] LocationSprites;

    public GameObject ListCanvasBack;
    public Canvas LocationCanvas;
    public GameObject LocationFirstButton;
    public Image SelectedLocationImage;
    


    public void Start()
    {
        setTime(0);
    }

    public void setTime(int minutes)
    {
        LocationCanvas.enabled = true;

        UI_Helper.SetSelectedUIElement(LocationFirstButton);
        timeM(minutes);
        for(int i = 0; i < locals.Length; i++)
        {
            locals[i].EventButtons = 0;
            locals[i].Events.Clear();
            locals[i].location.Clear();
        }

        for (int i = 0; i < available.Length; i++)
        {
            byte b = LocationOf(available[i]);


            if (b < 255)
            {
               
                locals[b].Events.Add(available[i].cutScene);
                locals[b].location.Add(available[i].location);

                locals[b].EventButtons++;
            }
        }

        for (int i = 0; i < partners.Length; i++)
        {
            int checkLove = partners[i].CheckLoveCutScene();
            if ( checkLove>= 0)
            {
                byte b = getLocal(partners[i].RelatedCutScenes[checkLove].Location);


                if (b < 255)
                {

                    locals[b].Events.Add(partners[i].RelatedCutScenes[checkLove].CutScene);
                    locals[b].location.Add(partners[i].RelatedCutScenes[checkLove].Location);

                    locals[b].EventButtons++;
                }
            }
            
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
        LocationName.text = locals[b].Name;
        Debug.Log(b);
        Debug.Log(LocationSprites[b].name);
        SelectedLocationImage.sprite = LocationSprites[b];

        for (int j = 0; j < EventButtons.Length; j++)
        {
            if (j < locals[b].EventButtons)
            {
                btnText[j].text = locals[b].Events[j].defaultCharacter;
                EventButtons[j].gameObject.SetActive(true);
            }
            else if (EventButtons[j].gameObject.activeSelf)
                EventButtons[j].gameObject.SetActive(false);
            else
                break;
        }
        location = (byte)b;
        LocationCanvas.enabled = false;
        EventListCanvas.enabled = true;
        UI_Helper.SetSelectedUIElement(ListCanvasBack);
    }

    public void selectEvent(int eve)
    {
        EventListCanvas.enabled = false;
        c.tart(locals[location].Events[eve],location);
    }

    public void back()
    {
        EventListCanvas.enabled = false;
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
    public string Name;
        #endif
    public byte EventButtons = 0;
  
    public List<CutScene> Events = new();
    public List<string> location = new();
}
