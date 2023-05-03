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

    public int location;
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

    public TMP_Text TimeText;
    public Canvas TimeCanvas;

    public void Start()
    {
        TimeText.text = timeAsString();
        UI_Helper.SetSelectedUIElement(g);
        Application.targetFrameRate = 60;
        ass.volume = PlayerPrefs.GetFloat("volume", .5f);
        writer.CustomWriteSpeed = PlayerPrefs.GetFloat("textSpeed", 10);
    }

    private string timeAsString()
    {
        return hour + ":" + (minutes==0?"00":minutes.ToString()) + "PM";
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

    private void setLocations()
    {
        for (int i = 0; i < locals.Length; i++)
        {
            locals[i].EventsButtonUsed = 0;
            locals[i].Events = new();//eww gross why would josh have done this GROSSS ~josh
        }
        //events
        for (int i = 0; i < available.Length; i++)
        {
            Locations b = LocationOf(available[i]);

            if (b != Locations.none)
            {
                locals[(int)b].Events.Add(available[i].cutScene);
                locals[(int)b].EventsButtonUsed++;
            }
        }

        for (int i = 0; i < partners.Length; i++)
        {
            int availableCutSceneIndex = partners[i].GetCutScene();

            if (availableCutSceneIndex >= 0)
            {
                Locations locationIndex = partners[i].RelatedCutScenes[availableCutSceneIndex].CutsceneLocation;

                if (locationIndex != Locations.none)
                {

                    locals[(int)locationIndex].Events.Add(partners[i].RelatedCutScenes[availableCutSceneIndex].CutScene);

                    locals[(int)locationIndex].EventsButtonUsed++;
                }
            }
            else
            {
                if (partners[i].DefaultCutScene != null)
                {
                    int loungeIndex = (int)Locations.lounge;
                    locals[loungeIndex].Events.Add(partners[i].DefaultCutScene);
                    locals[loungeIndex].EventsButtonUsed++;
                }
            }



        }
    }

    public void setTime(int minutes)
    {
        if (ass.clip != song0)
        {
            ass.clip = song0;
            ass.Play();
        }
        LocationCanvas.enabled = true;

        //UI_Helper.SetSelectedUIElement(LocationFirstButton);
        increaseTimeByMinutes(minutes);
        
        if (hour >= 8)
        {
            if (minutes >= 30)
            {
                Debug.Log("Now only duo dates should be available if they are not that is bug ");
                int numberAvailable = 0;
                for( int i = 0; i < 4;i++)
                {
                    if (partners[i].RelatedCutScenes[(int)PartnerCutscenes.FinalScene].completed)
                    {
                        numberAvailable++;
                    }
                }
                if (numberAvailable <= 0)
                {
                    //Debug.Log("GAME OVER please send to end state");
                    UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneNumbers.NoLovers);
                    return;
                }
            }
        }

        setLocations();
    }

    private void doCheck(CutScene c)
    {

        for (int i = 0; i < available.Length; i++)
            if (available[i].cutScene == c)
                available[i].done = true;
    }

    private void increaseTimeByMinutes(int times)
    {
        minutes += times;
        if (minutes > 60)
        {
            minutes -= 60;
            hour++;
        }

        TimeCanvas.enabled = true;
        TimeText.text = timeAsString();
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

        location = b;
        LocationCanvas.enabled = false;
        EventListCanvas.enabled = true;
        //UI_Helper.SetSelectedUIElement(ListCanvasBack);
    }

    public void selectEvent(int eve)
    {
        doCheck(locals[location].Events[eve]);
        darts.SetActive(false);
        EventListCanvas.enabled = false;
        c.PlayCutScene(locals[location].Events[eve],location);
    }

    public void off()
    {
        TimeCanvas.enabled = false;
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

    private bool checkIfValidTime(EventStart t)
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

    public Locations LocationOf(EventStart eventP)
    {
        if (eventP.done)
            return Locations.none;

        if (checkIfValidTime(eventP))
        {

            return eventP.Location;
        }
        return Locations.none;
    }


}
[System.Serializable]
public class Location
{

    public string Name;

    public byte EventsButtonUsed = 0;
  
    public List<CutScene> Events = new();
}
