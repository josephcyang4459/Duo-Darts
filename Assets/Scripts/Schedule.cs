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

    public GameObject darts;

    public Image LocationLocationImage;
    public AudioSource ass;
    public AudioClip song0;
    public AudioClip cli;

    public Sprite[] mf;
    public Image plyr;
    public GameObject g;
    public TypeWriterEffect writer;

    public void Start()
    {

        UI_Helper.SetSelectedUIElement(g);
        Application.targetFrameRate = 60;
        ass.volume = PlayerPrefs.GetFloat("volume", .5f);
        writer.CustomWriteSpeed = PlayerPrefs.GetFloat("textSpeed", 10);
    }

    public void click()
    {
        ass.PlayOneShot(cli);
    } 

    public void mfc(int i)
    {
        plyr.sprite = mf[i];
        setTime(0);
    }

    public void setTime(int minutes)
    {
        if (ass.clip != song0)
        {
            ass.clip = song0;
            ass.Play();
        }
        LocationCanvas.enabled = true;

        UI_Helper.SetSelectedUIElement(LocationFirstButton);
        timeM(minutes);

        for(int i = 0; i < locals.Length; i++)
        {
            locals[i].EventsButtonUsed = 0;
            locals[i].Events = new();//eww gross why would josh have done this GROSSS ~josh
        }

        for (int i = 0; i < available.Length; i++)
        {
            byte b = LocationOf(available[i]);

            if (b < 255)
            {
                locals[b].Events.Add(available[i].cutScene);
                locals[b].EventsButtonUsed++;
            }
        }

        for (int i = 0; i < partners.Length; i++)
        {
            int checkLove = partners[i].GetCutScene();
           
            if ( checkLove>= 0)
            {
                int b = getLocal(partners[i].RelatedCutScenes[checkLove].Location);

                if (b < 255)
                {

                    locals[b].Events.Add(partners[i].RelatedCutScenes[checkLove].CutScene);

                    locals[b].EventsButtonUsed++;
                }
            }
            else
            {
                if (partners[i].DefaultCutScene != null)
                {
                    int b = getLocal("lounge");
                    locals[b].Events.Add(partners[i].DefaultCutScene);
                    locals[b].EventsButtonUsed++;
                }
            }
         

            
        }
    }

    private void doCheck(CutScene c)
    {

        for (int i = 0; i < available.Length; i++)
            if (available[i].cutScene == c)
                available[i].done = true;
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

    public void LocationImage(int b)
    {
        LocationLocationImage.sprite = LocationSprites[b];
    }

    public void selectLocation(int b)
    {
        LocationName.text = locals[b].Name;
        SelectedLocationImage.sprite = LocationSprites[b];

        for (int j = 0; j < EventButtons.Length; j++)
        {
            if (j < locals[b].EventsButtonUsed)
            {
                btnText[j].text = locals[b].Events[j].defaultCharacter;
                EventButtons[j].gameObject.SetActive(true);
            }
            else if (EventButtons[j].gameObject.activeSelf)
                EventButtons[j].gameObject.SetActive(false);
            else
                break;
        }

        if (b == 4)
            darts.SetActive(true);

        location = (byte)b;
        LocationCanvas.enabled = false;
        EventListCanvas.enabled = true;
        UI_Helper.SetSelectedUIElement(ListCanvasBack);
    }

    public void selectEvent(int eve)
    {
        doCheck(locals[location].Events[eve]);
        darts.SetActive(false);
        EventListCanvas.enabled = false;
        c.tart(locals[location].Events[eve],location);
    }

    public void off()
    {
        EventListCanvas.enabled = false;
        LocationCanvas.enabled = false;
        darts.SetActive( false);
    }
    
    public void quit()
    {
        Application.Quit();
    }

    public void back()
    {
        EventListCanvas.enabled = false;
        LocationCanvas.enabled = true;
        darts.SetActive( false);

        UI_Helper.SetSelectedUIElement(LocationFirstButton);
    }


    public void pick()
    {

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

    public string Name;

    public byte EventsButtonUsed = 0;
  
    public List<CutScene> Events = new();
}
