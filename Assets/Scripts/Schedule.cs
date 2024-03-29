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

    public CharacterList characters;

    public int location;

    public TMP_Text LocationName;
    public Canvas EventListCanvas;
    public Button[] EventButtons;
    public TMP_Text[] btnText;
    public Sprite[] LocationSprites;

    public GameObject ListCanvasBack;
    public Canvas LocationCanvas;
    public GameObject LocationFirstButton;
    public Image SelectedLocationImage;

    public GameObject DartButtonGameObject;

    public Image LocationLocationImage;
    public AudioClip song0;
    public AudioClip cli;

    public GameObject g;

    public TMP_Text TimeText;
    public Canvas TimeCanvas;

    public DartMen DartsMenu;

    public void Start()
    {
        Audio.inst.PlaySong(song0);
        TimeText.text = timeAsString();
        UI_Helper.SetSelectedUIElement(g);
        
        CutsceneHandler.inst.SetUpForMainGame(DartsMenu, this);
        characters = CutsceneHandler.inst.characters;
    }

    private string timeAsString()
    {
        return hour + ":" + (minutes==0?"00":minutes.ToString()) + "PM";
    }

    public void click()
    {
        Audio.inst.PlayClip(cli);
    } 

    public void ChooseCharacterGender(int i)
    {
        CutsceneHandler.inst.SetCharacterSprite(i);
        setTime(0);
        PauseMenu.inst.SetEnabled(true);
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

        for (int i = 0; i < 4; i++)
        {
            int availableCutSceneIndex = characters.list[i].GetCutScene();

            if (availableCutSceneIndex >= 0)
            {
                Locations locationIndex = characters.list[i].RelatedCutScenes[availableCutSceneIndex].CutsceneLocation;

                if (locationIndex != Locations.none)
                {

                    locals[(int)locationIndex].Events.Add(characters.list[i].RelatedCutScenes[availableCutSceneIndex].CutScene);

                    locals[(int)locationIndex].EventsButtonUsed++;
                }
            }
            else
            {
                if (characters.list[i].DefaultCutScene != null)
                {
                    int loungeIndex = (int)Locations.lounge;
                    locals[loungeIndex].Events.Add(characters.list[i].DefaultCutScene);
                    locals[loungeIndex].EventsButtonUsed++;
                }
            }



        }
    }

    public void setTime(TimeBlocks time)
    {
        Audio.inst.PlaySong(song0);
        LocationCanvas.enabled = true;

        //UI_Helper.SetSelectedUIElement(LocationFirstButton);
        increaseTimeByMinutes((int)time);

        if (hour >= 9)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneNumbers.DidNotWinTheTournament);
        }

        if (hour >= 8)
        {
            if (minutes >= 30)
            {
                Debug.Log("Now only duo dates should be available if they are not that is bug ");
                int numberAvailable = 0;
                for( int i = 0; i < 4;i++)
                {
                    if (characters.list[i].RelatedCutScenes[(int)PartnerCutscenes.FinalScene].completed && characters.list[i].Love>0)
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
        if (minutes >= 60)
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
            DartButtonGameObject.SetActive(true);

        location = b;
        LocationCanvas.enabled = false;
        EventListCanvas.enabled = true;
        //UI_Helper.SetSelectedUIElement(ListCanvasBack);
    }

    public void selectEvent(int eve)
    {
        doCheck(locals[location].Events[eve]);
        DartButtonGameObject.SetActive(false);
        EventListCanvas.enabled = false;
        CutsceneHandler.inst.PlayCutScene(locals[location].Events[eve],location);
    }

    public void off()
    {
        TimeCanvas.enabled = false;
        EventListCanvas.enabled = false;
        LocationCanvas.enabled = false;
        DartButtonGameObject.SetActive( false);
    }
    

    public void back()
    {
        EventListCanvas.enabled = false;
        LocationCanvas.enabled = true;
        DartButtonGameObject.SetActive( false);

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
